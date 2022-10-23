using Abstract;
using AIBrain;
using Data.ValueObject;
using UnityEngine;
using UnityEngine.AI;

namespace States.Enemy
{
    public class EnemyDeath : EnemyBaseState
    {
        #region Self Variables

        #region Private Variables

        private EnemyAIBrain _manager;
        private NavMeshAgent _agent;
        private EnemyTypeData _data;

        #endregion
        
        #endregion

        public EnemyDeath(ref EnemyAIBrain manager, ref NavMeshAgent agent,ref EnemyTypeData data)
        {
            _manager = manager;
            _agent = agent;
            _data = data;
        }
        
        public override void EnterState()
        {
            _agent.Stop();
            _manager.AttackToPlayerStatus(false);
            _manager.IsDeath();
        }

        public override void UpdateState()
        {
        }

        public override void OnTriggerEnterState(Collider other)
        {
        }

        public override void OnTriggerExitState(Collider other)
        {
        }
    }
}