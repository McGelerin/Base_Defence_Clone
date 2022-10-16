using System;
using Abstract;
using AIBrain;
using Enums;
using UnityEngine;
using UnityEngine.AI;

namespace States.MoneyWorker
{
    public class MoveToMoneyPosition : MoneyWorkerBaseState
    {
        #region Self Variables

        #region Private Variables
        
        private MoneyWorkerAIBrain _manager;
        private NavMeshAgent _agent;
        private float _currentTime;

        #endregion

        #endregion

        public MoveToMoneyPosition(ref MoneyWorkerAIBrain manager, ref NavMeshAgent agent)
        {
            _manager = manager;
            _agent = agent;
        }


        public override void EnterState()
        {
            _agent.SetDestination(_manager.Target.transform.localPosition);
        }

        public override void UpdateState()
        {
            _manager.AnimFloatState(_agent.velocity.magnitude);
            _currentTime += Time.deltaTime;
            if (!(_currentTime >= 1f))
            {
                return;
            }
            
            if (!_manager.IsFullStack())
            {
                _manager.SwitchState(MoneyWorkerStates.MoveToRemoveStackState);
            }
            _currentTime = 0;
        }

        public override void OnTriggerEnterState(Collider other)
        {
            if (other.CompareTag("Money"))
            {
                _manager.InteractMoney(other.gameObject);
            }
        }
    }
}