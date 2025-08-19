using System.IO;
using TexturePackerImporter;
using UnityEditor;
using UnityEditor.U2D.Sprites;
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
        Debug.Log("★★CopyTexture★★");
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
                    Debug.Log($"Delete: {copyPath}");
                    Debug.Log($"Delete: {copyPath}.meta");
                }
                File.Copy(texturePath, copyPath, true);
                AssetDatabase.ImportAsset(copyPath);
                var import = (TextureImporter)AssetImporter.GetAtPath(copyPath);
                import.textureType = TextureImporterType.Default;
                import.isReadable = true;
                import.mipmapEnabled = false;
                import.alphaIsTransparency = true;
                import.SaveAndReimport();
            }
        }
    }

    private void CreateAtlas()
    {
        Debug.Log("★★CreateAtlas★★");
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
            var sheetPath = $"{_saveTexturePath}/{_packName}-{i}.tpsheet";
            if (!File.Exists(sheetPath))
                break;
            var s = new SpritesheetCollection();
            s.loadSheetData(sheetPath);
            var sheetInfo = s.sheetInfoForDataFile(sheetPath);
            if (sheetInfo != null)
            {
                foreach (var m in sheetInfo.metadata)
                    so.AddSprite(m, GetBorder(m), i);
            }
            so.AddTexture(AssetDatabase.LoadAssetAtPath<Texture2D>($"{SAVE_PATH}/{_packName}-{i}.png"));
        }
        so.DoSerialize();
        if (!hasSo)
            AssetDatabase.CreateAsset(so, tpAtlasPath);
        EditorUtility.SetDirty(so);
        AssetDatabase.SaveAssetIfDirty(so);
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private Vector4 GetBorder(SpriteMetaData meta)
    {
        var artSpriteName = meta.name;
        if (artSpriteName.Contains("-"))
            artSpriteName = artSpriteName.Split('-')[1];
        var artSpritePath = $"{_saveTexturePath}/{_packName}/{artSpriteName}.png";
        if (File.Exists(artSpritePath))
        {
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(artSpritePath);
            return sprite.border;
        }
        Debug.LogError($"不存在: {artSpritePath}");
        return Vector4.zero;
    }
}