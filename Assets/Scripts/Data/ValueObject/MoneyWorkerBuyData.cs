using System;
using Abstract;
using Enums;

namespace Data.ValueObject
{
    [Serializable]
    public class MoneyWorkerBuyData : Buyable
    {
        public MoneyWorkerBuyData(PayTypeEnum payType, int cost) : base(payType, cost)
        {
        }
    }
}