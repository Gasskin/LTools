using UnityEditor;
using UnityEngine;

public class SpineEditor
{
    [MenuItem("Assets/LTools/SpineGPU优化/GenMesh")]
    public static void GenMesh()
    {
        var select = Selection.activeGameObject;
        if (select == null)
            return;
        var meshFilter = select.GetComponent<MeshFilter>();
        if (meshFilter == null)
            return;
        var saveMesh = Object.Instantiate(meshFilter.sharedMesh);
        var saveMeshPath = AssetDatabase.GetAssetPath(select) + $"{select.name}_mesh.asset";
        AssetDatabase.CreateAsset(saveMesh, saveMeshPath);
        AssetDatabase.SaveAssets();
    }
}