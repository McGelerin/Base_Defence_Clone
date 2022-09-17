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
        public int GemCoundX;
        [Range(1,20)]
        public int GemCoundZ;
        [Range(1,5)]
        public float OffsetFactor;
    }
}