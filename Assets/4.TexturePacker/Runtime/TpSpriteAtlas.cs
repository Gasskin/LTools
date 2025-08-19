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
    public int Count => _spriteNames.Count;

    [SerializeField, Searchable]
    private List<string> _spriteNames = new();

    [SerializeField, Searchable]
    private List<Sprite> _sprites = new();

    [SerializeField, Searchable]
    private List<Texture> _textures = new();

    [SerializeField]
    private byte[] _tpSpriteAtlasProto;

    private Dictionary<string, Sprite> _spriteDic = new();

#if UNITY_EDITOR
    private List<Sprite> _spriteClones = new();
    private HashSet<string> _nameCheck = new();
    private TpSpriteAtlasProto _tempProto;
#endif

    public Sprite GetSprite(string spriteName)
    {
        if (!_spriteDic.TryGetValue(spriteName, out var sprite) || sprite == null)
        {
            if (sprite == null)
                _spriteDic.Remove(spriteName);
            for (int i = 0; i < _spriteNames.Count; i++)
            {
                if (_spriteNames[i] == spriteName)
                {
                    sprite = _sprites[i];
                    _spriteDic.Add(spriteName, sprite);
                    break;
                }
            }
        }
        if (sprite != null)
        {
#if UNITY_EDITOR
            var o = Instantiate(sprite);
            _spriteClones.Add(o);
            return o;
#endif
            return sprite;
        }
        Debug.LogError($"不存在sprite: {spriteName}");
        return null;
    }

    private void OnDestroy()
    {
#if UNITY_EDITOR
        foreach (var sprite in _spriteClones)
        {
            DestroyImmediate(sprite);
        }
        _spriteClones.Clear();
#endif
    }

#if UNITY_EDITOR
    public Sprite GetSpriteByIndex(int idx)
    {
        if (idx >= _sprites.Count)
        {
            return null;
        }
        return _sprites[idx];
    }

    public void AddTexture(Texture texture)
    {
        _textures.Add(texture);
    }

    public void AddSprites(Object[] sprite, bool init)
    {
        if (init)
        {
            _nameCheck = new();
            _sprites = new();
            _spriteNames = new();
            _textures = new();
        }

        if (sprite == null || sprite.Length <= 0)
        {
            return;
        }

        // 第0个是资源本身，剩下的是子资源，也就是sprite
        for (int i = 1; i < sprite.Length; i++)
        {
            if (sprite[i] is Sprite s)
            {
                if (!_nameCheck.Add(s.name))
                {
                    Debug.LogError($"重复的名称：{s.name}");
                    continue;
                }
                _sprites.Add(s);
                _spriteNames.Add(s.name);
            }
        }
    }

    public void AddSprite(SpriteMetaData meta, Vector4 spriteBorder, bool init)
    {
        if (init)
        {
            _nameCheck = new();
            _sprites = new();
            _spriteNames = new();
            _textures = new();
            _tempProto = new();
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
        };
        
        _tempProto.SpriteInfos.Add(info);
    }

    public void Clear()
    {
        _textures = null;
        _spriteNames = null;
        _sprites = null;
        _tpSpriteAtlasProto = null;
        _tempProto = null;
    }

    public void DoSerialize()
    {
        using var ms = new MemoryStream();
        Serializer.Serialize(ms, _tempProto);
        _tpSpriteAtlasProto = ms.ToArray();
    }
#endif
}