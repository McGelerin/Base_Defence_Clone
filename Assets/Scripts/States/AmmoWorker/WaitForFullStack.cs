using Abstract;
using AIBrain;
using UnityEngine;
using UnityEngine.AI;

namespace States.AmmoWorker
{
    public class WaitForFullStack : AmmoWorkerBaseState
    {
        #region Self Variables

        #region Private Variables
        
        private AmmoWorkerAIBrain _manager;

        #endregion
        #endregion

        public WaitForFullStack(ref AmmoWorkerAIBrain manager, ref NavMeshAgent agent)
        {
            _manager = manager;
        }
        
        public override void EnterState()
        {
            _manager.InteractWareHouseArea(_manager.Target.transform);
        }
        public override void UpdateState() { }
        public override void OnTriggerEnterState(Collider other) { }
        public override void OnTriggerExitState(Collider other) { }
    }
}