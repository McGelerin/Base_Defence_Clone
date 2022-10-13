using System.Collections;
using System.Collections.Generic;
using Abstract;
using Command.StackCommand;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Signals;
using States.AmmoWorker;
using UnityEngine;
using UnityEngine.AI;

namespace AIBrain
{
    public class AmmoWorkerAIBrain : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables
        
        public GameObject Target;

        #endregion

        #region Serialized Variables

        [SerializeField] private GameObject stackHolder;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;
        #endregion

        #region Private Variables
        
        private Coroutine _waitForAmmoArea;
        private AmmoWorkerAIData _data;
        private GameObject _turretAmmoAreaCache;
        private GameObject _turretAmmoAreaParentCache;
        private Vector3 _stackPositionCache;
        private List<GameObject> _stackList = new List<GameObject>();
        private ItemAddOnStack _objAddOnStack;
        private DinamicStackItemPosition _ammoDinamicStackItemPosition;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.3f);

        #region States

        private MoveToWareHouseArea _moveToWareHouse;
        private WaitForFullStack _waitForFullStack;
        private MoveToTurretAmmoArea _moveToTurretAmmoArea;
        private WaitToAmmoArea _waitToAmmoArea;
        private AmmoWorkerBaseState _currentState;
        private AnyState _anyState;

        #endregion

        #endregion

        #endregion

        private void Awake()
        {
            _data = GetTurretData();
            var brain = this;
            _ammoDinamicStackItemPosition = new DinamicStackItemPosition(ref _stackList, ref _data.WorkerStackData, ref stackHolder);
            _objAddOnStack = new ItemAddOnStack(ref _stackList, ref stackHolder, ref _data.WorkerStackData);
            _moveToWareHouse = new MoveToWareHouseArea(ref brain, ref agent);
            _waitForFullStack = new WaitForFullStack(ref brain, ref agent);
            _moveToTurretAmmoArea = new MoveToTurretAmmoArea(ref brain, ref agent);
            _waitToAmmoArea = new WaitToAmmoArea(ref brain);
            _anyState = new AnyState(ref brain, ref agent);
        }

        private AmmoWorkerAIData GetTurretData() => Resources.Load<Cd_AI>("Data/CD_AI").AmmoWorkerAIData;

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
            _currentState = _moveToWareHouse;
            _currentState.EnterState();
        }

        private void SubscribeEvents()
        {
            WorkerSignals.Instance.onAmmoAreaFull += TurretAmmoAreaFull;
        }

        private void UnsubscribeEvents()
        {
            WorkerSignals.Instance.onAmmoAreaFull -= TurretAmmoAreaFull;
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

        public void SwitchState(AmmoWorkerStates state)
        {
            _currentState = state switch
            {
                AmmoWorkerStates.MoveToWareHouse => _moveToWareHouse,
                AmmoWorkerStates.WaitForFullStack => _waitForFullStack,
                AmmoWorkerStates.MoveToTurretAmmoArea => _moveToTurretAmmoArea,
                AmmoWorkerStates.WaitToAmmoArea => _waitToAmmoArea,
                AmmoWorkerStates.AnyState => _anyState,
                _ => _currentState
            };
            _currentState.EnterState();
        }

        private void TurretAmmoAreaFull(GameObject area)
        {
            if(area != _turretAmmoAreaParentCache) return;
            SwitchState(AmmoWorkerStates.AnyState);
            CoroutineCheck();
        }
        
        public void AnimTriggerState(WorkerAnimState animState)
        {
            animator.SetTrigger(animState.ToString());
        }

        private void CoroutineCheck()
        {
            if (_waitForAmmoArea == null) return;
            StackSignals.Instance.onDecreseStackHolder?.Invoke(_turretAmmoAreaCache);
            StopCoroutine(_waitForAmmoArea);
            _waitForAmmoArea = null;
        }

        public void TurretAmmoArea(GameObject turretAmmoArea)
        {
            _turretAmmoAreaCache = turretAmmoArea;
            _turretAmmoAreaParentCache = _turretAmmoAreaCache.transform.parent.gameObject;
        }
        
        public void InteractTurretAmmoArea()
        {
            _waitForAmmoArea = StartCoroutine(WaitForAmmoArea());
        }

        public void InteractWareHouseArea()
        {
            StartCoroutine(AddAmmoToStack());
        }
        
        public void Wait()
        {
            StartCoroutine(ZeroPointWait());
        }

        private IEnumerator ZeroPointWait()
        {
            yield return new WaitForSeconds(2f);
            SwitchState(_stackList.Count > 0
                ? AmmoWorkerStates.MoveToTurretAmmoArea
                : AmmoWorkerStates.MoveToWareHouse);
        }
        
        private IEnumerator AddAmmoToStack()
        {
            while (_stackList.Count < _data.WorkerStackData.Capacity)
            {
                _stackPositionCache = _ammoDinamicStackItemPosition.Execute(_stackPositionCache);
                var obj = PoolSignals.Instance.onGetPoolObject(PoolType.AmmoBox.ToString(), Target.transform);
                _objAddOnStack.Execute(obj,_stackPositionCache);
                yield return _waitForSeconds;
            }
            SwitchState(AmmoWorkerStates.MoveToTurretAmmoArea);
        }
        
        private IEnumerator WaitForAmmoArea( )
        {
            StackSignals.Instance.onInteractStackHolder?.Invoke(_turretAmmoAreaCache,_stackList);
            while (_stackList.Count > 0)
            {
                yield return _waitForSeconds;
            }
            StackSignals.Instance.onDecreseStackHolder?.Invoke(_turretAmmoAreaCache);
            SwitchState(AmmoWorkerStates.MoveToWareHouse);
            _waitForAmmoArea = null;
        }
    }
}