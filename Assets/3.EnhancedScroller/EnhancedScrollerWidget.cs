using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

/// <summary>
/// 实例化一个Cell的Prefab时调用一次，创建与这个Cell关联的唯一Logic
/// Logic仅与Cell所代表的GameObject关联，不会和数据关联
/// </summary>
public delegate IEnhancedScrollerCellLogic CreateCellLogicDelegate(int dataIndex);

/// <summary>
/// 获取需要创建Cell的数量
/// </summary>
public delegate int GetNumberOfCellsDelegate();

/// <summary>
/// 获取Cell的高度，或者宽度
/// </summary>
public delegate float GetCellSizeDelegate(int dataIndex);

/// <summary>
/// 获取某个Cell的资源序号
/// 所有的Cell引用都在EnhancedScroller界面上的Cells中
/// </summary>
public delegate int GetCellAssetIndexDelegate(int dataIndex);

/// <summary>
/// 获取某个Cell关联的数据
/// </summary>
public delegate BaseEnhancedScrollerData GetCellDataDelegate(int dataIndex);


public class EnhancedScrollerWidget : IEnhancedScrollerDelegate
{
    private GameObject _owner;
    private EnhancedScroller _scroller;
    
    private  CreateCellLogicDelegate _createCellLogicDelegate;
    private  GetNumberOfCellsDelegate _getNumberOfCellsDelegate;
    private  GetCellSizeDelegate _getCellSizeDelegate;
    private  GetCellAssetIndexDelegate _getCellAssetIndexDelegate;
    private  GetCellDataDelegate _getCellDataDelegate;
    
    // 所有生成的CellLogic，包括未激活的
    private readonly Dictionary<int, IEnhancedScrollerCellLogic> _allCellLogic = new();

    // 激活中的CellLogic
    private readonly Dictionary<int, IEnhancedScrollerCellLogic> _activeCellLogic = new();
    
    public EnhancedScrollerWidget(GameObject owner,
        CreateCellLogicDelegate createCellLogic,
        GetNumberOfCellsDelegate getNumberOfCells,
        GetCellSizeDelegate getCellSize,
        GetCellAssetIndexDelegate getCellAssetIndex,
        GetCellDataDelegate getCellData)
    {
        _owner = owner;
        
        _scroller = owner.GetComponent<EnhancedScroller>();
        _scroller.Delegate = this;
        
        _scroller.cellViewWillRecycle = OnCellViewWillRecycle;
        _scroller.cellViewInstantiated = OnCellViewInstantiated;
        _scroller.cellViewVisibilityChanged = OnCellViewVisibilityChanged;

        _createCellLogicDelegate = createCellLogic;
        _getNumberOfCellsDelegate = getNumberOfCells;
        _getCellSizeDelegate = getCellSize;
        _getCellAssetIndexDelegate = getCellAssetIndex;
        _getCellDataDelegate = getCellData;
    }

#region 接口
    /// <summary>
    /// 刷新，指定Scroller的百分比，0是头
    /// </summary>
    /// <param name="percentage"></param>
    public void ReloadData(float percentage = 0)
    {
        _scroller.ReloadData(percentage);
    }

    /// <summary>
    /// 以指定dataIndex为头开始刷新
    /// </summary>
    /// <param name="dataIdx"></param>
    public void ReloadDataFromDataIndex(int dataIdx)
    {
        if (dataIdx < 0) 
        {
            ReloadData();
            return;
        }
        _scroller.ReloadDataFromDataIndex(dataIdx);
    }

    /// <summary>
    /// 固定当前Scroller的位置刷新一次
    /// </summary>
    public void ReloadDataFixedPosition()
    {
        _scroller.ReloadDataFromDataIndex(_scroller.StartDataIndex);
    }
    
    public void ClearAll()
    {
        foreach (var item in _activeCellLogic.Values)
            item.Hide();
        _activeCellLogic.Clear();
        _allCellLogic.Clear();
        _scroller.ClearAll();
    }
    
    public void RefreshActive()
    {
        foreach (var logic in _activeCellLogic.Values)
            logic.Refresh();
    }
#endregion
    
#region 列表刷新
    private void OnCellViewVisibilityChanged(EnhancedScrollerCellView cellView)
    {
        var id = cellView.gameObject.GetInstanceID();
        if (cellView.active)
        {
            if (_allCellLogic.TryGetValue(id, out var item))
            {
                item.Show(_getCellDataDelegate?.Invoke(cellView.dataIndex), cellView.dataIndex);
                _activeCellLogic.Add(id, item);
            }
            else
                Debug.LogError($"不存在的ItemView：{cellView.dataIndex}");
        }
        else
        {
            _activeCellLogic.Remove(id);
        }
    }

    private void OnCellViewWillRecycle(EnhancedScrollerCellView cellView)
    {
        var id = cellView.gameObject.GetInstanceID();
        if (_allCellLogic.TryGetValue(id, out var item))
            item.Hide();
        else
            Debug.LogError($"不存在的ItemView：{cellView.dataIndex}");
    }

    private void OnCellViewInstantiated(EnhancedScroller scroller, EnhancedScrollerCellView cellView)
    {
        var id = cellView.gameObject.GetInstanceID();
        if (_allCellLogic.TryGetValue(id, out var item))
        {
            Debug.LogError($"Item已存在 dataIndex:{cellView.dataIndex}");
        }
        else
        {
            item = _createCellLogicDelegate?.Invoke(cellView.dataIndex);
            if (item != null) 
            {
                item.Init(this, cellView.gameObject);
                _allCellLogic.Add(id, item);
            }
        }
    }
#endregion


#region IEnhancedScrollerDelegate
    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        if (_getNumberOfCellsDelegate == null)
            return 0;
        return _getNumberOfCellsDelegate.Invoke();
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        if (_getCellSizeDelegate == null)
            return 0;
        return _getCellSizeDelegate.Invoke(dataIndex);
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        if (_getCellAssetIndexDelegate == null)
            return null;
        var index = _getCellAssetIndexDelegate.Invoke(dataIndex);
        return scroller.GetCellView(scroller.Cells[index], dataIndex);
    }
#endregion
}