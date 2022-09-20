using System;
using Extentions;
using UnityEngine;

namespace Signals
{
    public class StackSignals : MonoSingleton<StackSignals>
    {
        public Func<GameObject,GameObject> onGetHostageTarget = delegate(GameObject o) { return default;  };
    }
}