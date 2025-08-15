//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------


using System;

namespace LTools.UI
{
    internal sealed partial class UIManager
    {
        /// <summary>
        /// 界面实例对象。
        /// </summary>
        private sealed class UIFormInstanceObject : IReference
        {
            public object UIFormInstance {get; private set;}
            public string UIFormAssetName {get; private set;}
            
            private object _uiFormAsset = null;
            private IUIFormHelper _uiFormHelper = null;

            public static UIFormInstanceObject Create(string uiFormAssetName, object uiFormAsset, object uiFormInstance, IUIFormHelper uiFormHelper)
            {
                if (uiFormAsset == null)
                {
                    throw new Exception("UI form asset is invalid.");
                }

                if (uiFormHelper == null)
                {
                    throw new Exception("UI form helper is invalid.");
                }

                var uiFormInstanceObject = ReferencePool.Acquire<UIFormInstanceObject>();
                uiFormInstanceObject.UIFormInstance = uiFormInstance;
                uiFormInstanceObject.UIFormAssetName = uiFormAssetName;
                uiFormInstanceObject._uiFormAsset = uiFormAsset;
                uiFormInstanceObject._uiFormHelper = uiFormHelper;
                return uiFormInstanceObject;
            }

            public void Clear()
            {
                _uiFormHelper.ReleaseUIForm(_uiFormAsset, UIFormInstance);
                _uiFormAsset = null;
                _uiFormHelper = null;
            }
        }
    }
}
