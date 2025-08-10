using UnityEngine;
using UnityEngine.UI;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// uGUI 界面组辅助器。
    /// </summary>
    public class UGuiGroupHelper : UIGroupHelperBase
    {
        public const int DEPTH_FACTOR = 1000;

        private int _depth = 0;
        private Canvas _cachedCanvas = null;

        /// <summary>
        /// 设置界面组深度。
        /// </summary>
        /// <param name="depth">界面组深度。</param>
        public override void SetDepth(int depth)
        {
            _depth = depth;
            _cachedCanvas.overrideSorting = true;
            _cachedCanvas.sortingOrder = DEPTH_FACTOR * depth;
        }

        private void Awake()
        {
            _cachedCanvas = gameObject.GetOrAddComponent<Canvas>();
            gameObject.GetOrAddComponent<GraphicRaycaster>();
        }

        private void Start()
        {
            _cachedCanvas.overrideSorting = true;
            _cachedCanvas.sortingOrder = DEPTH_FACTOR * _depth;

            RectTransform rectTransform = gameObject.GetOrAddComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.localPosition = Vector3.zero;
            rectTransform.localRotation = Quaternion.identity;
            rectTransform.localScale = Vector3.one;
        }
    }
}