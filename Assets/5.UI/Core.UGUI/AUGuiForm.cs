using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LTools.UI
{
    public abstract class AUGuiForm : UIFormLogic
    {
        public const int DEPTH_FACTOR = 100;

        private Canvas _cachedCanvas = null;

        private readonly List<ParticleSystemRenderer> _cachedParticleSystemRenderersContainer = new();

        private readonly List<Canvas> _cachedCanvasContainer = new List<Canvas>();

        public int OriginalDepth { get; private set; }

        public int Depth => _cachedCanvas.sortingOrder;

        protected internal override void OnInit(object userData)
        {
            base.OnInit(userData);
            _cachedCanvas = gameObject.GetOrAddComponent<Canvas>();
            _cachedCanvas.overrideSorting = true;
            OriginalDepth = _cachedCanvas.sortingOrder;
            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            gameObject.GetOrAddComponent<GraphicRaycaster>();
        }

        protected internal override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            int oldDepth = Depth;
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
            int deltaDepth = UGuiGroupHelper.DEPTH_FACTOR * uiGroupDepth + DEPTH_FACTOR * depthInUIGroup - oldDepth + OriginalDepth;
            GetComponentsInChildren(true, _cachedCanvasContainer);
            for (int i = 0; i < _cachedCanvasContainer.Count; i++)
            {
                _cachedCanvasContainer[i].sortingOrder += deltaDepth;
            }
            _cachedCanvasContainer.Clear();
            GetComponentsInChildren(true, _cachedParticleSystemRenderersContainer);
            foreach (var t in _cachedParticleSystemRenderersContainer)
            {
                t.sortingOrder += deltaDepth;
            }
            _cachedParticleSystemRenderersContainer.Clear();
        }
    }
}