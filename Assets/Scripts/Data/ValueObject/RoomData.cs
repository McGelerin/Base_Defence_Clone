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
        public bool Isbase;
        
        public RoomData(PayTypeEnum payType, int cost) : base(payType, cost)
        {
        }
        
        public TurretData TurretData;
    }
}