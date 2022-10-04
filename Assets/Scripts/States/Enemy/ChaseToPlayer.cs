using Abstract;
using AIBrain;
using Data.ValueObject;
using Enums;
using UnityEngine;
using UnityEngine.AI;

namespace States.Enemy
{
    public class ChaseToPlayer : EnemyBaseState
    {
        #region Self Variables

        #region Private Variables

        private EnemyAIBrain _manager;
        private NavMeshAgent _agent;
        private EnemyTypeData _data;

        #endregion
        
        #endregion

        public ChaseToPlayer(ref EnemyAIBrain manager, ref NavMeshAgent agent,ref EnemyTypeData data)
        {
            _manager = manager;
            _agent = agent;
            _data = data;
        }
        
        public override void EnterState()
        {
            _manager.AttackStatus(false);
            _agent.speed = _data.ChaseSpeed;
            _manager.AnimTriggerState(EnemyStates.Chase);
            _agent.SetDestination(_manager.PlayerTarget.transform.position);
        }

        public override void UpdateState()
        {
            //_agent.SetDestination(_manager.PlayerTarget.transform.position);
            _agent.destination = _manager.PlayerTarget.transform.position;
            if (_data.AttackRange > _agent.remainingDistance)
            {
                _manager.SwichState(EnemyStates.Attack);
            }
            if (_manager.HealthCheck())
            {
                _manager.SwichState(EnemyStates.Death);
            }
        }

        public override void OnTriggerEnterState(Collider other)
        {
        }

        public override void OnTriggerExitState(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _manager.SwichState(EnemyStates.Walk);
            }
        }
    }
}