using System;
using Data.ValueObject;
using Enums;
using Extentions;
using UnityEngine.Events;

namespace Signals
{
    public class IdleSignals : MonoSingleton<IdleSignals>
    {
        public UnityAction<RoomNameEnum,int> onBaseAreaBuyedItem = delegate {  };
        
        public UnityAction onGettedBaseData = delegate {  };
        public Func<RoomNameEnum,RoomData> onRoomData = delegate{return  default;};
        public Func<RoomNameEnum,int> onPayedRoomData = delegate{return  default;};
        
    }
}