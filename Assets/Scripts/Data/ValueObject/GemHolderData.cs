using System;
using UnityEngine;

namespace Data.ValueObject
{
    [Serializable]
    public class GemHolderData
    {
        public int MaxGemFromHolder;
        public Vector3 GemInitPoint;
        [Range(1,20)]
        public int GemCountX;
        [Range(1,20)]
        public int GemCountZ;
        [Range(1,5)]
        public float OffsetFactor;
    }
}