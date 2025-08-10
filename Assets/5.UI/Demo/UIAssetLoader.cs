using System;
using GameFramework.UI;
using UnityEditor;
using UnityEngine;

public class UIAssetLoader : IUIResourceLoader
{
    public void LoadAsset(string uiFormAssetName, Action<string, object, float, object> loadAssetSuccessCallback, Action<string, string, object> loadAssetFailureCallback, object userData)
    {
        var asset = AssetDatabase.LoadAssetAtPath<GameObject>(uiFormAssetName);
        if (asset != null)
        {
            loadAssetSuccessCallback?.Invoke(uiFormAssetName, asset, 0.0f, userData);
        }
        else
        {
            loadAssetFailureCallback?.Invoke(uiFormAssetName, "资源不存在", userData);
        }
    }

    public void UnloadAsset(object uiFormAsset)
    {
    }
}