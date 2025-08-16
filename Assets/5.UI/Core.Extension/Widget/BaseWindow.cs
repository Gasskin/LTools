using CodeBind;

namespace LTools.UI
{
    [MonoCodeBind]
    public class BaseWindow: AUGuiForm
    {
        protected WidgetContainer WidgetContainer { get; private set; }
        
        protected internal override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            WidgetContainer = WidgetContainer.Create(this);
        }

        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            WidgetContainer?.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected internal override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            ReferencePool.Release(WidgetContainer);
            WidgetContainer = null;
        }
    }
}
