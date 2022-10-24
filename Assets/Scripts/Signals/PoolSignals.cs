using System;
using Enums;
using Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class PoolSignals : MonoSingleton<PoolSignals>
    {
        public Func<string,Transform,GameObject> onGetPoolObject = delegate(string s, Transform transform1)
        {
            return default;};
        public UnityAction<string,GameObject> onReleasePoolObject = delegate {  };
    }
}