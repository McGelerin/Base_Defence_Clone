using System;

namespace Data.ValueObject
{
    [Serializable]
    public class MineAreaData
    {
        public int MaxWorkerAmound;
        public int CurrentWorkerAmound;
        public int MaxWorkerFromMine;
        public GemHolderData GemHolderData;
    }
}