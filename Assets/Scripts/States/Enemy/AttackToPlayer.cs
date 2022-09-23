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
            _manager.AttackStatus(true);
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
        }

        public override void OnTriggerExitState(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _manager.SwichState(EnemyStates.Chase);
            }
        }
    }
}