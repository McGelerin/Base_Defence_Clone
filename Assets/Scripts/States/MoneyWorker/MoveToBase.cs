using Abstract;
using AIBrain;
using Enums;
using ES3Types;
using Signals;
using UnityEngine;
using UnityEngine.AI;

namespace States.MoneyWorker
{
    public class MoveToBase : MoneyWorkerBaseState
    {
        #region Self Variables

        #region Private Variables
        
        private MoneyWorkerAIBrain _manager;
        private NavMeshAgent _agent;
        private float _currentTime;

        #endregion

        #endregion

        public MoveToBase(ref MoneyWorkerAIBrain manager, ref NavMeshAgent agent)
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
            Debug.Log(_agent.remainingDistance);
            if (_agent.remainingDistance > 0.5f)
            {
                return;
            }
            _currentTime += Time.deltaTime;
            if (!(_currentTime >= 1f))
            {
                return;
            }
            _manager.Target = WorkerSignals.Instance.onGetMoneyGameObject();
            
            if (_manager.Target != null)
            {
                _manager.SwitchState(MoneyWorkerStates.MoveToMoneyPosition);
            }
            _currentTime = 0;
        }

        public override void OnTriggerEnterState(Collider other) { }
    }
}