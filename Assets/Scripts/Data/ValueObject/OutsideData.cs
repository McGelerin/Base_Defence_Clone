using System;
using Abstract;
using Enums;

namespace Data.ValueObject
{
    [Serializable]
    public class OutsideData : Buyable
    {
        public OutSideStateLevels levels;
        
        public OutsideData(PayTypeEnum payType, int cost) : base(payType, cost)
        {
        }
        
    }
}