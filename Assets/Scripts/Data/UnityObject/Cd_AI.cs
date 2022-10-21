using Data.ValueObject;
using UnityEngine;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_AI", menuName = "Game/CD_AI", order = 0)]
    public class CD_AI : ScriptableObject
    {
        public EnemyAIData EnemyAIData;
        public AmmoWorkerAIData AmmoWorkerAIData;
        public MoneyWorkerData MoneyWorkerData;
        public SoldierAIData SoldierAIData;
    }
}