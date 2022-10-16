using System;
using System.Collections.Generic;
using Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class WorkerSignals : MonoSingleton<WorkerSignals>
    {
        public Func<GameObject> onGetBaseCenter = delegate { return null;};
        public UnityAction<GameObject, List<GameObject>> onTurretAmmoAreas = delegate(GameObject arg0, List<GameObject> list) {  };
        public Func<GameObject> onGetTurretArea = delegate { return default;};
        public UnityAction<GameObject> onAmmoAreaFull = delegate {  };
        
        public UnityAction<GameObject> onAddListToMoney = delegate(GameObject arg0) {  };
        public UnityAction<GameObject> onRemoveMoneyFromList = delegate(GameObject arg0) {  };
        public UnityAction onChangeDestination = delegate {  };
        public Func<GameObject> onGetMoneyGameObject = delegate { return default;};
    }
}