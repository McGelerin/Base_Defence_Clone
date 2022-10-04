using System;
using Extentions;
using UnityEngine;

namespace Signals
{
    public class AttackSignals : MonoSingleton<AttackSignals>
    {
        public Func<Transform> onPlayerIsTarget = delegate { return default;};
        public Func<int> onGetWeaponDamage = delegate { return 0;};
    }
}