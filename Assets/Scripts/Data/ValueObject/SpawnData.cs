using System;
using System.Security.Permissions;
using Enums;

namespace Data.ValueObject
{
    [Serializable]
    public class SpawnData
    {
        public EnemyType EnemyType;
        public int EnemyCount;
        public int CurrentCount = 0;
    }
}