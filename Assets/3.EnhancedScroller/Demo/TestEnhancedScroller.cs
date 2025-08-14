using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class TestEnhancedScrollerData : BaseEnhancedScrollerData
{
    public int Index;
}

public class TestEnhancedScrollerLogic : BaseEnhancedScrollerCellLogic<TestEnhancedScrollerData>
{
    private TMPro.TextMeshProUGUI _textMeshPro;

    protected override void OnCreate()
    {
        _textMeshPro = Owner.GetComponentInChildren<TextMeshProUGUI>();
    }

    protected override void OnShow()
    {
        _textMeshPro.text = Data.Index.ToString();
    }

    protected override void OnRefresh()
    {
    }

    protected override void OnHide()
    {
    }
}

public class TestEnhancedScroller : MonoBehaviour
{
    private List<TestEnhancedScrollerData> _data = new();

    private void Start()
    {
        for (int i = 0; i < 10000; i++)
        {
            _data.Add(new TestEnhancedScrollerData() { Index = i });
        }

        var s = new EnhancedScrollerWidget(gameObject, 
            CreateCellLogicDelegate,
            GetNumberOfCellsDelegate,
            GetCellSizeDelegate, 
            GetCellAssetIndexDelegate,
            GetCellDataDelegate);
        
        s.ReloadData();
    }

    private BaseEnhancedScrollerData GetCellDataDelegate(int dataIndex)
    {
        return _data[dataIndex];
    }

    private int GetCellAssetIndexDelegate(int dataIndex)
    {
        return 0;
    }

    private float GetCellSizeDelegate(int dataIndex)
    {
        return 100;
    }

    private int GetNumberOfCellsDelegate()
    {
        return _data.Count;
    }

    private IEnhancedScrollerCellLogic CreateCellLogicDelegate(int dataIndex)
    {
        return new TestEnhancedScrollerLogic();
    }
}