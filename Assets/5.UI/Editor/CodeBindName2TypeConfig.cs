using System;
using System.Collections.Generic;
using CodeBind;
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
        // UI
        // { "UIElement", typeof(AUIElement) },
        // TMP
        { "TMPText", typeof(TMPro.TMP_Text) },
        { "TMPInputField", typeof(TMPro.TMP_InputField) },
        { "TextMeshProUGUI", typeof(TMPro.TextMeshProUGUI) },
        { "TextMeshPro", typeof(TMPro.TextMeshPro) },
    };
}