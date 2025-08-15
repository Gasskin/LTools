//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.UI;

namespace LTools.UI
{
    public sealed partial class UIComponent 
    {
        [SerializeField]
        private Camera _uiCamera;

        [SerializeField]
        private Transform _instanceRoot = null;

        private Canvas _rootCanvas;
        private CanvasScaler _canvasScaler;

        [SerializeField]
        private int _standardWidth;

        [SerializeField]
        private int _standardHeight;

        private Rect _safeArea;

        private void SetRoot()
        {
            _rootCanvas = _instanceRoot.GetComponent<Canvas>();
            _canvasScaler = _instanceRoot.GetComponent<CanvasScaler>();
            
            _safeArea = Screen.safeArea;
            _canvasScaler.referenceResolution = new Vector2(_standardWidth, _standardHeight);
            var standardVerticalRatio = 1f * _standardHeight / _standardWidth;
            var screenSafeRatio = _safeArea.height / _safeArea.width;
            _canvasScaler.matchWidthOrHeight = screenSafeRatio > standardVerticalRatio ? 0 : 1;
        }
    }
}
