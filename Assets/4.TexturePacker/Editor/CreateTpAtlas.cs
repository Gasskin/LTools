using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateTpAtlas
{
    private const string SAVE_PATH = @"Assets/4.TexturePacker/TpAtlas";
    
    private string _saveTexturePath;
    private string _packName;

    public CreateTpAtlas(string saveTexturePath, string packName)
    {
        _saveTexturePath = saveTexturePath;
        _packName = packName;
    }

    public void Create(bool isPolygon)
    {
        var tpAtlasPath = $"{SAVE_PATH}/{_packName}.asset";
        if (File.Exists(tpAtlasPath))
            File.Delete(tpAtlasPath);
        
        var so = ScriptableObject.CreateInstance<TpSpriteAtlas>();
        so.IsPolygon = isPolygon;
        
        // 假设最大一共999个图集
        for (int i = 0; i < 999; i++)
        {
            var texturePath = $"{_saveTexturePath}/{_packName}-{i}.png";
            var sprite = AssetDatabase.LoadAllAssetsAtPath(texturePath);
            if (sprite == null || sprite.Length <= 0) 
                break;
            so.AddSprites(sprite, i == 0);
        }
        
        AssetDatabase.CreateAsset(so, tpAtlasPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}