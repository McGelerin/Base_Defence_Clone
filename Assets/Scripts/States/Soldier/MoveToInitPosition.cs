using Abstract;
using AIBrain;
using Signals;
using UnityEngine.AI;

namespace States.Soldier
{
    public class MoveToInitPosition : SoldierBaseStates
    {
        #region Self Variables
        #region Private Variables

        private SoldierAIWorker _manager;
        private NavMeshAgent _agent;

        #endregion
        
        #endregion

        public MoveToInitPosition(ref SoldierAIWorker manager, ref NavMeshAgent agent)
        {
            _manager = manager;
            _agent = agent;
        }
        
        public override void EnterState()
        {
            _agent.stoppingDistance = 0;
            _manager.Target = IdleSignals.Instance.onSoldierInitPosition();
            _agent.SetDestination(_manager.Target.transform.position);
        }

        public override void UpdateState()
        {
            _manager.AnimSetFloat(_agent.velocity.magnitude);
        }
    }
}