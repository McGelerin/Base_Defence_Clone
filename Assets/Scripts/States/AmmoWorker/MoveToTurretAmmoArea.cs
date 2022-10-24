using System;
using Abstract;
using AIBrain;
using Enums;
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
            _manager.AnimTriggerState(WorkerAnimState.Walk);
        }

        public override void OnTriggerEnterState(Collider other)
        {
            if (other.CompareTag("TurretAmmoArea"))
            {
                if ((_agent.remainingDistance > 1.5f)) return;
                _manager.TurretAmmoArea(other.gameObject);
                _manager.SwitchState(AmmoWorkerStates.WaitToAmmoArea);
            }
        }
    }
}