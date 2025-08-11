using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

public class TexturePackerEditor
{
    private const string SAVE_PATH = @"Assets/4.TexturePacker/Art";
    
    [MenuItem("Assets/LTools/Texture Packer/PackFolder-无裁切")]
    public static void PackFolderNone()
    {
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        var cmd = new TexturePackerCmd(path, SAVE_PATH);
        cmd.PackByCmd(TexturePackerCmd.AlgorithmType.None);
        new CreateTpAtlas(SAVE_PATH, cmd.PackName).Create(false);
    }

    [MenuItem("Assets/LTools/Texture Packer/PackFolder-矩形裁切丢弃位置")]
    public static void PackFolderRectDisPos()
    {
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        var cmd = new TexturePackerCmd(path, SAVE_PATH);
        cmd.PackByCmd(TexturePackerCmd.AlgorithmType.RectTrimDisPos);
        new CreateTpAtlas(SAVE_PATH, cmd.PackName).Create(false);
    }

    [MenuItem("Assets/LTools/Texture Packer/PackFolder-矩形裁切保留位置")]
    public static void PackFolderRectKeepPos()
    {
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        var cmd = new TexturePackerCmd(path, SAVE_PATH);
        cmd.PackByCmd(TexturePackerCmd.AlgorithmType.RectTrimKeepPos);
        new CreateTpAtlas(SAVE_PATH, cmd.PackName).Create(false);
    }

    [MenuItem("Assets/LTools/Texture Packer/PackFolder-多边形裁切")]
    public static void PackFolderPolygon()
    {
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        var cmd = new TexturePackerCmd(path, SAVE_PATH);
        cmd.PackByCmd(TexturePackerCmd.AlgorithmType.PolygonTrim);
        new CreateTpAtlas(SAVE_PATH, cmd.PackName).Create(true);
    }

    [MenuItem("Assets/LTools/Texture Packer/GenOutline")]
    public static void GenOutline()
    {
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        var cmd = new TexturePackerCmd(path, Path.GetDirectoryName(path));
        cmd.PackByCmd(TexturePackerCmd.AlgorithmType.PolygonTrim, false);
        // File.Delete(Path.Combine(SAVE_PATH, cmd.PackName) + "-0.tpsheet");
        // File.Delete(Path.Combine(SAVE_PATH, cmd.PackName) + "-0.tpsheet.meta");
        // File.Copy(Path.Combine(SAVE_PATH, cmd.PackName) + "-0.png", path, true);
        // File.Delete(Path.Combine(SAVE_PATH, cmd.PackName) + "-0.png.meta");
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
}