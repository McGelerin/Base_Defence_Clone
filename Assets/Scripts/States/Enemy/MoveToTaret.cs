using Abstract;
using AIBrain;
using Data.ValueObject;
using Enums;
using Managers;
using UnityEngine;
using UnityEngine.AI;

namespace States.Enemy
{
    public class MoveToTaret : EnemyBaseState
    {

        #region Self Variables

        #region Private Variables

        private EnemyAIBrain _manager;
        private NavMeshAgent _agent;
        private EnemyTypeData _data;

        #endregion
        
        #endregion

        public MoveToTaret(ref EnemyAIBrain manager, ref NavMeshAgent agent,ref EnemyTypeData data)
        {
            _manager = manager;
            _agent = agent;
            _data = data;
        }
        
        public override void EnterState()
        {
            _agent.speed = _data.MoveSpeed;
            _manager.AnimTriggerState(EnemyStates.Walk);
            _agent.SetDestination(_manager.Target.transform.position);
        }

        public override void UpdateState()
        {
            if (_manager.HealthChack())
            {
                _manager.SwichState(EnemyStates.Death);
            }
        }

        public override void OnTriggerEnterState(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _manager.SwichState(EnemyStates.Chase);
            }
        }

        public override void OnTriggerExitState(Collider other)
        {
        }
    }
}