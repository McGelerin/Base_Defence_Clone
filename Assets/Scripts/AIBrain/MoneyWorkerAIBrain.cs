using System.Collections.Generic;
using Abstract;
using Command.StackCommand;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Signals;
using States.MoneyWorker;
using UnityEngine;
using UnityEngine.AI;

namespace AIBrain
{
    public class MoneyWorkerAIBrain : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public GameObject Target;
        public GameObject Base;

        #endregion

        #region Serialized Variables

        [SerializeField] private GameObject stackHolder;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;

        #endregion

        #region Private Variables

        private MoneyWorkerData _data;
        private Vector3 _stackPositionCache;
        private List<GameObject> _stackList = new List<GameObject>();
        private ItemAddOnStack _objAddOnStack;
        private DynamicStackItemPosition _moneyDynamicStackItemPosition;
        private AddMoneyStackToScore _addMoneyStackToScore;

        #endregion

        #region States

        private MoneyWorkerBaseState _currentState;
        private MoveToBase _moveToBase;
        private MoveToRemoveStack _moveToRemoveStack;
        private MoveToMoneyPosition _moveToMoneyPosition;

        #endregion

        #endregion

        private void Awake()
        {
            _data = GetTurretData();
            var brain = this;
            _moneyDynamicStackItemPosition = new DynamicStackItemPosition(ref _stackList, ref _data.WorkerStackData, ref stackHolder);
            _objAddOnStack = new ItemAddOnStack(ref _stackList, ref stackHolder, ref _data.WorkerStackData);
            _addMoneyStackToScore = new AddMoneyStackToScore(ref _stackList);
            _moveToRemoveStack = new MoveToRemoveStack(ref brain, ref agent);
            _moveToMoneyPosition = new MoveToMoneyPosition(ref brain, ref agent);
            _moveToBase = new MoveToBase(ref brain, ref agent);
        }
        
        private MoneyWorkerData GetTurretData() => Resources.Load<CD_AI>("Data/CD_AI").MoneyWorkerData;

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
            Base = WorkerSignals.Instance.onGetBaseCenter();
            _currentState = _moveToBase;
            _currentState.EnterState();
        }

        private void SubscribeEvents()
        {
            WorkerSignals.Instance.onChangeDestination += OnChangeDestination;
        }

        private void UnsubscribeEvents()
        {
            WorkerSignals.Instance.onChangeDestination -= OnChangeDestination;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            _currentState.OnTriggerEnterState(other);
        }

        private void Update()
        {
            _currentState.UpdateState();
        }
        
        public void SwitchState(MoneyWorkerStates state)
        {
            _currentState = state switch
            {
                MoneyWorkerStates.MoveToBase => _moveToBase,
                MoneyWorkerStates.MoveToMoneyPosition => _moveToMoneyPosition,
                MoneyWorkerStates.MoveToRemoveStackState => _moveToRemoveStack,
                _ => _currentState
            };
            _currentState.EnterState();
        }

        public void AnimFloatState(float speed)
        {
            animator.SetFloat("Speed", speed);
        }

        private void OnChangeDestination()
        {
            if (!IsFullStack())
            {
                SwitchState(MoneyWorkerStates.MoveToRemoveStackState);
            }
            else
            {
                Target = WorkerSignals.Instance.onGetMoneyGameObject();
                SwitchState(Target == null 
                    ? MoneyWorkerStates.MoveToBase 
                    : MoneyWorkerStates.MoveToMoneyPosition);
            }
        }

        public bool IsFullStack()
        {
            return _stackList.Count < _data.WorkerStackData.Capacity;
        }
        
        public void InteractMoney(GameObject money)
        {
            if (_stackList.Count >= _data.WorkerStackData.Capacity) return; // switch state
            money.GetComponent<BoxCollider>().enabled = false;
            _stackPositionCache = _moneyDynamicStackItemPosition.Execute(_stackPositionCache);
            _objAddOnStack.Execute(money,_stackPositionCache);
            WorkerSignals.Instance.onRemoveMoneyFromList?.Invoke(money);
        }
        
        public void InteractBarrierArea()
        {
            _addMoneyStackToScore.Execute();
        }
    }
}