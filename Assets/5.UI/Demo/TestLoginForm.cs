using System;
using CodeBind;
using UnityEngine;
using LTools.UI;

[MonoCodeBind]
public partial class TestLoginForm : AUGuiForm
{
    protected internal override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        
        ConfirmBtn.onClick.AddListener(OnConfirmBtnClick);
    }

    protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
    }

    protected internal override void OnClose(bool isShutdown, object userData)
    {
        base.OnClose(isShutdown, userData);
        ConfirmBtn.onClick.RemoveAllListeners();
    }

    private void OnConfirmBtnClick()
    {
        TipTMPText.text = DateTime.UtcNow.ToString("yyyy:MM:dd HH:mm:ss");
        var d = new TestTipForm.TestTipFormOpenData()
        {
            TipShow = DateTime.UtcNow.ToString("yyyy:MM:dd HH:mm:ss"),
            OnConfirm = (() =>
            {
                Debug.LogError(999);
            })
        };
        UIComponent.Instance.OpenUIForm("Assets/5.UI/Demo/TestTipForm.prefab", "Default", false, d);
    }
}