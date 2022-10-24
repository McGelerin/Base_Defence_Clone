using Abstract;
using AIBrain;
using Enums;
using UnityEngine;
using UnityEngine.AI;

namespace States.AmmoWorker
{
    public class AnyState : AmmoWorkerBaseState
    {
        #region Self Variables

        #region Private Variables
        
        private AmmoWorkerAIBrain _manager;
        private NavMeshAgent _agent;

        #endregion

        #endregion

        public AnyState(ref AmmoWorkerAIBrain manager, ref NavMeshAgent agent)
        {
            _manager = manager;
            _agent = agent;
        }
        
        public override void EnterState()
        {
            _agent.SetDestination(Vector3.zero);
            _manager.AnimTriggerState(WorkerAnimState.Walk);
            _manager.Wait();
        }

        public override void OnTriggerEnterState(Collider other)
        {}
    }
}