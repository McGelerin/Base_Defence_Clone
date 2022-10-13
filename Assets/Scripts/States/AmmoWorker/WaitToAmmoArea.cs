using Abstract;
using AIBrain;
using Enums;
using UnityEngine;

namespace States.AmmoWorker
{
    public class WaitToAmmoArea : AmmoWorkerBaseState
    {
        #region Self Variables

        #region Private Variables
        
        private AmmoWorkerAIBrain _manager;

        #endregion

        #endregion

        public WaitToAmmoArea(ref AmmoWorkerAIBrain manager)
        {
            _manager = manager;
        }
        
        public override void EnterState()
        {
            _manager.InteractTurretAmmoArea();
            _manager.AnimTriggerState(WorkerAnimState.Idle);
        }

        public override void OnTriggerEnterState(Collider other)
        { }
    }
}