using UnityEngine;

public interface IEnhancedScrollerCellLogic
{
    public void Init(EnhancedScrollerWidget ownerScroller, GameObject owner);
    public void Show(BaseEnhancedScrollerData data, int dataIndex);
    public void Refresh();
    public void Hide();
}


public abstract class BaseEnhancedScrollerCellLogic<T> : IEnhancedScrollerCellLogic where T : BaseEnhancedScrollerData
{
    protected EnhancedScrollerWidget OwnerScroller;
    protected T Data;
    protected int DataIndex;
    protected GameObject Owner;

    public void Init(EnhancedScrollerWidget ownerScroller, GameObject owner)
    {
        OwnerScroller = ownerScroller;
        Owner = owner;
        OnCreate();
    }

    public void Show(BaseEnhancedScrollerData data, int dataIndex)
    {
        Data = (T)data;
        DataIndex = dataIndex;
        OnShow();
    }

    public void Refresh()
    {
        OnRefresh();
    }

    public void Hide()
    {
        OnHide();
        Data = null;
        DataIndex = -1;
    }

    /// <summary>
    /// 实例化一个新的Prefab时调用一次
    /// </summary>
    protected abstract void OnCreate();

    /// <summary>
    /// 显示时调用
    /// </summary>
    protected abstract void OnShow();

    /// <summary>
    /// 刷新时调用
    /// </summary>
    protected abstract void OnRefresh();

    /// <summary>
    /// 隐藏时调用
    /// </summary>
    protected abstract void OnHide();
}