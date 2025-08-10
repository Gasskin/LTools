//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using Cysharp.Text;

namespace GameFramework.UI
{
    /// <summary>
    /// 界面管理器。
    /// </summary>
    internal sealed partial class UIManager
    {
        private readonly Dictionary<string, UIGroup> _uiGroups;
        private readonly Dictionary<int, string> _uiFormsBeingLoaded;
        private readonly HashSet<int> _uiFormsToReleaseOnLoad;

        private readonly Queue<IUIForm> _recycleQueue;

        // private readonly LoadAssetCallbacks m_LoadAssetCallbacks;
        // private IObjectPoolManager m_ObjectPoolManager;
        private IUIResourceLoader _resourceLoader;

        // private IObjectPool<UIFormInstanceObject> m_InstancePool;
        private IUIFormHelper _uiFormHelper;
        private int _serial;
        private bool _isShutdown;
        private EventHandler<OpenUIFormSuccessEventArgs> _openUIFormSuccessEventHandler;
        private EventHandler<OpenUIFormFailureEventArgs> _openUIFormFailureEventHandler;
        // private EventHandler<OpenUIFormUpdateEventArgs> _openUIFormUpdateEventHandler;
        // private EventHandler<OpenUIFormDependencyAssetEventArgs> _openUIFormDependencyAssetEventHandler;
        private EventHandler<CloseUIFormCompleteEventArgs> _closeUIFormCompleteEventHandler;

        /// <summary>
        /// 初始化界面管理器的新实例。
        /// </summary>
        public UIManager()
        {
            _uiGroups = new Dictionary<string, UIGroup>(StringComparer.Ordinal);
            _uiFormsBeingLoaded = new Dictionary<int, string>();
            _uiFormsToReleaseOnLoad = new HashSet<int>();
            _recycleQueue = new Queue<IUIForm>();
            // m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadAssetSuccessCallback, LoadAssetFailureCallback, LoadAssetUpdateCallback, LoadAssetDependencyAssetCallback);
            // m_ObjectPoolManager = null;
            _resourceLoader = null;
            // m_InstancePool = null;
            _uiFormHelper = null;
            _serial = 0;
            _isShutdown = false;
            _openUIFormSuccessEventHandler = null;
            _openUIFormFailureEventHandler = null;
            // _openUIFormUpdateEventHandler = null;
            // _openUIFormDependencyAssetEventHandler = null;
            _closeUIFormCompleteEventHandler = null;
        }

        /// <summary>
        /// 获取界面组数量。
        /// </summary>
        public int UIGroupCount => _uiGroups.Count;

        /// <summary>
        /// 打开界面成功事件。
        /// </summary>
        public event EventHandler<OpenUIFormSuccessEventArgs> OpenUIFormSuccess
        {
            add => _openUIFormSuccessEventHandler += value;
            remove => _openUIFormSuccessEventHandler -= value;
        }

        /// <summary>
        /// 打开界面失败事件。
        /// </summary>
        public event EventHandler<OpenUIFormFailureEventArgs> OpenUIFormFailure
        {
            add => _openUIFormFailureEventHandler += value;
            remove => _openUIFormFailureEventHandler -= value;
        }

        // /// <summary>
        // /// 打开界面更新事件。
        // /// </summary>
        // public event EventHandler<OpenUIFormUpdateEventArgs> OpenUIFormUpdate
        // {
        //     add => _openUIFormUpdateEventHandler += value;
        //     remove => _openUIFormUpdateEventHandler -= value;
        // }
        //
        // /// <summary>
        // /// 打开界面时加载依赖资源事件。
        // /// </summary>
        // public event EventHandler<OpenUIFormDependencyAssetEventArgs> OpenUIFormDependencyAsset
        // {
        //     add { _openUIFormDependencyAssetEventHandler += value; }
        //     remove { _openUIFormDependencyAssetEventHandler -= value; }
        // }

        /// <summary>
        /// 关闭界面完成事件。
        /// </summary>
        public event EventHandler<CloseUIFormCompleteEventArgs> CloseUIFormComplete
        {
            add { _closeUIFormCompleteEventHandler += value; }
            remove { _closeUIFormCompleteEventHandler -= value; }
        }

