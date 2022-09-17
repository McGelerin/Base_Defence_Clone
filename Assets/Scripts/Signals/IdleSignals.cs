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
        public UnityAction<RoomNameEnum,int> onBaseAreaBuyedItem = delegate {  };
        
        public UnityAction onGettedBaseData = delegate {  };
        public Func<RoomNameEnum,RoomData> onRoomData = delegate{return  default;};
        public Func<RoomNameEnum,int> onPayedRoomData = delegate{return  default;};


        #region Mine Area

        public Func<MineAreaData> onGetMineAreaData = delegate { return default;};
        public Func<GameObject> onGetMineGameObject = delegate { return default;};
        public Func<GameObject> onGemAreaHolder = delegate { return default;};
        public UnityAction<Transform> onGemHolderAddGem = delegate(Transform arg0) {  }; 

        #endregion

    }
}