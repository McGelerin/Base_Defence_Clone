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
            _manager.AttackToPlayerStatus(false);
            _agent.speed = _data.ChaseSpeed;
            _manager.AnimTriggerState(EnemyAnimState.Chase);
            _agent.SetDestination(_manager.Target.transform.position);
        }

        public override void UpdateState()
        {
            //_agent.SetDestination(_manager.PlayerTarget.transform.position);
            _agent.destination = _manager.Target.transform.position;
            if (_data.AttackRange > _agent.remainingDistance)
            {
                _manager.SwitchState(EnemyStates.AttackToPlayer);
            }
            if (_manager.HealthCheck())
            {
                _manager.SwitchState(EnemyStates.EnemyDeath);
            }
        }

        public override void OnTriggerEnterState(Collider other)
        {
        }

        public override void OnTriggerExitState(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _manager.SwitchState(EnemyStates.MoveToTurret);
            }
        }
    }
}