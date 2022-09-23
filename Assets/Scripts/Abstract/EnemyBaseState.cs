using AIBrain;
using UnityEngine;

namespace Abstract
{
    public abstract class EnemyBaseState
    {
        public abstract void EnterState();
        
        public abstract void UpdateState();
        
        public abstract void OnTriggerEnterState(Collider other);
        
        public abstract void OnTriggerExitState(Collider other);
    }
}