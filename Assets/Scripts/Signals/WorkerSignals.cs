using System;
using System.Collections.Generic;
using Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class WorkerSignals : MonoSingleton<WorkerSignals>
    {
        //public UnityAction<GameObject> onWareHouse = delegate(GameObject arg0) {  }; zaten varmış
        public UnityAction<GameObject, List<GameObject>> onTurretAmmoAreas = delegate(GameObject arg0, List<GameObject> list) {  };
        //public Func<GameObject> onGetWarHouseArea = delegate { return default;};
        public Func<GameObject> onGetTurretArea = delegate { return default;};
        //public Func<GameObject,int> onRemainingCapacity = delegate { return default;};
    }
}