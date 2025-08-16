//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using LTools.UI;
using System.Collections.Generic;
using Cysharp.Text;
using UnityEngine;

namespace LTools.UI
{
    /// <summary>
    /// 界面组件。
    /// </summary>

    [DisallowMultipleComponent]
    public sealed partial class UIComponent : MonoBehaviour
    {
        public static UIComponent Instance { get; private set; }
        
        private UIManager _uiManager = null;

        private IUIResourceLoader _resourceLoader;
        // private EventComponent m_EventComponent = null;

        private readonly List<IUIForm> _uiFormGetHelper = new List<IUIForm>();

        [SerializeField]
        private bool _enableOpenUIFormSuccessEvent = true;

        [SerializeField]
        private bool _enableOpenUIFormFailureEvent = true;

        [SerializeField]
        private bool _enableCloseUIFormCompleteEvent = true;

        [SerializeField]
        private float _autoReleaseInterval = 60f;

        [SerializeField]
        private int _capacity = 16;

        [SerializeField]
        private UIGroup[] _uiGroups = null;

        /// <summary>
        /// 获取界面组数量。
        /// </summary>
        public int UIGroupCount => _uiManager.UIGroupCount;


        public event EventHandler<OpenUIFormSuccessEventArgs> OpenUIFormSuccess
        {
            add
            {
                if (_enableOpenUIFormSuccessEvent)
                {
                    _uiManager.OpenUIFormSuccess += value;
                }
            }
            remove
            {
                if (_enableOpenUIFormSuccessEvent)
                {
                    _uiManager.OpenUIFormSuccess -= value;
                }
            }
        }
        
        public event EventHandler<OpenUIFormFailureEventArgs> OpenUIFormFailure
        {
            add
            {
                if (_enableOpenUIFormFailureEvent)
                {
                    _uiManager.OpenUIFormFailure += value;
                }
            }
            remove
            {
                if (_enableOpenUIFormFailureEvent)
                {
                    _uiManager.OpenUIFormFailure -= value;
                }
            }
        }
        
