using System;
using Abstract;
using Enums;
using UnityEngine;

namespace Data.ValueObject
{
    [Serializable]
    public class PoolValueData
    {
        [Range(0,200)]
        public int ObjectLimit;
        public GameObject PooledObject;
    }
}