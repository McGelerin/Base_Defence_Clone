using Abstract;
using AIBrain;
using Data.ValueObject;
using Enums;
using UnityEngine;
using UnityEngine.AI;

namespace States.Enemy
{
    public class AttackToSoldier : EnemyBaseState
    {
        #region Self Variables

        #region Private Variables

        private EnemyAIBrain _manager;
        private NavMeshAgent _agent;
        private EnemyTypeData _data;

        #endregion
        
        #endregion

        public AttackToSoldier(ref EnemyAIBrain manager, ref NavMeshAgent agent,ref EnemyTypeData data)
        {
            _manager = manager;
            _agent = agent;
            _data = data;
        }

        public override void EnterState()
        {
            _manager.AttackToSoldierStatus(true);
        }

        public override void UpdateState()
        {
            LookTarget();
            _agent.destination = _manager.Target.transform.position;
            if (_data.AttackRange < _agent.remainingDistance)
            {
                _manager.SwitchState(EnemyStates.ChaseToSoldier);
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
            if (other.CompareTag("Soldier"))
            {
                _manager.AttackToPlayerStatus(false);
                _manager.SwitchState(EnemyStates.MoveToTurret);
            }
        }
        
        private void LookTarget()
        {
            var direct = _manager.Target.transform.position - _manager.transform.position;
            var lookRotation = Quaternion.LookRotation(direct,Vector3.up);
            _manager.transform.rotation = Quaternion.Slerp(_manager.transform.rotation,
                lookRotation, 0.1f);
        }
    }
}