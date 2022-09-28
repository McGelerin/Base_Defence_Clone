using System;
using Abstract;
using AIBrain;
using Data.ValueObject;
using Enums;
using UnityEngine;
using UnityEngine.AI;

namespace States.Enemy
{
    public class AttackToPlayer : EnemyBaseState
    {
        #region Self Variables

        #region Private Variables

        private EnemyAIBrain _manager;
        private NavMeshAgent _agent;
        private EnemyTypeData _data;

        #endregion
        
        #endregion

        public AttackToPlayer(ref EnemyAIBrain manager, ref NavMeshAgent agent,ref EnemyTypeData data)
        {
            _manager = manager;
            _agent = agent;
            _data = data;
        }
        
        public override void EnterState()
        {
            Debug.Log("Attack");
            //_agent.ResetPath();
            _manager.AttackStatus(true);
            //_agent.SetDestination(_manager.PlayerTarget.transform.position);
        }

        public override void UpdateState()
        {
            //_agent.SetDestination(_manager.PlayerTarget.transform.position);
            _agent.destination = _manager.PlayerTarget.transform.position;
            if (_data.AttackRange < _agent.remainingDistance)
            {
                _manager.SwichState(EnemyStates.Chase);
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
                Debug.Log("Trigger cıktı");
                _manager.AttackStatus(false);
                _manager.SwichState(EnemyStates.Walk);
            }
        }
    }
}