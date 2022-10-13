using Abstract;
using AIBrain;
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
            throw new System.NotImplementedException();
        }

        public override void UpdateState()
        {
            throw new System.NotImplementedException();
        }

        public override void OnTriggerEnterState(Collider other)
        {
            throw new System.NotImplementedException();
        }

        public override void OnTriggerExitState(Collider other)
        {
            throw new System.NotImplementedException();
        }
    }
}