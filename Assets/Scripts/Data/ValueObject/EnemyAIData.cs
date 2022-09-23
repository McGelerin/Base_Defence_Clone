using System;
using Enums;
using UnityEngine.Rendering;

namespace Data.ValueObject
{
    [Serializable]
    public class EnemyAIData
    {
        public SerializedDictionary<EnemyType, EnemyTypeData> EnemyTypeDatas;
    }
}