using System.IO;
using Spine.Unity;
using UnityEditor;
using UnityEngine;

public class SpineEditor
{
    private static MeshFilter _meshFilter;
    private static MeshRenderer _meshRenderer;

    private static string _savePath;
    private static string _saveName;
    private static string _saveMeshPath;
    private static string _saveMainTexturePath;
    private static string _saveMaterialPath;


    [MenuItem("Assets/LTools/SpineGPU优化/GenMesh")]
    public static void GenMesh()
    {
        var select = Selection.activeGameObject;
        if (select == null)
            return;
        var skeletonAnimation = select.GetComponent<SkeletonAnimation>();
        _meshFilter = select.GetComponent<MeshFilter>();
        _meshRenderer = select.GetComponent<MeshRenderer>();
        if (_meshFilter == null || skeletonAnimation == null || _meshRenderer == null)
            return;
        _savePath = AssetDatabase.GetAssetPath(select).Replace(".prefab", "_GPU");
        _saveName = $"{select.name}_GPU";
        if (Directory.Exists(_savePath))
            Directory.Delete(_savePath, true);
        Directory.CreateDirectory(_savePath);
        CreateMesh();
        CreateMainTexture();
        // CreateAnimationTexture();
        CreateMaterial();
        CreatePrefab();
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
        var savePrefab = new GameObject();
        savePrefab.AddComponent<MeshFilter>().sharedMesh = AssetDatabase.LoadAssetAtPath<Mesh>(_saveMeshPath);
        savePrefab.AddComponent<MeshRenderer>().sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>(_saveMaterialPath);
        PrefabUtility.SaveAsPrefabAsset(savePrefab, $"{_savePath}/{_saveName}.prefab");
        Object.DestroyImmediate(savePrefab);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}