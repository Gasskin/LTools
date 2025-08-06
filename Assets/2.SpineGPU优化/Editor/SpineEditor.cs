using System;
using System.Collections.Generic;
using System.IO;
using Spine.Unity;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public partial class SpineEditor
{
    private static MeshFilter _meshFilter;
    private static MeshRenderer _meshRenderer;
    private static SkeletonAnimation _skeletonAnimation;

    private static string _savePath;
    private static string _saveName;
    private static string _saveMeshPath;
    private static string _saveMainTexturePath;
    private static string _saveMaterialPath;
    private static string _savePrefabPath;
    private static string _saveAnimationTexture;

    [MenuItem("Assets/LTools/SpineGPU优化/Gen")]
    public static void GenMesh()
    {
        var select = Selection.activeGameObject;
        if (select == null)
            return;
        _skeletonAnimation = select.GetComponent<SkeletonAnimation>();
        _meshFilter = select.GetComponent<MeshFilter>();
        _meshRenderer = select.GetComponent<MeshRenderer>();
        if (_meshFilter == null || _skeletonAnimation == null || _meshRenderer == null)
            return;
        _savePath = AssetDatabase.GetAssetPath(select).Replace(".prefab", "_GPU");
        _saveName = $"{select.name}_GPU";
        if (Directory.Exists(_savePath))
            Directory.Delete(_savePath, true);
        Directory.CreateDirectory(_savePath);
        CreateMesh();
        CreateMainTexture();
        CreateMaterial();
        CreatePrefab();
        CreateAnimationTexture();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }


    private static void CreateMesh()
    {
        _saveMeshPath = $"{_savePath}/{_saveName}_mesh.asset";
        var saveMesh = Object.Instantiate(_meshFilter.sharedMesh);
        AssetDatabase.CreateAsset(saveMesh, _saveMeshPath);
        AssetDatabase.ImportAsset(_saveMeshPath);
    }

    private static void CreateMainTexture()
    {
        _saveMainTexturePath = $"{_savePath}/{_saveName}_main_texture.png";
        var mainTextureSrcPath = AssetDatabase.GetAssetPath(_meshRenderer.sharedMaterial.mainTexture);
        File.Copy(mainTextureSrcPath, _saveMainTexturePath);
        AssetDatabase.ImportAsset(_saveMainTexturePath);
        AssetDatabase.Refresh();
    }

    private static void CreateMaterial()
    {
        _saveMaterialPath = $"{_savePath}/{_saveName}_material.mat";
        var saveMaterial = new Material(Shader.Find("Spine/Skeleton"));
        saveMaterial.SetTexture(Shader.PropertyToID("_MainTex"), AssetDatabase.LoadAssetAtPath<Texture>(_saveMainTexturePath));
        AssetDatabase.CreateAsset(saveMaterial, _saveMaterialPath);
        AssetDatabase.ImportAsset(_saveMaterialPath);
    }


    private static void CreatePrefab()
    {
        _savePrefabPath = $"{_savePath}/{_saveName}.prefab";
        var savePrefab = new GameObject();
        savePrefab.AddComponent<MeshFilter>().sharedMesh = AssetDatabase.LoadAssetAtPath<Mesh>(_saveMeshPath);
        savePrefab.AddComponent<MeshRenderer>().sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>(_saveMaterialPath);
        savePrefab.AddComponent<GPUSkeletonAnimation>();
        PrefabUtility.SaveAsPrefabAsset(savePrefab, _savePrefabPath);
        Object.DestroyImmediate(savePrefab);
        AssetDatabase.ImportAsset(_savePrefabPath);
    }

    public static void CreateAnimationTexture()
    {
        var select = Selection.activeGameObject;
        if (select == null)
            return;
        var savePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(_savePrefabPath);
        var gpuAnimation = savePrefab.GetComponent<GPUSkeletonAnimation>();

        var animations = _skeletonAnimation.skeletonDataAsset.GetSkeletonData(true).Animations;
        var main = animations.Find((animation => animation.Name == "main"));
        if (main == null)
            return;
        var framePerSecond = 60;
        var frameDuration = 1f / framePerSecond;
        var frameCount = Mathf.CeilToInt(main.Duration / frameDuration);
        gpuAnimation.FrameCount = frameCount + 1;
        gpuAnimation.FramePerSecond = framePerSecond;
        gpuAnimation.MaxX = gpuAnimation.MaxY = float.MinValue;
        gpuAnimation.MinX = gpuAnimation.MinY = float.MaxValue;

        _skeletonAnimation.Initialize(true);
        // 第0帧的初始值
        var frameOneVertices = new List<Vector3>();
        _meshFilter.sharedMesh.GetVertices(frameOneVertices);
        gpuAnimation.FrameVertices.Add(new OneFrameVertices() { Vertices = frameOneVertices });
        FindMaxAndMin(gpuAnimation, frameOneVertices);
        // 后续帧
        for (int i = 1; i <= frameCount; i++)
        {
            _skeletonAnimation.Update(frameDuration);
            _skeletonAnimation.LateUpdateMesh();
            var frameVertices = new List<Vector3>();
            frameVertices.AddRange(_meshFilter.sharedMesh.vertices);
            gpuAnimation.FrameVertices.Add(new OneFrameVertices() { Vertices = frameVertices });
            FindMaxAndMin(gpuAnimation, frameVertices);
        }

        // 所有数据映射为0-1之间
        for (int i = 0; i < gpuAnimation.FrameVertices.Count; i++)
        {
            var oneFrameVertices = gpuAnimation.FrameVertices[i].Vertices;
            for (var j = 0; j < oneFrameVertices.Count; j++)
            {
                var vertex = oneFrameVertices[j];
                var newVertex = new Vector3();
                // 映射到0-1之间
                newVertex.x = (vertex.x - gpuAnimation.MinX) / (gpuAnimation.MaxX - gpuAnimation.MinX);
                newVertex.y = (vertex.y - gpuAnimation.MinY) / (gpuAnimation.MaxY - gpuAnimation.MinY);
                newVertex.z = 0;
                oneFrameVertices[j] = newVertex;
            }
            // AddOneFrameVerticesToTexture(texture, i, oneFrameVertices);
        }
        var texture = new Texture2D(_meshFilter.sharedMesh.vertices.Length, _meshFilter.sharedMesh.vertices.Length, TextureFormat.RGBA32, false);
        var color32Array = new Color32[texture.GetPixels().Length];
        for (int i = 0; i < _meshFilter.sharedMesh.vertices.Length; i++)
        {
            for (var j = 0; j < _meshFilter.sharedMesh.vertices.Length; j++)
            {
                var index = i * _meshFilter.sharedMesh.vertices.Length + j;
                var empty = i >= gpuAnimation.FrameVertices.Count;
                if (empty)
                {
                    color32Array[index] = new Color32(0,0,0,0);
                }
                else
                {
                    var oneFrameVertices = gpuAnimation.FrameVertices[i].Vertices;
                    var vertex = oneFrameVertices[j];
                    var (x1, x2) = PackFloat2Bit88(vertex.x);
                    var (y1, y2) = PackFloat2Bit88(vertex.y);
                    color32Array[index] = new Color32(x1, x2, y1, y2);
                }
            }
        }
        texture.SetPixels32(color32Array);
        texture.Apply();

        var bytes = texture.EncodeToPNG();
        var savePath = $"{_savePath}/{_saveName}_anima_texture.png";
        File.WriteAllBytes(savePath, bytes);
        AssetDatabase.ImportAsset(savePath);

        gpuAnimation.AnimaTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(savePath);

        var textureImporter = (TextureImporter)AssetImporter.GetAtPath(savePath);
        // 设置可读、wrap/filter模式（这些不能通过 settings 设置）
        textureImporter.isReadable = true;
        textureImporter.wrapMode = TextureWrapMode.Repeat;
        textureImporter.filterMode = FilterMode.Point;
        textureImporter.textureType = TextureImporterType.Default;
        // 设置基础导入属性
        var settings = new TextureImporterSettings();
        textureImporter.ReadTextureSettings(settings);
        settings.sRGBTexture = false;
        settings.spriteMode = 0;
        settings.mipmapEnabled = false;
        settings.alphaSource = TextureImporterAlphaSource.FromInput;
        settings.alphaIsTransparency = false;
        textureImporter.SetTextureSettings(settings);
        // 设置平台格式
        var standaloneSettings = new TextureImporterPlatformSettings
        {
            name = "Standalone",
            overridden = true,
            format = TextureImporterFormat.RGBA32
        };
        textureImporter.SetPlatformTextureSettings(standaloneSettings);
        textureImporter.SaveAndReimport();

        //2
        // var saveTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(savePath);
        // Debug.LogError($"贴图格式：{saveTexture.format}");
        // for (int i = 0; i < frameCount; i++)
        // {
        //     var vertices = gpuAnimation.FrameVertices[i].Vertices;
        //     for (int j = 0; j < _meshFilter.sharedMesh.vertices.Length; j++)
        //     {
        //         Debug.LogError($"帧{i}点{j} {vertices[j]}===={saveTexture.GetPixel(i, j)}");
        //     }
        // }
    }

    private static void FindMaxAndMin(GPUSkeletonAnimation gpuAnimation, List<Vector3> oneFrameVertices)
    {
        foreach (var vertex in oneFrameVertices)
        {
            if (vertex.x > gpuAnimation.MaxX)
            {
                gpuAnimation.MaxX = vertex.x;
            }
            if (vertex.y > gpuAnimation.MaxY)
            {
                gpuAnimation.MaxY = vertex.y;
            }
            if (vertex.x < gpuAnimation.MinX)
            {
                gpuAnimation.MinX = vertex.x;
            }
            if (vertex.y < gpuAnimation.MinY)
            {
                gpuAnimation.MinY = vertex.y;
            }
        }
    }

    private static void AddOneFrameVerticesToTexture(Texture2D texture, int frame, List<Vector3> frameVertices)
    {
        for (int i = 0; i < frameVertices.Count; i++)
        {
            var vertex = frameVertices[i];
            var color = new Color(vertex.x, vertex.y, 0, 1);
            texture.SetPixel(i, frame, color);
        }
    }

    private static (byte, byte) PackFloat2Bit88(float source)
    {
        // 转为 16 位整数
        int intVal = Mathf.RoundToInt(source * 65535f);
        // 高8位
        var x = (byte)((intVal >> 8) & 0xFF);
        // 低8位
        var y = (byte)(intVal & 0xFF);
        return (x, y);
    }
}