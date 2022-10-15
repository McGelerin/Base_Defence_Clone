using System;
using Abstract;
using Enums;

namespace Data.ValueObject
{
    [Serializable]
    public class AmmoWorkerBuyData : Buyable
    {
        public AmmoWorkerBuyData(PayTypeEnum payType, int cost) : base(payType, cost)
        {
        }
    }
}