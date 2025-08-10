//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.UI
{
    internal sealed partial class UIManager
    {
        private sealed class OpenUIFormInfo : IReference
        {
            private int _serialId = 0;
            private UIGroup _uiGroup = null;
            private bool _pauseCoveredUIForm = false;
            private object _userData = null;

            public int SerialId => _serialId;

            public UIGroup UIGroup => _uiGroup;

            public bool PauseCoveredUIForm => _pauseCoveredUIForm;

            public object UserData => _userData;

            public static OpenUIFormInfo Create(int serialId, UIGroup uiGroup, bool pauseCoveredUIForm, object userData)
            {
                OpenUIFormInfo openUIFormInfo = ReferencePool.Acquire<OpenUIFormInfo>();
                openUIFormInfo._serialId = serialId;
                openUIFormInfo._uiGroup = uiGroup;
                openUIFormInfo._pauseCoveredUIForm = pauseCoveredUIForm;
                openUIFormInfo._userData = userData;
                return openUIFormInfo;
            }

            public void Clear()
            {
                _serialId = 0;
                _uiGroup = null;
                _pauseCoveredUIForm = false;
                _userData = null;
            }
        }
    }
}
