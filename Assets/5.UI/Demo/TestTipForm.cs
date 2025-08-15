using System;
using CodeBind;
using LTools;
using LTools.UI;

[MonoCodeBind]
public partial class TestTipForm : AUGuiForm
{
    public class TestTipFormOpenData
    {
        public string TipShow;
        public Action OnConfirm;
    }

    // private TestTipFormOpenData _openData;
    //
    // protected internal override void OnOpen(object userData)
    // {
    //     base.OnOpen(userData);
    //     InitBind(GetComponent<CSCodeBindMono>());
    //
    //     _openData = (TestTipFormOpenData)userData;
    //     if (_openData != null) 
    //         TipTMPText.text = _openData.TipShow;
    //     CloseBtn.onClick.AddListener(OnCloseBtnClick);
    //     ConfirmBtn.onClick.AddListener(OnConfirmBtnClick);
    // }
    //
    // protected internal override void OnClose(bool isShutdown, object userData)
    // {
    //     base.OnClose(isShutdown, userData);
    //     CloseBtn.onClick.RemoveAllListeners();
    //     ConfirmBtn.onClick.RemoveAllListeners();
    // }
    //
    // private void OnConfirmBtnClick()
    // {
    //     _openData?.OnConfirm?.Invoke();
    // }
    //
    // private void OnCloseBtnClick()
    // {
    //     UIComponent.Instance.CloseUIForm(UIForm);
    // }
}