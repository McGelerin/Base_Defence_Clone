using Abstract;
using AIBrain;
using Enums;
using Signals;
using UnityEngine;
using UnityEngine.AI;

namespace States.AmmoWorker
{
    public class MoveToWareHouseArea : AmmoWorkerBaseState
    {
        #region Self Variables

        #region Private Variables
        
        private AmmoWorkerAIBrain _manager;
        private NavMeshAgent _agent;

        #endregion

        #endregion

        public MoveToWareHouseArea(ref AmmoWorkerAIBrain manager, ref NavMeshAgent agent)
        {
            _manager = manager;
            _agent = agent;
        }
        
        public override void EnterState()
        {
            _manager.Target = IdleSignals.Instance.onGetWarHousePositon().gameObject;
            _agent.SetDestination(_manager.Target.transform.position);
            _manager.AnimTriggerState(WorkerAnimState.Walk);
        }

        public override void OnTriggerEnterState(Collider other)
        {
            if (other.CompareTag("AmmoReloadArea"))
            {
                _manager.SwitchState(AmmoWorkerStates.WaitForFullStack);
            }
        }
    }
}