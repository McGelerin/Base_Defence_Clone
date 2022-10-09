using System;

namespace Data.ValueObject
{
    [Serializable]
    public class EnemyTypeData
    {
        public int Health;
        public int Damage;
        public float AttackRange;
        public float MoveSpeed;
        public float ChaseSpeed;
        public int PrizeMoney;
    }
}