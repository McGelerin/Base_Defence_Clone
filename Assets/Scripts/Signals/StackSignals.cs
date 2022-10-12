using System;
using System.Collections.Generic;
using Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class StackSignals : MonoSingleton<StackSignals>
    {
        #region Dinamic Stack

        public Func<GameObject,GameObject> onGetHostageTarget = delegate(GameObject o) { return default;  };
        public UnityAction onLastGameObjectRemone = delegate {  };
        public Func<List<GameObject>> onGetHostageList = delegate { return default;};

        #endregion

        #region Static Stack
        
        public UnityAction<GameObject, List<GameObject>> onInteractStackHolder = delegate{  };
        public UnityAction<GameObject> onDecreseStackHolder = delegate {  };
        

        #endregion


    }
}