using System;
using Abstract;
using AIBrain;
using Enums;
using UnityEngine;
using UnityEngine.AI;

namespace States.MoneyWorker
{
    public class MoveToRemoveStack : MoneyWorkerBaseState
    {
        #region Self Variables

        #region Private Variables
        
        private MoneyWorkerAIBrain _manager;
        private NavMeshAgent _agent;

        #endregion

        #endregion

        public MoveToRemoveStack(ref MoneyWorkerAIBrain manager, ref NavMeshAgent agent)
        {
            _manager = manager;
            _agent = agent;
        }

        public override void EnterState()
        {
            _agent.SetDestination(_manager.Base.transform.position);
        }

        public override void UpdateState()
        {
            _manager.AnimFloatState(_agent.velocity.magnitude);
        }

        public override void OnTriggerEnterState(Collider other)
        {
            if (other.CompareTag("BarrierInSide"))
            {
                _manager.InteractBarrierArea();
                _manager.SwitchState(MoneyWorkerStates.MoveToBase);
            }
        }
    }
}