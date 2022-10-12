using System;
using Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class AttackSignals : MonoSingleton<AttackSignals>
    {
        public Func<int> onGetWeaponDamage = delegate { return 0;};
        public Func<int> onGetAmmoDamage = delegate { return 0;};
        public Func<Vector3> onGetBulletDirect = delegate { return default;};
        public UnityAction<GameObject> onEnemyDead = delegate(GameObject arg0) {  };
        
        public UnityAction<bool> onPlayerHasTarget = delegate {  };
        public Func<GameObject> onPlayerIsTarget = delegate { return default;};
    }
}