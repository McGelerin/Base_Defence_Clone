using System;
using Data.ValueObject;
using Enums;
using Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class IdleSignals : MonoSingleton<IdleSignals>
    {
        #region Base Area

        public UnityAction<RoomNameEnum,int> onBaseAreaBuyedItem = delegate {  };
        public UnityAction<TurretNameEnum,int> onTurretAreaBuyedItem = delegate {  };
        
        public UnityAction onGettedBaseData = delegate {  };
        public Func<RoomNameEnum,RoomData> onRoomData = delegate{return  default;};
        public Func<RoomNameEnum,int> onPayedRoomData = delegate{return  default;};
        public  Func<Transform> onGetWarHousePositon = delegate { return default;};

        public Func<TurretNameEnum,BuyableTurretData> onTurretData = delegate{return  default;};
        public Func<TurretNameEnum,int> onPayedTurretData = delegate{return  default;};

        #endregion
        
        #region Mine Area

        public Func<MineAreaData> onGetMineAreaData = delegate { return default;};
        public Func<GameObject> onGetMineGameObject = delegate { return default;};
        public Func<GameObject> onGemAreaHolder = delegate { return default;};
        public UnityAction<Transform> onGemHolderAddGem = delegate(Transform arg0) {  }; 

        #endregion

        #region OutSideSignals

        public Func<GameObject> onEnemyTarget = delegate { return default;};
        public UnityAction<GameObject,EnemyType> onEnemyDead = delegate(GameObject arg0, EnemyType type) {  };
        public UnityAction<GameObject> onHostageCollected = delegate {  };
        //public Func<int> onMineAreaCurrentScore = delegate { return 0;};

        #endregion
        
        #region WeaponArea

        public Func<WeaponType> onSelectedWeapon = delegate { return WeaponType.Pistol;};
        public Func<PlayerAnimState> onSelectedWeaponAnimState = delegate { return 0;};
        public Func<PlayerAnimState> onSelectedWeaponAttackAnimState = delegate { return 0;};
        
        #endregion

        #region Turret

        public UnityAction onInteractPlayerWithTurret = delegate { };

        #endregion

    }
}