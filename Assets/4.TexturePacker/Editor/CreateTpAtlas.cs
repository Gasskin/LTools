using System.IO;
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
            var texturePath = $"{_saveTexturePath}/{_packName}-{i}.png";
            var sprites = AssetDatabase.LoadAllAssetsAtPath(texturePath);
            if (sprites == null || sprites.Length <= 0)
                break;
            so.AddSprites(sprites, i == 0);
        }
        if (!hasSo)
            AssetDatabase.CreateAsset(so, tpAtlasPath);
        EditorUtility.SetDirty(so);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}