using System;
using System.Collections.Generic;
using System.IO;
using ProtoBuf;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;


[CreateAssetMenu(fileName = "TpSpriteAtlas", menuName = "Create/ScriptableObjects/TpSpriteAtlas", order = 1)]
public class TpSpriteAtlas : ScriptableObject
{
    [SerializeField, Searchable]
    private List<Texture2D> _textures = new();

    [SerializeField]
    private byte[] _tpSpriteAtlasProto;

    private TpSpriteAtlasProto _tpSpriteAtlasProtoInfo;

    private Dictionary<string, Sprite> _spriteDic = new();

    private Dictionary<string, OneSpriteInfo> _spriteInfoDic = new();

#if UNITY_EDITOR
    private TpSpriteAtlasProto _tempProto;
#endif

    public Sprite GetSprite(string spriteName, bool forceRefresh)
    {
        // 初始化
        if (_tpSpriteAtlasProtoInfo == null || forceRefresh)
        {
            if (_tpSpriteAtlasProto == null)
                return null;
            using var ms = new MemoryStream(_tpSpriteAtlasProto);
            _tpSpriteAtlasProtoInfo = Serializer.Deserialize<TpSpriteAtlasProto>(ms);
        }
        if (forceRefresh || (_spriteInfoDic.Count <= 0 && _tpSpriteAtlasProtoInfo != null && _tpSpriteAtlasProtoInfo.SpriteInfos.Count > 0))
        {
            _spriteInfoDic.Clear();
            foreach (var info in _tpSpriteAtlasProtoInfo.SpriteInfos)
                _spriteInfoDic.Add(info.Name, info);
        }
        if (_tpSpriteAtlasProtoInfo == null)
            return null;
        if (forceRefresh)
        {
            if (_spriteDic.TryGetValue(spriteName, out var exist))
            {
                if (Application.isPlaying)
                {
                    Destroy(exist);
                }
#if UNITY_EDITOR
                else
                {
                    DestroyImmediate(exist);
                }
#endif
                _spriteDic.Remove(spriteName);
            }
        }
        if (_spriteDic.TryGetValue(spriteName, out var sprite))
        {
            if (sprite != null)
                return sprite;
            _spriteDic.Remove(spriteName);
        }
        if (_spriteInfoDic.TryGetValue(spriteName, out var spriteInfo))
        {
            var newSprite = Sprite.Create(_textures[spriteInfo.TextureIndex],
                new Rect(spriteInfo.RectX, spriteInfo.RectY, spriteInfo.RectW, spriteInfo.RectH),
                new Vector2(spriteInfo.PivotX, spriteInfo.PivotY),
                100, 1, SpriteMeshType.Tight,
                new Vector4(spriteInfo.BorderX, spriteInfo.BorderY, spriteInfo.BorderZ, spriteInfo.BorderW));
            _spriteDic.Add(spriteName, newSprite);
            newSprite.name = spriteName;
            return newSprite;
        }
        Debug.LogError($"不存在sprite: {spriteName}");
        return null;
    }


    private void OnDestroy()
    {
        foreach (var sprite in _spriteDic.Values)
        {
            if (Application.isPlaying)
            {
                Destroy(sprite);
            }
#if UNITY_EDITOR
            else
            {
                DestroyImmediate(sprite);
            }
#endif
        }
        _spriteDic.Clear();
    }

#if UNITY_EDITOR
    public List<string> GetSpriteNames()
    {
        using var ms = new MemoryStream(_tpSpriteAtlasProto);
        var msg = Serializer.Deserialize<TpSpriteAtlasProto>(ms);
        var list = new List<string>();
        foreach (var s in msg.SpriteInfos)
            list.Add(s.Name);
        return list;
    }

    public void AddTexture(Texture2D texture)
    {
        _textures.Add(texture);
    }

    public void AddSprite(SpriteMetaData meta, Vector4 spriteBorder, int textureIndex)
    {
        if (_textures == null)
        {
            _textures = new();

            _tempProto = new();
            _tempProto.SpriteInfos = new();
        }

        var info = new OneSpriteInfo()
        {
            Name = meta.name,
            RectX = meta.rect.x,
            // tp的y和unity的y不一样
            // RectY = _mainTexture.height - meta.rect.y - meta.rect.height,
            RectY = meta.rect.y,
            RectH = meta.rect.height,
            RectW = meta.rect.width,
            Alignment = meta.alignment,
            BorderX = spriteBorder.x,
            BorderY = spriteBorder.y,
            BorderZ = spriteBorder.z,
            BorderW = spriteBorder.w,
            PivotX = meta.pivot.x,
            PivotY = meta.pivot.y,
            TextureIndex = textureIndex,
        };

        _tempProto.SpriteInfos.Add(info);
    }

    public void Clear()
    {
        _textures = null;
        _tpSpriteAtlasProto = null;
    }

    public void DoSerialize()
    {
        using var ms = new MemoryStream();
        Serializer.Serialize(ms, _tempProto);
        _tpSpriteAtlasProto = ms.ToArray();
    }
#endif
}