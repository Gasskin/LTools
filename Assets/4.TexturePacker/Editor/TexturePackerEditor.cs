using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

public class TexturePackerEditor
{
    public const string ART_ATLAS_PATH = "Assets/4.TexturePacker/Art/TpAtlas";

    [MenuItem("Assets/LTools/Texture Packer/PackFolder-无裁切")]
    public static void PackFolderNone()
    {
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        var cmd = new TexturePackerCmd();
        cmd.PackFolder(path, ART_ATLAS_PATH, TexturePackerCmd.AlgorithmType.None);
        new CreateTpAtlas(ART_ATLAS_PATH, cmd.PackName).Create();
    }

    [MenuItem("Assets/LTools/Texture Packer/PackFolder-矩形裁切丢弃位置")]
    public static void PackFolderRectDisPos()
    {
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        var cmd = new TexturePackerCmd();
        cmd.PackFolder(path, ART_ATLAS_PATH, TexturePackerCmd.AlgorithmType.RectTrimDisPos);
        new CreateTpAtlas(ART_ATLAS_PATH, cmd.PackName).Create();
    }

    [MenuItem("Assets/LTools/Texture Packer/PackFolder-矩形裁切保留位置")]
    public static void PackFolderRectKeepPos()
    {
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        var cmd = new TexturePackerCmd();
        cmd.PackFolder(path, ART_ATLAS_PATH, TexturePackerCmd.AlgorithmType.RectTrimKeepPos);
        new CreateTpAtlas(ART_ATLAS_PATH, cmd.PackName).Create();
    }

    // [MenuItem("Assets/LTools/Texture Packer/PackFolder-多边形裁切")]
    // public static void PackFolderPolygon()
    // {
    // var path = AssetDatabase.GetAssetPath(Selection.activeObject);
    // var cmd = new TexturePackerCmd(path, SAVE_PATH);
    // cmd.PackByCmd(TexturePackerCmd.AlgorithmType.PolygonTrim);
    // new CreateTpAtlas(SAVE_PATH, cmd.PackName).Create(true);
    // }

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