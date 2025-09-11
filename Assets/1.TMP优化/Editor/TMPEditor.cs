using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;

public class TMPEditor
{
    [MenuItem("Assets/TextMeshPro/导出静态贴图")]
        public static void ExportOne()
        {
            var fontAsset = Selection.activeObject as TMP_FontAsset;
            if (fontAsset == null)
            {
                return;
            }
            
            var confirm = EditorUtility.DisplayDialog(
                "导出静态贴图",                      
                "导出静态字体的贴图，只可以对静态字体使用，是否确定？",     
                "确定",                         
                "取消"                         
            );

            if (!confirm)
            {
                return;
            }

            Export(AssetDatabase.GetAssetPath(fontAsset));
        }


        public static void ExportAllInFolder()
        {
            
        }


        private static void Export(string assetPath)
        {
            var fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(assetPath);
            var saveTexturePath = assetPath.Replace(".asset", ".png");
            var saveTexture = new Texture2D(fontAsset.atlasTexture.width, fontAsset.atlasTexture.height, TextureFormat.Alpha8, false);
            Graphics.CopyTexture(fontAsset.atlasTexture, saveTexture);
            var saveBytes = saveTexture.EncodeToPNG();
            if (File.Exists(saveTexturePath))
                File.Delete(saveTexturePath);
            File.WriteAllBytes(saveTexturePath, saveBytes);
            
            AssetDatabase.ImportAsset(saveTexturePath);
            var atlas = AssetDatabase.LoadAssetAtPath<Texture2D>(saveTexturePath);
            fontAsset.atlasTextures[0] = atlas;
            fontAsset.material.mainTexture = atlas;
            
            AssetDatabase.RemoveObjectFromAsset(fontAsset.atlasTexture);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            SetCommon(saveTexturePath);
            SetWindows(saveTexturePath);
            SetAndroid(saveTexturePath);
            SetIOS(saveTexturePath);
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
            win.maxTextureSize = 4096;
            win.resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
            win.textureCompression = TextureImporterCompression.Uncompressed;
            win.format = TextureImporterFormat.Alpha8;
    
            ti.SetPlatformTextureSettings(win);
            ti.SaveAndReimport();
        }
        
        private static void SetAndroid(string texturePath)
        {
            var ti = (TextureImporter)AssetImporter.GetAtPath(texturePath);
            var android = ti.GetPlatformTextureSettings("Android");
            android.overridden = true;
            android.maxTextureSize = 4096;
            android.resizeAlgorithm = TextureResizeAlgorithm.Mitchell;

            android.format = TextureImporterFormat.ASTC_6x6;
            android.textureCompression = TextureImporterCompression.Compressed;
            android.crunchedCompression = false;
            android.compressionQuality = 100;
            
            ti.SetPlatformTextureSettings(android);
            ti.SaveAndReimport();
        }

        private static void SetIOS(string texturePath)
        {
            var ti = (TextureImporter)AssetImporter.GetAtPath(texturePath);

            var ios = ti.GetPlatformTextureSettings("iPhone");
            ios.overridden = true;
            ios.maxTextureSize = 4096;
            ios.resizeAlgorithm = TextureResizeAlgorithm.Mitchell;

            ios.format = TextureImporterFormat.ASTC_6x6;
            ios.textureCompression = TextureImporterCompression.Compressed;
            ios.crunchedCompression = false;
            ios.compressionQuality = 100;

            ti.SetPlatformTextureSettings(ios);
            ti.SaveAndReimport();
        }
    }
}
