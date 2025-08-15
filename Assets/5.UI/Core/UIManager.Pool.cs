using System.Collections.Generic;
using UnityEngine;

namespace LTools.UI
{
    internal sealed partial class UIManager
    {
        private int _capacity;
        private float _autoReleaseInterval;
        private float _autoReleaseTime;

        private LinkedList<UIFormInstanceObject> _pool = new();

        private Dictionary<object, UIFormInstanceObject> _register = new();

        private void UpdatePool(float elapseSeconds, float realElapseSeconds)
        {
            _autoReleaseTime += realElapseSeconds;
            if (_autoReleaseTime < _autoReleaseInterval)
            {
                return;
            }

            Release(_pool.Count - _capacity);
        }

        public void SetPool(int capacity, float autoReleaseInterval)
        {
            _capacity = capacity;
            _autoReleaseInterval = autoReleaseInterval;
        }

        private void Release(int toReleaseCount)
        {
            _autoReleaseTime = 0;
            if (toReleaseCount <= 0)
                return;
            if (toReleaseCount > _pool.Count)
                toReleaseCount = _pool.Count;
            while (toReleaseCount-- > 0)
            {
                var value = _pool.First.Value;
                _pool.RemoveFirst();
                ReferencePool.Release(value);
            }
        }


        private void Register(UIFormInstanceObject poolObject)
        {
            _register.Add(poolObject.UIFormInstance, poolObject);
        }

        private UIFormInstanceObject Spawn(string uiFormAssetName)
        {
            var node = _pool.First;
            while (node != null) 
            {
                if (node.Value.UIFormAssetName == uiFormAssetName)
                {
                    _pool.Remove(node);
                    return node.Value;
                }
                node = node.Next;
            }
            return null;
        }

        private void UnSpawn(object instance)
        {
            if (_register.TryGetValue(instance, out var poolObject))
            {
                _pool.AddLast(poolObject);
            }
        }
    }
}