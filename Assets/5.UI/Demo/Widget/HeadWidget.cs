using CodeBind;
using LTools.UI;
using UnityEngine;

public partial class HeadWidget : BaseWidget
{
    public void SetHead(Color color, string id)
    {
        IconImg.color = color;
        NameTMPText.text = id;
    }

    public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
    }
}