using UnityEngine;
using UnityEngine.UI;

public class TpSprite : Image
{
    [SerializeField]
    private TpSpriteAtlas _atlas;

    [SerializeField]
    private string _spriteName;

    protected override void OnEnable()
    {
        base.OnEnable();
        useSpriteMesh = _atlas.IsPolygon;
        UpdateSprite();
    }

    public void SetSprite(string spriteName)
    {
        _spriteName = spriteName;
        UpdateSprite();
    }

    public void UpdateSprite()
    {
        sprite = null;
        if (_atlas == null || string.IsNullOrEmpty(_spriteName))
            return;
        sprite = _atlas.GetSprite(_spriteName);
    }
}