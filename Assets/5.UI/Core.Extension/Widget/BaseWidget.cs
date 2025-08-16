using CodeBind;
using UnityEngine;

namespace LTools.UI
{
    [MonoCodeBind]
    public abstract class BaseWidget : MonoBehaviour
    {
        private BaseWindow _parentWindow;

        private Transform _cachedTransform;

        private GameObject _cachedGameObject;

        public void Create(BaseWindow parentForm)
        {
            _parentWindow = parentForm;
            _cachedTransform = transform;
            _cachedGameObject = gameObject;
        }

        /// <summary>
        /// 由父界面驱动
        /// </summary>
        /// <param name="elapseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
        public virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 仅手动调用Show时触发
        /// </summary>
        protected virtual void OnShow()
        {
        }

        /// <summary>
        /// 仅手动调用Hide时触发
        /// </summary>
        protected virtual void OnHide()
        {
        }

        protected void Show()
        {
            if (!_parentWindow.Visible)
                return;
            _cachedGameObject.SetActive(true);
            OnShow();
        }

        protected void Hide()
        {
            if (!_parentWindow.Visible)
                return;
            _cachedGameObject.SetActive(false);
            OnHide();
        }
    }
}