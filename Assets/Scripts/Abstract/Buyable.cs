using Enums;

namespace Abstract
{
    public abstract class Buyable
    {
        public PayTypeEnum PayType;
        
        public int Cost;

        public int PayedAmount;

        protected Buyable(PayTypeEnum payType, int cost, int payedAmount)
        {
            PayType = payType;
            Cost = cost;
            PayedAmount = payedAmount;
        }
    }
}