using System;
using Data.ValueObject;
using Enums;
using Extentions;
using Keys;
using UnityEngine.Events;

namespace Signals
{
    public class DataTransferSignals : MonoSingleton<DataTransferSignals>
    {
        #region Base Area

        public UnityAction<RoomNameEnum,int> onBaseAreaBuyedItem = delegate {  };
        public UnityAction<TurretNameEnum,int> onTurretAreaBuyedItem = delegate {  };
        public UnityAction onGettedBaseData = delegate {  };
        public Func<RoomNameEnum,RoomData> onRoomData = delegate{return  default;};
        public Func<RoomNameEnum,int> onPayedRoomData = delegate{return  default;};
        public Func<TurretNameEnum,BuyableTurretData> onTurretData = delegate{return  default;};
        public Func<TurretNameEnum,int> onPayedTurretData = delegate{return  default;};

        #endregion
        
        #region OutsideArea
        
        public UnityAction onGettedOutSideData = delegate {  };
        public Func<OutSideStateLevels,OutsideData> onGetOutsideData = delegate { return default;};
        public Func<OutSideStateLevels,int> onGetPayedStageData = delegate { return default;};
        public UnityAction<OutSideStateLevels,int> onOutsideBuyedItems = delegate {  };
        
        #endregion
        
        #region Mine Area

        public Func<MineAreaData> onGetMineAreaData = delegate { return default;};
        
        #endregion

        #region Supporter Area
        
        public UnityAction onGettedSupporterData = delegate {  };
        public Func<AmmoWorkerBuyData> onGetAmmoWorkerData = delegate { return default;};
        public Func<int> onGetPayedAmmoWorkerData = delegate { return default;};
        public UnityAction<int> onAmmoWorkerAreaBuyedItems = delegate {  };
        
        public Func<MoneyWorkerBuyData> onGetMoneyWorkerData = delegate { return default;};
        public Func<int> onGetPayedMoneyWorkerData = delegate { return default;};
        public UnityAction<int> onMoneyWorkerAreaBuyedItems = delegate {  };
        #endregion
    }
}