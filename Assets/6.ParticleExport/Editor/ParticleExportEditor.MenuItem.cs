using UnityEditor;
using UnityEngine;

public partial class ParticleExportEditor
{
    [MenuItem("Assets/LTools/ParticleExport/Export")]
    public static void ParticleExport()
    {
        if (Selection.gameObjects.Length <= 0)
            return;
        foreach (var go in Selection.gameObjects)
        {
            new ParticleExportEditor(go).Export();
        }
    }
}