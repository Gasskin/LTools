using System;
using System.Collections.Generic;
using CodeBind;
using LTools.UI;
using UnityEngine;
using UnityEngine.UI;

public class CodeBindName2TypeConfig
{
    [CodeBindNameType]
    public static Dictionary<string, Type> BindNameTypeDict = new Dictionary<string, Type>()
    {
        // Unity
        { "Rect", typeof(RectTransform) },
        { "Btn", typeof(Button) },
        { "Img", typeof(Image) },
        // UI
        { "Widget", typeof(BaseWidget) },
        // TMP
        { "TMPText", typeof(TMPro.TMP_Text) },
        { "TMPInputField", typeof(TMPro.TMP_InputField) },
        { "TextMeshProUGUI", typeof(TMPro.TextMeshProUGUI) },
        { "TextMeshPro", typeof(TMPro.TextMeshPro) },
    };
}