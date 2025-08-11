using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

public class TexturePackerEditor
{
    [MenuItem("Assets/LTools/Texture Packer/PackFolder-无裁切")]
    public static void PackFolderNone()
    {
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        var cmd = new TexturePackerCmd(path);
        cmd.PackByCmd(TexturePackerCmd.AlgorithmType.None);
        new CreateTpAtlas(TexturePackerCmd.SAVE_PATH, cmd.PackName).Create(false);
    }

    [MenuItem("Assets/LTools/Texture Packer/PackFolder-矩形裁切丢弃位置")]
    public static void PackFolderRectDisPos()
    {
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        var cmd = new TexturePackerCmd(path);
        cmd.PackByCmd(TexturePackerCmd.AlgorithmType.RectTrimDisPos);
        new CreateTpAtlas(TexturePackerCmd.SAVE_PATH, cmd.PackName).Create(false);
    }

    [MenuItem("Assets/LTools/Texture Packer/PackFolder-矩形裁切保留位置")]
    public static void PackFolderRectKeepPos()
    {
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        var cmd = new TexturePackerCmd(path);
        cmd.PackByCmd(TexturePackerCmd.AlgorithmType.RectTrimKeepPos);
        new CreateTpAtlas(TexturePackerCmd.SAVE_PATH, cmd.PackName).Create(false);
    }

    [MenuItem("Assets/LTools/Texture Packer/PackFolder-多边形裁切")]
    public static void PackFolderPolygon()
    {
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        var cmd = new TexturePackerCmd(path);
        cmd.PackByCmd(TexturePackerCmd.AlgorithmType.PolygonTrim);
        new CreateTpAtlas(TexturePackerCmd.SAVE_PATH, cmd.PackName).Create(true);
    }
}