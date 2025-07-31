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
        if (fontAsset == null) 
            return;
        var assetPath = AssetDatabase.GetAssetPath(fontAsset);
        var saveTexturePath = assetPath.Replace(".asset", ".png");
        var saveTexture = new Texture2D(fontAsset.atlasTexture.width, fontAsset.atlasTexture.height, TextureFormat.Alpha8, false);
        Graphics.CopyTexture(fontAsset.atlasTexture, saveTexture);
        var saveBytes = saveTexture.EncodeToPNG();
        if (File.Exists(saveTexturePath))
            File.Delete(saveTexturePath);
        File.WriteAllBytes(saveTexturePath, saveBytes);
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        // 正式打包时这里应该剔除
        // AssetDatabase.RemoveObjectFromAsset(fontAsset.atlasTexture);
        // Object.DestroyImmediate(fontAsset.atlasTexture, true); 
        var atlas = AssetDatabase.LoadAssetAtPath<Texture2D>(saveTexturePath);
        fontAsset.atlasTextures[0] = atlas;
        fontAsset.material.mainTexture = atlas;
        
        // var importer = (TextureImporter)AssetImporter.GetAtPath(saveTexturePath);
        // importer.isReadable = false;
        // string[] platforms = { "iPhone", "Android", "Standalone" };
        // int maxSize = 4096;
        // foreach (string plat in platforms)
        // {
        //     var platSetting = new TextureImporterPlatformSettings();
        //     platSetting.name = plat;
        //     platSetting.maxTextureSize = maxSize;
        //     platSetting.format = plat == "Standalone" ? TextureImporterFormat.BC7 : TextureImporterFormat.ASTC_5x5;
        //     platSetting.overridden = true;
        //     importer.SetPlatformTextureSettings(platSetting);
        // }
        // importer.SaveAndReimport();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}