        /// <summary>
        /// 界面管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal void Update(float elapseSeconds, float realElapseSeconds)
        {
            while (_recycleQueue.Count > 0)
            {
                IUIForm uiForm = _recycleQueue.Dequeue();
                uiForm.OnRecycle();
                UnSpawn(uiForm.Handle);
            }

            foreach (KeyValuePair<string, UIGroup> uiGroup in _uiGroups)
            {
                uiGroup.Value.Update(elapseSeconds, realElapseSeconds);
            }
            
            UpdatePool(elapseSeconds, realElapseSeconds);
        }

        /// <summary>
        /// 关闭并清理界面管理器。
        /// </summary>
        internal void Shutdown()
        {
            _isShutdown = true;
            CloseAllLoadedUIForms();
            _uiGroups.Clear();
            _uiFormsBeingLoaded.Clear();
            _uiFormsToReleaseOnLoad.Clear();
            _recycleQueue.Clear();
        }


        /// <summary>
        /// 设置资源管理器。
        /// </summary>
        /// <param name="resourceManager">资源管理器。</param>
        public void SetResourceLoader(IUIResourceLoader resourceManager)
        {
            if (resourceManager == null)
            {
                throw new Exception("Resource manager is invalid.");
            }

            _resourceLoader = resourceManager;
        }

        /// <summary>
        /// 设置界面辅助器。
        /// </summary>
        /// <param name="uiFormHelper">界面辅助器。</param>
        public void SetUIFormHelper(IUIFormHelper uiFormHelper)
        {
            if (uiFormHelper == null)
            {
                throw new Exception("UI form helper is invalid.");
            }

            _uiFormHelper = uiFormHelper;
        }

        /// <summary>
        /// 是否存在界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>是否存在界面组。</returns>
        public bool HasUIGroup(string uiGroupName)
        {
            if (string.IsNullOrEmpty(uiGroupName))
            {
                throw new Exception("UI group name is invalid.");
            }

            return _uiGroups.ContainsKey(uiGroupName);
        }

        /// <summary>
        /// 获取界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <returns>要获取的界面组。</returns>
        public IUIGroup GetUIGroup(string uiGroupName)
        {
            if (string.IsNullOrEmpty(uiGroupName))
            {
                throw new Exception("UI group name is invalid.");
            }

            UIGroup uiGroup = null;
            if (_uiGroups.TryGetValue(uiGroupName, out uiGroup))
            {
                return uiGroup;
            }

            return null;
        }

        /// <summary>
        /// 获取所有界面组。
        /// </summary>
        /// <returns>所有界面组。</returns>
        public IUIGroup[] GetAllUIGroups()
        {
            int index = 0;
            IUIGroup[] results = new IUIGroup[_uiGroups.Count];
            foreach (KeyValuePair<string, UIGroup> uiGroup in _uiGroups)
            {
                results[index++] = uiGroup.Value;
            }

            return results;
        }

        /// <summary>
        /// 获取所有界面组。
        /// </summary>
        /// <param name="results">所有界面组。</param>
        public void GetAllUIGroups(List<IUIGroup> results)
        {
            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<string, UIGroup> uiGroup in _uiGroups)
            {
                results.Add(uiGroup.Value);
            }
        }

        /// <summary>
        /// 增加界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="uiGroupHelper">界面组辅助器。</param>
        /// <returns>是否增加界面组成功。</returns>
        public bool AddUIGroup(string uiGroupName, IUIGroupHelper uiGroupHelper)
        {
            return AddUIGroup(uiGroupName, 0, uiGroupHelper);
        }

        /// <summary>
        /// 增加界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="uiGroupDepth">界面组深度。</param>
        /// <param name="uiGroupHelper">界面组辅助器。</param>
        /// <returns>是否增加界面组成功。</returns>
        public bool AddUIGroup(string uiGroupName, int uiGroupDepth, IUIGroupHelper uiGroupHelper)
        {
            if (string.IsNullOrEmpty(uiGroupName))
            {
                throw new Exception("UI group name is invalid.");
            }

            if (uiGroupHelper == null)
            {
                throw new Exception("UI group helper is invalid.");
            }

            if (HasUIGroup(uiGroupName))
            {
                return false;
            }

            _uiGroups.Add(uiGroupName, new UIGroup(uiGroupName, uiGroupDepth, uiGroupHelper));

            return true;
        }

