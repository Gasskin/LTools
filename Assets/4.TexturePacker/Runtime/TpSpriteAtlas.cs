using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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

    public bool IsPolygon;
    
    private Dictionary<string, Sprite> _spriteDic = new();
    
#if UNITY_EDITOR
    private List<Sprite> _spriteClones = new();
    private HashSet<string> _nameCheck = new();
#endif
    
    public Sprite GetSprite(string spriteName)
    {
        _spriteDic.TryGetValue(spriteName, out var sprite);
        if (sprite == null)
        {
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

    private void OnDisable()
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
    
    
    public void AddSprites(Object[] sprite, bool init)
    {
        if (init)
        {
            _nameCheck = new();
            _sprites = new();
            _spriteNames = new();
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
    
    public void Clear()
    {
        _spriteNames = null;
        _sprites = null;
    }
    
#endif
}