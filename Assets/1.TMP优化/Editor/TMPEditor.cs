using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;

public class TMPEditor
{
    [MenuItem("Assets/LTools/TMP优化/GenTMPTexture")]
    public static void Gen()
    {
        var fontAsset = Selection.activeObject as TMP_FontAsset;
        if (fontAsset == null) return;
        var assetPath = AssetDatabase.GetAssetPath(fontAsset);
        var saveTexturePath = assetPath.Replace(".asset", ".png");
        var saveTexture = new Texture2D(fontAsset.atlasTexture.width, fontAsset.atlasTexture.height, TextureFormat.Alpha8, false);
        Graphics.CopyTexture(fontAsset.atlasTexture, saveTexture);
        var saveBytes = saveTexture.EncodeToPNG();
        if (File.Exists(saveTexturePath))
            File.Delete(saveTexturePath);
        File.WriteAllBytes(saveTexturePath, saveBytes);
        AssetDatabase.Refresh();
        var atlas = AssetDatabase.LoadAssetAtPath<Texture2D>(saveTexturePath);
        fontAsset.atlasTextures[0] = atlas;
        fontAsset.material.mainTexture = atlas;
        AssetDatabase.RemoveObjectFromAsset(fontAsset.atlasTexture);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        SetCommon(saveTexturePath);
        SetWindows(saveTexturePath);
    }

    private static void SetCommon(string texturePath)
    {
        var ti = (TextureImporter)AssetImporter.GetAtPath(texturePath);
        ti.textureType = TextureImporterType.Default;
        ti.sRGBTexture = false;
        ti.alphaSource = TextureImporterAlphaSource.FromInput;
        ti.alphaIsTransparency = false;
        ti.mipmapEnabled = false;
        ti.npotScale = TextureImporterNPOTScale.None;
        ti.wrapMode = TextureWrapMode.Clamp;
        ti.filterMode = FilterMode.Bilinear;
        ti.SaveAndReimport();
    }

    private static void SetWindows(string texturePath)
    {
        var ti = (TextureImporter)AssetImporter.GetAtPath(texturePath);
        var win = ti.GetPlatformTextureSettings("Standalone");
        win.overridden = true;
        win.maxTextureSize = ti.maxTextureSize;
        win.textureCompression = TextureImporterCompression.Uncompressed;
        win.format = TextureImporterFormat.BC7;
        win.maxTextureSize = 4096;
        win.resizeAlgorithm = TextureResizeAlgorithm.Mitchell;

        ti.SetPlatformTextureSettings(win);
        ti.SaveAndReimport();
    }
}
