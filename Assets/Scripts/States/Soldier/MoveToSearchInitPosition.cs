using Abstract;
using AIBrain;
using Enums;
using Signals;
using UnityEngine;
using UnityEngine.AI;

namespace States.Soldier
{
    public class MoveToSearchInitPosition : SoldierBaseStates
    {
        #region Self Variables
        #region Private Variables

        private SoldierAIWorker _manager;
        private NavMeshAgent _agent;
        private Vector3 _randomPosition;

        #endregion
        
        #endregion

        public MoveToSearchInitPosition(ref SoldierAIWorker manager, ref NavMeshAgent agent)
        {
            _manager = manager;
            _agent = agent;
        }
        
        public override void EnterState()
        {
            _agent.stoppingDistance = 2f;
            _randomPosition = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
            _manager.SearchInitPosition = IdleSignals.Instance.onSoldierSearchInitPosition();
            _agent.SetDestination(_manager.SearchInitPosition.transform.position +_randomPosition);
        }

        public override void UpdateState()
        {
            if (_agent.stoppingDistance >= _agent.remainingDistance)
            {
                _manager.SwitchState(SoldierStates.SearchEnemy);
            }
            _manager.AnimSetFloat(_agent.velocity.magnitude);
        }
    }
}