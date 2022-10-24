using Abstract;
using AIBrain;
using Data.ValueObject;
using Enums;
using UnityEngine.AI;

namespace States.Soldier
{
    public class MoveToEnemy : SoldierBaseStates
    {
        #region Self Variables
        #region Private Variables

        private SoldierAIWorker _manager;
        private NavMeshAgent _agent;
        private SoldierAIData _data;

        #endregion
        
        #endregion

        public MoveToEnemy(ref SoldierAIWorker manager, ref NavMeshAgent agent, ref SoldierAIData data)
        {
            _manager = manager;
            _agent = agent;
            _data = data;
        }

        public override void EnterState()
        {
            _manager.IsAttack(false);
            _agent.SetDestination(_manager.Target.transform.position);
        }

        public override void UpdateState()
        {
            if (_agent.remainingDistance <= _data.AttackRange)
            {
                _manager.SwitchState(SoldierStates.RangedAttack);
            }
            else
            {
                _agent.destination = _manager.Target.transform.position;
            }
            
            _manager.AnimSetFloat(_agent.velocity.magnitude);
            
            if (_manager.HealthCheck())
            {
                _manager.SwitchState(SoldierStates.Dead);
            }
        }
    }
}