        public event EventHandler<CloseUIFormCompleteEventArgs> CloseUIFormComplete
        {
            add
            {
                if (_enableCloseUIFormCompleteEvent)
                {
                    _uiManager.CloseUIFormComplete += value;
                }
            }
            remove
            {
                if (_enableCloseUIFormCompleteEvent)
                {
                    _uiManager.CloseUIFormComplete -= value;
                }
            }
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        private void Awake()
        {
            _uiManager = new UIManager();
            // 替换项目自己的资源加载器
            _resourceLoader = new UIAssetLoader();
            _uiManager.SetResourceLoader(_resourceLoader);
            _uiManager.SetPool(_capacity, _autoReleaseInterval);
        }

        private void Start()
        {
            var uiFormHelper = new GameObject().AddComponent<UGuiUIFormHelper>();
            uiFormHelper.SetResourceLoader(_resourceLoader);
            uiFormHelper.name = "UI Form Helper";
            uiFormHelper.transform.SetParent(this.transform);
            uiFormHelper.transform.localScale = Vector3.one;

            _uiManager.SetUIFormHelper(uiFormHelper);

            SetRoot();
            SetGroup();

            Instance = this;

            OpenUIForm("Assets\\5.UI\\Demo\\TestLoginWindow.prefab", "Default", true);
        }


        private void Update()
        {
            _uiManager.Update(Time.deltaTime,Time.unscaledDeltaTime);
        }

    #region UIGroup
        /// <summary>
        /// 是否存在界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>是否存在界面组。</returns>
        public bool HasUIGroup(string uiGroupName)
        {
            return _uiManager.HasUIGroup(uiGroupName);
        }

        /// <summary>
        /// 获取界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>要获取的界面组。</returns>
        public IUIGroup GetUIGroup(string uiGroupName)
        {
            return _uiManager.GetUIGroup(uiGroupName);
        }

        /// <summary>
        /// 获取所有界面组。
        /// </summary>
        /// <returns>所有界面组。</returns>
        public IUIGroup[] GetAllUIGroups()
        {
            return _uiManager.GetAllUIGroups();
        }

        /// <summary>
        /// 获取所有界面组。
        /// </summary>
        /// <param name="results">所有界面组。</param>
        public void GetAllUIGroups(List<IUIGroup> results)
        {
            _uiManager.GetAllUIGroups(results);
        }

        /// <summary>
        /// 增加界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>是否增加界面组成功。</returns>
        public bool AddUIGroup(string uiGroupName)
        {
            return AddUIGroup(uiGroupName, 0);
        }

        /// <summary>
        /// 增加界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="depth">界面组深度。</param>
        /// <returns>是否增加界面组成功。</returns>
        public bool AddUIGroup(string uiGroupName, int depth)
        {
            if (_uiManager.HasUIGroup(uiGroupName))
            {
                return false;
            }

            var uiGroupHelper = new GameObject().AddComponent<UGuiGroupHelper>();
            uiGroupHelper.name = ZString.Format("UI Group - {0}", uiGroupName);
            uiGroupHelper.gameObject.layer = LayerMask.NameToLayer("UI");
            uiGroupHelper.transform.SetParent(_instanceRoot);
            uiGroupHelper.transform.localScale = Vector3.one;

            return _uiManager.AddUIGroup(uiGroupName, depth, uiGroupHelper);
        }
    #endregion

    #region UIForm
        /// <summary>
        /// 是否存在界面。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <returns>是否存在界面。</returns>
        public bool HasUIForm(int serialId)
        {
            return _uiManager.HasUIForm(serialId);
        }

        /// <summary>
        /// 是否存在界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>是否存在界面。</returns>
        public bool HasUIForm(string uiFormAssetName)
        {
            return _uiManager.HasUIForm(uiFormAssetName);
        }

        /// <summary>
        /// 获取界面。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <returns>要获取的界面。</returns>
        public UIForm GetUIForm(int serialId)
        {
            return (UIForm)_uiManager.GetUIForm(serialId);
        }

        /// <summary>
        /// 获取界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>要获取的界面。</returns>
        public UIForm GetUIForm(string uiFormAssetName)
        {
            return (UIForm)_uiManager.GetUIForm(uiFormAssetName);
        }

        /// <summary>
        /// 获取界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>要获取的界面。</returns>
        public UIForm[] GetUIForms(string uiFormAssetName)
        {
            IUIForm[] uiForms = _uiManager.GetUIForms(uiFormAssetName);
            UIForm[] uiFormImpls = new UIForm[uiForms.Length];
            for (int i = 0; i < uiForms.Length; i++)
            {
                uiFormImpls[i] = (UIForm)uiForms[i];
            }

            return uiFormImpls;
        }

        /// <summary>
        /// 获取界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="results">要获取的界面。</param>
        public void GetUIForms(string uiFormAssetName, List<UIForm> results)
        {
            if (results == null)
            {
                Debug.LogError("Results is invalid.");
                return;
            }

            results.Clear();
            _uiManager.GetUIForms(uiFormAssetName, _uiFormGetHelper);
            foreach (IUIForm uiForm in _uiFormGetHelper)
            {
                results.Add((UIForm)uiForm);
            }
        }

        /// <summary>
        /// 获取所有已加载的界面。
        /// </summary>
        /// <returns>所有已加载的界面。</returns>
        public UIForm[] GetAllLoadedUIForms()
        {
            IUIForm[] uiForms = _uiManager.GetAllLoadedUIForms();
            UIForm[] uiFormImpls = new UIForm[uiForms.Length];
            for (int i = 0; i < uiForms.Length; i++)
            {
                uiFormImpls[i] = (UIForm)uiForms[i];
            }

            return uiFormImpls;
        }

        /// <summary>
        /// 获取所有已加载的界面。
        /// </summary>
        /// <param name="results">所有已加载的界面。</param>
        public void GetAllLoadedUIForms(List<UIForm> results)
        {
            if (results == null)
            {
                Debug.LogError("Results is invalid.");
                return;
            }

            results.Clear();
            _uiManager.GetAllLoadedUIForms(_uiFormGetHelper);
            foreach (IUIForm uiForm in _uiFormGetHelper)
            {
                results.Add((UIForm)uiForm);
            }
        }

        /// <summary>
        /// 获取所有正在加载界面的序列编号。
        /// </summary>
        /// <returns>所有正在加载界面的序列编号。</returns>
        public int[] GetAllLoadingUIFormSerialIds()
        {
            return _uiManager.GetAllLoadingUIFormSerialIds();
        }

        /// <summary>
        /// 获取所有正在加载界面的序列编号。
        /// </summary>
        /// <param name="results">所有正在加载界面的序列编号。</param>
        public void GetAllLoadingUIFormSerialIds(List<int> results)
        {
            _uiManager.GetAllLoadingUIFormSerialIds(results);
        }

        /// <summary>
        /// 是否正在加载界面。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <returns>是否正在加载界面。</returns>
        public bool IsLoadingUIForm(int serialId)
        {
            return _uiManager.IsLoadingUIForm(serialId);
        }

        /// <summary>
        /// 是否正在加载界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>是否正在加载界面。</returns>
        public bool IsLoadingUIForm(string uiFormAssetName)
        {
            return _uiManager.IsLoadingUIForm(uiFormAssetName);
        }

        /// <summary>
        /// 是否是合法的界面。
        /// </summary>
        /// <param name="uiForm">界面。</param>
        /// <returns>界面是否合法。</returns>
        public bool IsValidUIForm(UIForm uiForm)
        {
            return _uiManager.IsValidUIForm(uiForm);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="priority">加载界面资源的优先级。</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面的序列编号。</returns>
        public int OpenUIForm(string uiFormAssetName, string uiGroupName, bool pauseCoveredUIForm = false, object userData = null)
        {
            return _uiManager.OpenUIForm(uiFormAssetName, uiGroupName, pauseCoveredUIForm, userData);
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="serialId">要关闭界面的序列编号。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void CloseUIForm(int serialId, object userData = null)
        {
            _uiManager.CloseUIForm(serialId, userData);
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="uiForm">要关闭的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void CloseUIForm(UIForm uiForm, object userData = null)
        {
            _uiManager.CloseUIForm(uiForm, userData);
        }

        /// <summary>
        /// 关闭所有已加载的界面。
        /// </summary>
        public void CloseAllLoadedUIForms()
        {
            _uiManager.CloseAllLoadedUIForms();
        }

        /// <summary>
        /// 关闭所有已加载的界面。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        public void CloseAllLoadedUIForms(object userData)
        {
            _uiManager.CloseAllLoadedUIForms(userData);
        }

        /// <summary>
        /// 关闭所有正在加载的界面。
        /// </summary>
        public void CloseAllLoadingUIForms()
        {
            _uiManager.CloseAllLoadingUIForms();
        }

        /// <summary>
        /// 激活界面。
        /// </summary>
        /// <param name="uiForm">要激活的界面。</param>
        public void RefocusUIForm(UIForm uiForm)
        {
            _uiManager.RefocusUIForm(uiForm);
        }

        /// <summary>
        /// 激活界面。
        /// </summary>
        /// <param name="uiForm">要激活的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void RefocusUIForm(UIForm uiForm, object userData)
        {
            _uiManager.RefocusUIForm(uiForm, userData);
        }
    #endregion
    }
}