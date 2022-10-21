using System;
using Enums;
using Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class IdleSignals : MonoSingleton<IdleSignals>
    {
        #region Base Area
        
        public  Func<Transform> onGetWarHousePositon = delegate { return default;};

        #endregion

        #region MineArea

        public Func<GameObject> onGetMineGameObject = delegate { return default;};
        public Func<GameObject> onGemAreaHolder = delegate { return default;};
        public UnityAction<Transform> onGemHolderAddGem = delegate(Transform arg0) {  }; 

        #endregion
        
        #region OutSideSignals

        public Func<GameObject> onEnemyTarget = delegate { return default;};
        public UnityAction<GameObject,EnemyType> onEnemyDead = delegate(GameObject arg0, EnemyType type) {  };
        public UnityAction<GameObject> onHostageCollected = delegate {  };

        #endregion
        
        #region WeaponArea

        public Func<WeaponType> onSelectedWeapon = delegate { return WeaponType.M4;};
        public Func<PlayerAnimState> onSelectedWeaponAnimState = delegate { return 0;};
        public Func<PlayerAnimState> onSelectedWeaponAttackAnimState = delegate { return 0;};
        
        #endregion

        #region Turret

        public UnityAction onInteractPlayerWithTurret = delegate { };

        #endregion

        #region SoldierSignals

        public Func<GameObject> onSoldierInitPosition = delegate { return default;};
        public Func<GameObject> onSoldierSearchInitPosition = delegate { return default;};
        public UnityAction<GameObject> onPlayerEntrySoldierArea = delegate(GameObject arg0) {  };
        public Func<GameObject> onGetSoldierBarrack = delegate { return default;};
        public UnityAction onHostageEntryBarrack = delegate {  };
        
        #endregion
        
    }
}