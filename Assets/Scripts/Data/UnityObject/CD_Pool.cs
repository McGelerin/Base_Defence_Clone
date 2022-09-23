using System.Collections.Generic;
using Data.ValueObject;
using Enums;
using UnityEngine;
using UnityEngine.Rendering;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_Pool", menuName = "Game/CD_Pool", order = 0)]
    public class CD_Pool : ScriptableObject
    {
        public SerializedDictionary<PoolType, PoolValueData> PoolValueDatas;
    }
}