using System;
using UnityEngine;


namespace Data.ValueObject
{
    [Serializable]
    public class StaticStackData
    {
        public int Capacity;
        public Vector3 InitPosition;
        public int StackCountX;
        public int StackCountZ;
        public Vector3 StackOffset;
        public float Delay;
    }
}