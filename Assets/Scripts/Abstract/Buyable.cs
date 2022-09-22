using Enums;

namespace Abstract
{
    public abstract class Buyable
    {
        public PayTypeEnum PayType;
        
        public int Cost;
        
        protected Buyable(PayTypeEnum payType, int cost)
        {
            PayType = payType;
            Cost = cost;
        }
    }
}