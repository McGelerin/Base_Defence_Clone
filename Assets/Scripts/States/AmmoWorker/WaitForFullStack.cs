using Abstract;
using AIBrain;
using Enums;
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
            _manager.AnimTriggerState(WorkerAnimState.Idle);
            _manager.InteractWareHouseArea();
        }
        public override void OnTriggerEnterState(Collider other) { }
    }
}