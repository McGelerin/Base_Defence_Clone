using System;
using UnityEngine;

namespace Data.ValueObject
{
    [Serializable]
    public class StackData
    {
        public int Capacity;
        public Vector3 InitPosition;
        public int StackCountX;
        public int StackCountY;
        public Vector3 StackOffset;
    }
}