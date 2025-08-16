using System.Collections.Generic;
using UnityEngine;

namespace LTools.UI
{
    public class WidgetContainer : IReference
    {
        private List<BaseWidget> _widgets = new();

        private BaseWindow _parentWindow;

    #region Reference
        public static WidgetContainer Create(BaseWindow parent)
        {
            var r = ReferencePool.Acquire<WidgetContainer>();
            r._parentWindow = parent;
            return r;
        }

        public void Clear()
        {
            _widgets.Clear();
        }
    #endregion

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            foreach (var w in _widgets)
            {
                w.OnUpdate(elapseSeconds, realElapseSeconds);
            }
        }

        public T AddOneWidget<T>(BaseWidget widget) where T : BaseWidget
        {
            var t = widget as T;
            if (t == null)
            {
                Debug.LogError("AddOneWidget: 非法转换");
                return null;
            }
            if (_widgets.Contains(t))
            {
                Debug.LogError("AddOneWidget: 重复添加");
                return null;
            }
            t.Create(_parentWindow);
            _widgets.Add(t);
            return t;
        }
    }
}