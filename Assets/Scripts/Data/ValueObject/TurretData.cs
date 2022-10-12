using System;

namespace Data.ValueObject
{
    [Serializable]
    public class TurretData
    {
        public float TurretRange;
        public float AttackDelay;
        public float RotateDelay;
        public int AmmoCapacity;
        public int AmmoDamage;
        public StaticStackData TurretStackData;
    }
}