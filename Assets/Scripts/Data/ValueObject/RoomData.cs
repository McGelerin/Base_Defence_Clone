using System;
using Abstract;
using Enums;
using Sirenix.OdinInspector;

namespace Data.ValueObject
{
    [Serializable]
    public class RoomData : Buyable
    {
        public RoomNameEnum RoomName;
        
        public RoomData(PayTypeEnum payType, int cost, int payedAmount) : base(payType, cost, payedAmount)
        {
        }

        public TurretData TurretData;
    }
}