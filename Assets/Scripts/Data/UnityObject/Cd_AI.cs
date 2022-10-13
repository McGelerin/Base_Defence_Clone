using Data.ValueObject;
using UnityEngine;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_AI", menuName = "Game/CD_AI", order = 0)]
    public class Cd_AI : ScriptableObject
    {
        public EnemyAIData EnemyAIData;
        public AmmoWorkerAIData AmmoWorkerAIData;
    }
}