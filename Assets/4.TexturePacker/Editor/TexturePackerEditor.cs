using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

public class TexturePackerEditor
{
    public const string ART_ATLAS_PATH = "Assets/4.TexturePacker/Art/Atlas";

    [MenuItem("Assets/LTools/Texture Packer/PackFolder-无裁切")]
    public static void PackFolderNone()
    {
        if (Selection.activeObject == null)
            return;
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (!AssetDatabase.IsValidFolder(path) || !path.Contains(ART_ATLAS_PATH))
            return;
        var cmd = new TexturePackerCmd();
        cmd.PackFolder(path, ART_ATLAS_PATH, TexturePackerCmd.AlgorithmType.None);
        new CreateTpAtlas(ART_ATLAS_PATH, cmd.PackName).Create();
    }

    [MenuItem("Assets/LTools/Texture Packer/PackFolder-矩形裁切丢弃位置")]
    public static void PackFolderRectDisPos()
    {
        if (Selection.activeObject == null)
            return;
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (!AssetDatabase.IsValidFolder(path) || !path.Contains(ART_ATLAS_PATH))
            return;
        var cmd = new TexturePackerCmd();
        cmd.PackFolder(path, ART_ATLAS_PATH, TexturePackerCmd.AlgorithmType.RectTrimDisPos);
        new CreateTpAtlas(ART_ATLAS_PATH, cmd.PackName).Create();
    }

    [MenuItem("Assets/LTools/Texture Packer/PackFolder-矩形裁切保留位置")]
    public static void PackFolderRectKeepPos()
    {
        if (Selection.activeObject == null)
            return;
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (!AssetDatabase.IsValidFolder(path) || !path.Contains(ART_ATLAS_PATH))
            return;
        var cmd = new TexturePackerCmd();
        cmd.PackFolder(path, ART_ATLAS_PATH, TexturePackerCmd.AlgorithmType.RectTrimKeepPos);
        new CreateTpAtlas(ART_ATLAS_PATH, cmd.PackName).Create();
    }

    [MenuItem("Assets/LTools/Texture Packer/PackOneImagePolygonTrim")]
    public static void GenOutline()
    {
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        var cmd = new TexturePackerCmd();
        cmd.PackOneImagePolygonTrim(path);
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
    
    
}