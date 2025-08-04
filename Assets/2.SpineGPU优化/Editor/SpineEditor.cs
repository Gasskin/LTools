using System.Collections.Generic;
using System.IO;
using Spine.Unity;
using UnityEditor;
using UnityEngine;

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
    }


    private static void CreateMesh()
    {
        _saveMeshPath = $"{_savePath}/{_saveName}_mesh.asset";
        var saveMesh = Object.Instantiate(_meshFilter.sharedMesh);
        AssetDatabase.CreateAsset(saveMesh, _saveMeshPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void CreateMainTexture()
    {
        _saveMainTexturePath = $"{_savePath}/{_saveName}_main_texture.png";
        var mainTextureSrcPath = AssetDatabase.GetAssetPath(_meshRenderer.sharedMaterial.mainTexture);
        File.Copy(mainTextureSrcPath, _saveMainTexturePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void CreateMaterial()
    {
        _saveMaterialPath = $"{_savePath}/{_saveName}_material.mat";
        var saveMaterial = new Material(Shader.Find("Spine/Skeleton"));
        saveMaterial.SetTexture(Shader.PropertyToID("_MainTex"), AssetDatabase.LoadAssetAtPath<Texture>(_saveMainTexturePath));
        AssetDatabase.CreateAsset(saveMaterial, _saveMaterialPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
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
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
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
        
        _skeletonAnimation.Initialize(true);
        var verticesList = new List<List<Vector3>>();
        // 第0帧，初始值
        AddVertices(verticesList, gpuAnimation);

        for (int i = 1; i <= frameCount; i++)
        {
            _skeletonAnimation.Update(frameDuration);
            _skeletonAnimation.LateUpdateMesh();
            AddVertices(verticesList, gpuAnimation);
        }
        var baseFrame = verticesList[0]; 
        var maxSize = Mathf.Max(baseFrame.Count, verticesList.Count);
        var textureSize = 4;
        while (textureSize < maxSize)
            textureSize += 4;
        var texture = new Texture2D(textureSize, textureSize, TextureFormat.RGBAFloat, false);
        for (int i = 0; i < verticesList.Count; i++) {
            var frame = verticesList[i];
            for (int j = 0; j < frame.Count; j++) {
                Vector3 v;

                if (i == 0) {
                    // 第0帧，写原始位置
                    v = baseFrame[j];
                } else {
                    // 其余帧，写偏移量
                    v = frame[j] - baseFrame[j];
                }

                int pixelIndex = i * baseFrame.Count + j;
                int x = pixelIndex % textureSize;
                int y = pixelIndex / textureSize;

                var normX = (v.x - gpuAnimation.MinX) / (gpuAnimation.MaxX - gpuAnimation.MinX);
                var normY = (v.y - gpuAnimation.MinY) / (gpuAnimation.MaxY - gpuAnimation.MinY);
                var color = new Color(normX, normY, 0, 1);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        var bytes = texture.EncodeToEXR(Texture2D.EXRFlags.OutputAsFloat);
        var savePath = $"{_savePath}/{_saveName}_anima_texture.exr";
        File.WriteAllBytes(savePath, bytes);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        gpuAnimation.AnimaTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(savePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void AddVertices(List<List<Vector3>> verticesList, GPUSkeletonAnimation animation)
    {
        var list = new List<Vector3>();
        var vertices = _meshFilter.sharedMesh.vertices;
        foreach (var v in vertices)
        {
            if (v.x > animation.MaxX)
                animation.MaxX = v.x;
            if (v.x < animation.MinX)
                animation.MinX = v.x;
            if (v.y > animation.MaxY)
                animation.MaxY = v.y;
            if (v.y < animation.MinY)
                animation.MinY = v.y;
            list.Add(v);
        }
        verticesList.Add(list);
    }
}