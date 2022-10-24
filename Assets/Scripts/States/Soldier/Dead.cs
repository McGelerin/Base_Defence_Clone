using Abstract;
using AIBrain;
using UnityEngine.AI;

namespace States.Soldier
{
    public class Dead : SoldierBaseStates
    {
        #region Self Variables
        #region Private Variables

        private SoldierAIWorker _manager;
        private NavMeshAgent _agent;

        #endregion
        
        #endregion

        public Dead(ref SoldierAIWorker manager, ref NavMeshAgent agent)
        {
            _manager = manager;
            _agent = agent;
        }

        public override void EnterState()
        {
            _agent.Stop();
            _manager.IsAttack(false);
            _manager.IsDeath();
        }

        public override void UpdateState() { }
    }
}