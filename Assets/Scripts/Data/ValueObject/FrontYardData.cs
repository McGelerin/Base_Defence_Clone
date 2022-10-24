using System;
using System.Collections.Generic;

namespace Data.ValueObject
{
    [Serializable]
    public class FrontYardData
    {
        public List<OutsideData> OutsideLevelData;
        public List<SpawnData> SpawnDatas;
    }
}