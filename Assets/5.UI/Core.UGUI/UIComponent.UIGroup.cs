//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public sealed partial class UIComponent 
    {
        [Serializable]
        private sealed class UIGroup
        {
            [SerializeField]
            private string _name = null;

            [SerializeField]
            private int _depth = 0;

            public string Name => _name;

            public int Depth => _depth;
        }

        private void SetGroup()
        {
            for (int i = 0; i < _uiGroups.Length; i++)
            {
                if (!AddUIGroup(_uiGroups[i].Name, _uiGroups[i].Depth))
                    continue;
            }
        }
    }
}
