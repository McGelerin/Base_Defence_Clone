using System;
using Abstract;
using Enums;

namespace Data.ValueObject
{
    [Serializable]
    public class BuyableTurretData : Buyable
    {
        public BuyableTurretData(PayTypeEnum payType, int cost) : base(payType, cost)
        {
        }
    }
}