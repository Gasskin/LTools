using System;

namespace LTools.UI
{
    public interface IUIResourceLoader
    {
        void LoadAsset(string uiFormAssetName,
            //string uiFormAssetName, object uiFormAsset, float duration, object userData
            Action<string, object, float, object> loadAssetSuccessCallback,
            //string uiFormAssetName,  string errorMessage, object userData
            Action<string, string, object> loadAssetFailureCallback,
            object userData);
        
        void UnloadAsset(object uiFormAsset);
    }
}