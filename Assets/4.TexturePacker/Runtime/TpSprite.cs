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
        UpdateSprite();
    }

    public void SetSprite(string spriteName)
    {
        _spriteName = spriteName;
        UpdateSprite();
    }

    public void UpdateSprite(bool forceRefresh = false)
    {
        sprite = null;
        if (!enabled)
            return;
        if (_atlas == null || string.IsNullOrEmpty(_spriteName))
            return;
        sprite = _atlas.GetSprite(_spriteName, forceRefresh);
    }
}