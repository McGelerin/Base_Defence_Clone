using System;
using Abstract;
using Enums;

namespace Data.ValueObject
{
    [Serializable]
    public class TurretData : Buyable
    {
        public int AmmoCapacity;
        public int AmmoDamage;

        public TurretData(PayTypeEnum payType, int cost) : base(payType, cost)
        {
        }
    }
}