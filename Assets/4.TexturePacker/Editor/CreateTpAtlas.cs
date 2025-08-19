using System.IO;
using TexturePackerImporter;
using UnityEditor;
using UnityEngine;

public class CreateTpAtlas
{
    private const string SAVE_PATH = @"Assets/4.TexturePacker/Bundles/TpAtlas";

    private string _saveTexturePath;
    private string _packName;

    public CreateTpAtlas(string saveTexturePath, string packName)
    {
        _saveTexturePath = saveTexturePath;
        _packName = packName;
    }

    public void Create()
    {
        CopyTexture();
        CreateAtlas();
    }

    private void CopyTexture()
    {
        // 假设最大一共999个图集
        for (int i = 0; i < 999; i++)
        {
            var texturePath = $"{_saveTexturePath}/{_packName}-{i}.png";
            if (File.Exists(texturePath))
            {
                var copyPath = $"{SAVE_PATH}/{_packName}-{i}.png";
                if (File.Exists(copyPath))
                {
                    File.Delete(copyPath);
                    File.Delete($"{copyPath}.meta");
                }
                File.Copy(texturePath, copyPath, true);
                AssetDatabase.ImportAsset(copyPath);
                var import = (TextureImporter)AssetImporter.GetAtPath(copyPath);
                import.textureType = TextureImporterType.Default;
                import.isReadable = true;
                import.alphaIsTransparency = true;
                import.SaveAndReimport();
            }
        }
    }

    private void CreateAtlas()
    {
        var tpAtlasPath = $"{SAVE_PATH}/{_packName}.asset";
        var hasSo = File.Exists(tpAtlasPath);
        TpSpriteAtlas so;
        if (hasSo)
            so = AssetDatabase.LoadAssetAtPath<TpSpriteAtlas>(tpAtlasPath);
        else
            so = ScriptableObject.CreateInstance<TpSpriteAtlas>();
        so.Clear();

        // 假设最大一共999个图集
        for (int i = 0; i < 999; i++)
        {
            var sourceTexturePath = $"{_saveTexturePath}/{_packName}-{i}.png";
            // var sprites = AssetDatabase.LoadAllAssetsAtPath(sourceTexturePath);
            // if (sprites == null || sprites.Length <= 0)
                // break;
            // so.AddSprites(sprites, i == 0);
            so.AddTexture(AssetDatabase.LoadAssetAtPath<Texture>($"{SAVE_PATH}/{_packName}-{i}.png"));
        }
        so.DoSerialize();
        if (!hasSo)
            AssetDatabase.CreateAsset(so, tpAtlasPath);
        EditorUtility.SetDirty(so);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void AddSprites(TpSpriteAtlas so, string sourceTexturePath)
    {
        var importer = (TextureImporter)AssetImporter.GetAtPath(sourceTexturePath);
        var sheet = TexturePackerImporter.TexturePackerImporter.getSheetInfo(importer);
    }
}