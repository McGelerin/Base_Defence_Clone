using System;

namespace Data.ValueObject
{
    [Serializable]
    public class SoldierAIData
    {
        public int Health;
        public int Damage;
        public float AttackDelay;
        public float AttackRange;
    }
}