using Abstract;
using AIBrain;
using Data.ValueObject;
using Enums;
using UnityEngine;
using UnityEngine.AI;

namespace States.Enemy
{
    public class MoveToTurret : EnemyBaseState
    {

        #region Self Variables

        #region Private Variables

        private EnemyAIBrain _manager;
        private NavMeshAgent _agent;
        private EnemyTypeData _data;

        #endregion
        
        #endregion

        public MoveToTurret(ref EnemyAIBrain manager, ref NavMeshAgent agent,ref EnemyTypeData data)
        {
            _manager = manager;
            _agent = agent;
            _data = data;
        }
        
        public override void EnterState()
        {
            Debug.Log("turret");
            _agent.speed = _data.MoveSpeed;
            _manager.AnimTriggerState(EnemyStates.Walk);
            _agent.SetDestination(_manager.TurretTarget.transform.position);
        }

        public override void UpdateState()
        {
            if (_manager.HealthCheck())
            {
                _manager.SwichState(EnemyStates.Death);
            }

            _manager.AnimBoolState(EnemyStates.Idle, _data.AttackRange > _agent.remainingDistance);
        }

        public override void OnTriggerEnterState(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _manager.PlayerTarget = other.transform.parent.gameObject;
                _manager.SwichState(EnemyStates.Chase);
            }
        }

        public override void OnTriggerExitState(Collider other)
        {
        }
    }
}