        /// <summary>
        /// 是否存在界面。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <returns>是否存在界面。</returns>
        public bool HasUIForm(int serialId)
        {
            foreach (KeyValuePair<string, UIGroup> uiGroup in _uiGroups)
            {
                if (uiGroup.Value.HasUIForm(serialId))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 是否存在界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>是否存在界面。</returns>
        public bool HasUIForm(string uiFormAssetName)
        {
            if (string.IsNullOrEmpty(uiFormAssetName))
            {
                throw new Exception("UI form asset name is invalid.");
            }

            foreach (KeyValuePair<string, UIGroup> uiGroup in _uiGroups)
            {
                if (uiGroup.Value.HasUIForm(uiFormAssetName))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取界面。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <returns>要获取的界面。</returns>
        public IUIForm GetUIForm(int serialId)
        {
            foreach (KeyValuePair<string, UIGroup> uiGroup in _uiGroups)
            {
                IUIForm uiForm = uiGroup.Value.GetUIForm(serialId);
                if (uiForm != null)
                {
                    return uiForm;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>要获取的界面。</returns>
        public IUIForm GetUIForm(string uiFormAssetName)
        {
            if (string.IsNullOrEmpty(uiFormAssetName))
            {
                throw new Exception("UI form asset name is invalid.");
            }

            foreach (KeyValuePair<string, UIGroup> uiGroup in _uiGroups)
            {
                IUIForm uiForm = uiGroup.Value.GetUIForm(uiFormAssetName);
                if (uiForm != null)
                {
                    return uiForm;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>要获取的界面。</returns>
        public IUIForm[] GetUIForms(string uiFormAssetName)
        {
            if (string.IsNullOrEmpty(uiFormAssetName))
            {
                throw new Exception("UI form asset name is invalid.");
            }

            List<IUIForm> results = new List<IUIForm>();
            foreach (KeyValuePair<string, UIGroup> uiGroup in _uiGroups)
            {
                results.AddRange(uiGroup.Value.GetUIForms(uiFormAssetName));
            }

            return results.ToArray();
        }

        /// <summary>
        /// 获取界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="results">要获取的界面。</param>
        public void GetUIForms(string uiFormAssetName, List<IUIForm> results)
        {
            if (string.IsNullOrEmpty(uiFormAssetName))
            {
                throw new Exception("UI form asset name is invalid.");
            }

            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<string, UIGroup> uiGroup in _uiGroups)
            {
                uiGroup.Value.InternalGetUIForms(uiFormAssetName, results);
            }
        }

        /// <summary>
        /// 获取所有已加载的界面。
        /// </summary>
        /// <returns>所有已加载的界面。</returns>
        public IUIForm[] GetAllLoadedUIForms()
        {
            List<IUIForm> results = new List<IUIForm>();
            foreach (KeyValuePair<string, UIGroup> uiGroup in _uiGroups)
            {
                results.AddRange(uiGroup.Value.GetAllUIForms());
            }

            return results.ToArray();
        }

        /// <summary>
        /// 获取所有已加载的界面。
        /// </summary>
        /// <param name="results">所有已加载的界面。</param>
        public void GetAllLoadedUIForms(List<IUIForm> results)
        {
            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<string, UIGroup> uiGroup in _uiGroups)
            {
                uiGroup.Value.InternalGetAllUIForms(results);
            }
        }

        /// <summary>
        /// 获取所有正在加载界面的序列编号。
        /// </summary>
        /// <returns>所有正在加载界面的序列编号。</returns>
        public int[] GetAllLoadingUIFormSerialIds()
        {
            int index = 0;
            int[] results = new int[_uiFormsBeingLoaded.Count];
            foreach (KeyValuePair<int, string> uiFormBeingLoaded in _uiFormsBeingLoaded)
            {
                results[index++] = uiFormBeingLoaded.Key;
            }

            return results;
        }

        /// <summary>
        /// 获取所有正在加载界面的序列编号。
        /// </summary>
        /// <param name="results">所有正在加载界面的序列编号。</param>
        public void GetAllLoadingUIFormSerialIds(List<int> results)
        {
            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<int, string> uiFormBeingLoaded in _uiFormsBeingLoaded)
            {
                results.Add(uiFormBeingLoaded.Key);
            }
        }

        /// <summary>
        /// 是否正在加载界面。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <returns>是否正在加载界面。</returns>
        public bool IsLoadingUIForm(int serialId)
        {
            return _uiFormsBeingLoaded.ContainsKey(serialId);
        }

        /// <summary>
        /// 是否正在加载界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>是否正在加载界面。</returns>
        public bool IsLoadingUIForm(string uiFormAssetName)
        {
            if (string.IsNullOrEmpty(uiFormAssetName))
            {
                throw new Exception("UI form asset name is invalid.");
            }

            return _uiFormsBeingLoaded.ContainsValue(uiFormAssetName);
        }

        /// <summary>
        /// 是否是合法的界面。
        /// </summary>
        /// <param name="uiForm">界面。</param>
        /// <returns>界面是否合法。</returns>
        public bool IsValidUIForm(IUIForm uiForm)
        {
            if (uiForm == null)
            {
                return false;
            }

            return HasUIForm(uiForm.SerialId);
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
        public int OpenUIForm(string uiFormAssetName, string uiGroupName, bool pauseCoveredUIForm, object userData)
        {
            if (_resourceLoader == null)
            {
                throw new Exception("You must set resource manager first.");
            }

            if (_uiFormHelper == null)
            {
                throw new Exception("You must set UI form helper first.");
            }

            if (string.IsNullOrEmpty(uiFormAssetName))
            {
                throw new Exception("UI form asset name is invalid.");
            }

            if (string.IsNullOrEmpty(uiGroupName))
            {
                throw new Exception("UI group name is invalid.");
            }

            UIGroup uiGroup = (UIGroup)GetUIGroup(uiGroupName);
            if (uiGroup == null)
            {
                throw new Exception(ZString.Format("UI group '{0}' is not exist.", uiGroupName));
            }

            int serialId = ++_serial;
            UIFormInstanceObject uiFormInstanceObject = Spawn(uiFormAssetName);
            if (uiFormInstanceObject == null)
            {
                _uiFormsBeingLoaded.Add(serialId, uiFormAssetName);
                _resourceLoader.LoadAsset(uiFormAssetName, LoadAssetSuccessCallback, LoadAssetFailureCallback, OpenUIFormInfo.Create(serialId, uiGroup, pauseCoveredUIForm, userData));
            }
            else
            {
                InternalOpenUIForm(serialId, uiFormAssetName, uiGroup, uiFormInstanceObject.UIFormInstance, pauseCoveredUIForm, false, 0f, userData);
            }

            return serialId;
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="serialId">要关闭界面的序列编号。</param>
        public void CloseUIForm(int serialId)
        {
            CloseUIForm(serialId, null);
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="serialId">要关闭界面的序列编号。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void CloseUIForm(int serialId, object userData)
        {
            if (IsLoadingUIForm(serialId))
            {
                _uiFormsToReleaseOnLoad.Add(serialId);
                _uiFormsBeingLoaded.Remove(serialId);
                return;
            }

            IUIForm uiForm = GetUIForm(serialId);
            if (uiForm == null)
            {
                throw new Exception(ZString.Format("Can not find UI form '{0}'.", serialId));
            }

            CloseUIForm(uiForm, userData);
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="uiForm">要关闭的界面。</param>
        public void CloseUIForm(IUIForm uiForm)
        {
            CloseUIForm(uiForm, null);
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="uiForm">要关闭的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void CloseUIForm(IUIForm uiForm, object userData)
        {
            if (uiForm == null)
            {
                throw new Exception("UI form is invalid.");
            }

            UIGroup uiGroup = (UIGroup)uiForm.UIGroup;
            if (uiGroup == null)
            {
                throw new Exception("UI group is invalid.");
            }

            uiGroup.RemoveUIForm(uiForm);
            uiForm.OnClose(_isShutdown, userData);
            uiGroup.Refresh();

            if (_closeUIFormCompleteEventHandler != null)
            {
                CloseUIFormCompleteEventArgs closeUIFormCompleteEventArgs = CloseUIFormCompleteEventArgs.Create(uiForm.SerialId, uiForm.UIFormAssetName, uiGroup, userData);
                _closeUIFormCompleteEventHandler(this, closeUIFormCompleteEventArgs);
                ReferencePool.Release(closeUIFormCompleteEventArgs);
            }

            _recycleQueue.Enqueue(uiForm);
        }

        /// <summary>
        /// 关闭所有已加载的界面。
        /// </summary>
        public void CloseAllLoadedUIForms()
        {
            CloseAllLoadedUIForms(null);
        }

        /// <summary>
        /// 关闭所有已加载的界面。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        public void CloseAllLoadedUIForms(object userData)
        {
            IUIForm[] uiForms = GetAllLoadedUIForms();
            foreach (IUIForm uiForm in uiForms)
            {
                if (!HasUIForm(uiForm.SerialId))
                {
                    continue;
                }

                CloseUIForm(uiForm, userData);
            }
        }

        /// <summary>
        /// 关闭所有正在加载的界面。
        /// </summary>
        public void CloseAllLoadingUIForms()
        {
            foreach (KeyValuePair<int, string> uiFormBeingLoaded in _uiFormsBeingLoaded)
            {
                _uiFormsToReleaseOnLoad.Add(uiFormBeingLoaded.Key);
            }

            _uiFormsBeingLoaded.Clear();
        }

        /// <summary>
        /// 激活界面。
        /// </summary>
        /// <param name="uiForm">要激活的界面。</param>
        public void RefocusUIForm(IUIForm uiForm)
        {
            RefocusUIForm(uiForm, null);
        }

        /// <summary>
        /// 激活界面。
        /// </summary>
        /// <param name="uiForm">要激活的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void RefocusUIForm(IUIForm uiForm, object userData)
        {
            if (uiForm == null)
            {
                throw new Exception("UI form is invalid.");
            }

            UIGroup uiGroup = (UIGroup)uiForm.UIGroup;
            if (uiGroup == null)
            {
                throw new Exception("UI group is invalid.");
            }

            uiGroup.RefocusUIForm(uiForm, userData);
            uiGroup.Refresh();
            uiForm.OnRefocus(userData);
        }

        private void InternalOpenUIForm(int serialId, string uiFormAssetName, UIGroup uiGroup, object uiFormInstance, bool pauseCoveredUIForm, bool isNewInstance, float duration, object userData)
        {
            try
            {
                IUIForm uiForm = _uiFormHelper.CreateUIForm(uiFormInstance, uiGroup, userData);
                if (uiForm == null)
                {
                    throw new Exception("Can not create UI form in UI form helper.");
                }

                uiForm.OnInit(serialId, uiFormAssetName, uiGroup, pauseCoveredUIForm, isNewInstance, userData);
                uiGroup.AddUIForm(uiForm);
                uiForm.OnOpen(userData);
                uiGroup.Refresh();

                if (_openUIFormSuccessEventHandler != null)
                {
                    OpenUIFormSuccessEventArgs openUIFormSuccessEventArgs = OpenUIFormSuccessEventArgs.Create(uiForm, duration, userData);
                    _openUIFormSuccessEventHandler(this, openUIFormSuccessEventArgs);
                    ReferencePool.Release(openUIFormSuccessEventArgs);
                }
            }
            catch (Exception exception)
            {
                if (_openUIFormFailureEventHandler != null)
                {
                    OpenUIFormFailureEventArgs openUIFormFailureEventArgs = OpenUIFormFailureEventArgs.Create(serialId, uiFormAssetName, uiGroup.Name, pauseCoveredUIForm, exception.ToString(), userData);
                    _openUIFormFailureEventHandler(this, openUIFormFailureEventArgs);
                    ReferencePool.Release(openUIFormFailureEventArgs);
                    return;
                }

                throw;
            }
        }

        private void LoadAssetSuccessCallback(string uiFormAssetName, object uiFormAsset, float duration, object userData)
        {
            OpenUIFormInfo openUIFormInfo = (OpenUIFormInfo)userData;
            if (openUIFormInfo == null)
            {
                throw new Exception("Open UI form info is invalid.");
            }

            if (_uiFormsToReleaseOnLoad.Contains(openUIFormInfo.SerialId))
            {
                _uiFormsToReleaseOnLoad.Remove(openUIFormInfo.SerialId);
                ReferencePool.Release(openUIFormInfo);
                _uiFormHelper.ReleaseUIForm(uiFormAsset, null);
                return;
            }

            _uiFormsBeingLoaded.Remove(openUIFormInfo.SerialId);
            UIFormInstanceObject uiFormInstanceObject = UIFormInstanceObject.Create(uiFormAssetName, uiFormAsset, _uiFormHelper.InstantiateUIForm(uiFormAsset), _uiFormHelper);
            Register(uiFormInstanceObject);

            InternalOpenUIForm(openUIFormInfo.SerialId, uiFormAssetName, openUIFormInfo.UIGroup, uiFormInstanceObject.UIFormInstance, openUIFormInfo.PauseCoveredUIForm, true, duration, openUIFormInfo.UserData);
            ReferencePool.Release(openUIFormInfo);
        }

        private void LoadAssetFailureCallback(string uiFormAssetName,  string errorMessage, object userData)
        {
            OpenUIFormInfo openUIFormInfo = (OpenUIFormInfo)userData;
            if (openUIFormInfo == null)
            {
                throw new Exception("Open UI form info is invalid.");
            }

            if (_uiFormsToReleaseOnLoad.Contains(openUIFormInfo.SerialId))
            {
                _uiFormsToReleaseOnLoad.Remove(openUIFormInfo.SerialId);
                return;
            }

            _uiFormsBeingLoaded.Remove(openUIFormInfo.SerialId);
            string appendErrorMessage = ZString.Format("Load UI form failure, asset name '{0}', error message '{1}'.", uiFormAssetName, errorMessage);
            if (_openUIFormFailureEventHandler != null)
            {
                OpenUIFormFailureEventArgs openUIFormFailureEventArgs = OpenUIFormFailureEventArgs.Create(openUIFormInfo.SerialId, uiFormAssetName, openUIFormInfo.UIGroup.Name, openUIFormInfo.PauseCoveredUIForm, appendErrorMessage, openUIFormInfo.UserData);
                _openUIFormFailureEventHandler(this, openUIFormFailureEventArgs);
                ReferencePool.Release(openUIFormFailureEventArgs);
                return;
            }

            throw new Exception(appendErrorMessage);
        }

        // private void LoadAssetUpdateCallback(string uiFormAssetName, float progress, object userData)
        // {
        //     OpenUIFormInfo openUIFormInfo = (OpenUIFormInfo)userData;
        //     if (openUIFormInfo == null)
        //     {
        //         throw new Exception("Open UI form info is invalid.");
        //     }
        //
        //     if (_openUIFormUpdateEventHandler != null)
        //     {
        //         OpenUIFormUpdateEventArgs openUIFormUpdateEventArgs = OpenUIFormUpdateEventArgs.Create(openUIFormInfo.SerialId, uiFormAssetName, openUIFormInfo.UIGroup.Name, openUIFormInfo.PauseCoveredUIForm, progress, openUIFormInfo.UserData);
        //         _openUIFormUpdateEventHandler(this, openUIFormUpdateEventArgs);
        //         ReferencePool.Release(openUIFormUpdateEventArgs);
        //     }
        // }
        //
        // private void LoadAssetDependencyAssetCallback(string uiFormAssetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        // {
        //     OpenUIFormInfo openUIFormInfo = (OpenUIFormInfo)userData;
        //     if (openUIFormInfo == null)
        //     {
        //         throw new Exception("Open UI form info is invalid.");
        //     }
        //
        //     if (_openUIFormDependencyAssetEventHandler != null)
        //     {
        //         OpenUIFormDependencyAssetEventArgs openUIFormDependencyAssetEventArgs = OpenUIFormDependencyAssetEventArgs.Create(openUIFormInfo.SerialId, uiFormAssetName, openUIFormInfo.UIGroup.Name, openUIFormInfo.PauseCoveredUIForm, dependencyAssetName, loadedCount, totalCount, openUIFormInfo.UserData);
        //         _openUIFormDependencyAssetEventHandler(this, openUIFormDependencyAssetEventArgs);
        //         ReferencePool.Release(openUIFormDependencyAssetEventArgs);
        //     }
        // }
    }
}