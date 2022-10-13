using System;
using Abstract;
using AIBrain;
using Signals;
using UnityEngine;
using UnityEngine.AI;

namespace States.AmmoWorker
{
    public class MoveToTurretAmmoArea : AmmoWorkerBaseState
    {
        #region Self Variables

        #region Private Variables
        
        private AmmoWorkerAIBrain _manager;
        private NavMeshAgent _agent;

        #endregion

        #endregion

        public MoveToTurretAmmoArea(ref AmmoWorkerAIBrain manager, ref NavMeshAgent agent)
        {
            _manager = manager;
            _agent = agent;
        }

        public override void EnterState()
        {
            _manager.Target = WorkerSignals.Instance.onGetTurretArea();
            _agent.SetDestination(_manager.Target.transform.position);
        }

        public override void UpdateState()
        {
            throw new System.NotImplementedException();
        }

        public override void OnTriggerEnterState(Collider other)
        {
            if (other.CompareTag(""))
            {
                throw new NotImplementedException();
            }
        }

        public override void OnTriggerExitState(Collider other)
        {
            throw new System.NotImplementedException();
        }
    }
}