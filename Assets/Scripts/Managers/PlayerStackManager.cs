using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Command.StackCommand;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Managers
{
    public class PlayerStackManager : MonoBehaviour
    {
        #region Self Variables

        #region Serializefield Variables

        [SerializeField] private GameObject stackHolder;

        #endregion
        
        #region Private Variables
        private readonly List<GameObject> _hostageList = new List<GameObject>();
        private List<GameObject> _stackList = new List<GameObject>();
        private StackType _stackType;
        private StackData _moneyStackData;
        private StackData _ammoStackData;
        private Vector3 _stackPositionCache;

        private DynamicStackItemPosition _moneyDynamicStackItemPosition;
        private DynamicStackItemPosition _ammoDynamicStackItemPosition;
        private ItemAddOnStack _objAddOnStack;
        private AddMoneyStackToScore _addMoneyStackToScore;
        private RemoveAmmoStackItems _removeAmmoStackItems;
        private PlayerDeathClearStack _playerDeathClearStack;

        private Transform _warHouseTransform;
        

        #endregion
        #endregion

        private void Awake()
        {
            var transformParent = transform.parent.transform;
            _moneyStackData = Resources.Load<CD_PlayerData>("Data/CD_Player").MoneyStackData;
            _ammoStackData = Resources.Load<CD_PlayerData>("Data/CD_Player").AmmoStackData;
            _moneyDynamicStackItemPosition = new DynamicStackItemPosition(ref _stackList,ref _moneyStackData, ref stackHolder);
            _ammoDynamicStackItemPosition = new DynamicStackItemPosition(ref _stackList, ref _ammoStackData, ref stackHolder);
            _objAddOnStack = new ItemAddOnStack(ref _stackList, ref stackHolder, ref _moneyStackData);
            _addMoneyStackToScore = new AddMoneyStackToScore(ref _stackList);
            _playerDeathClearStack = new PlayerDeathClearStack(ref _stackList, ref transformParent);
        }
        
        #region Event Subscription

        private void OnEnable()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            StackSignals.Instance.onGetHostageTarget += OnGetHostageTarget;
            StackSignals.Instance.onLastGameObjectRemove += OnLastHostageRemove;
            StackSignals.Instance.onGetHostageList += OnGetHostageList;
            IdleSignals.Instance.onPlayerDeath += OnPlayerDeath;
        }


        private void Unsubscribe()
        {
            StackSignals.Instance.onGetHostageTarget -= OnGetHostageTarget;
            StackSignals.Instance.onLastGameObjectRemove -= OnLastHostageRemove;
            StackSignals.Instance.onGetHostageList -= OnGetHostageList;
            IdleSignals.Instance.onPlayerDeath -= OnPlayerDeath;
        }
        
        private void OnDisable()
        {
            Unsubscribe();
        }

        #endregion

        private void Start()
        {
            _warHouseTransform = IdleSignals.Instance.onGetWarHousePositon();
            _removeAmmoStackItems = new RemoveAmmoStackItems(ref _stackList, _warHouseTransform);
        }

        private GameObject OnGetHostageTarget(GameObject hostage)
        {
            if (_hostageList.Count == 0)
            {
                _hostageList.Add(hostage);
                return transform.gameObject;
            }
            _hostageList.Add(hostage);
            return _hostageList[_hostageList.Count - 2];
        }

        private void OnLastHostageRemove(bool sendPool)
        {
            if (sendPool)
            {
                PoolSignals.Instance.onReleasePoolObject?.Invoke(PoolType.Hostage.ToString(),_hostageList.Last());
            }
            _hostageList.Remove(_hostageList.Last());
            _hostageList.TrimExcess();
        }
        
        private List<GameObject> OnGetHostageList() => _hostageList;

        public void InteractMoney(GameObject money)
        {
            if (_stackType == StackType.Ammo) return;
            _stackType = StackType.Money;
            if (_stackList.Count >= _moneyStackData.Capacity) return;
            money.GetComponent<BoxCollider>().enabled = false;
            _stackPositionCache = _moneyDynamicStackItemPosition.Execute(_stackPositionCache);
            _objAddOnStack.Execute(money,_stackPositionCache);
            WorkerSignals.Instance.onRemoveMoneyFromList?.Invoke(money);
        }

        public void InteractWareHouseArea(Transform ammoArea,bool isTriggerAmmoArea)
        {
            if (_stackType == StackType.Money) return;
            if (isTriggerAmmoArea)
            {
                StartCoroutine(AddAmmoToStack(ammoArea));
            }
            else
            {
                StopAllCoroutines();
            }
        }

        public void AmmoStackCheck()
        {
            if (_stackList.Count == 0)
            {
                _stackType = StackType.None;
            }
        }

        private IEnumerator AddAmmoToStack(Transform ammoArea)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(0.3f);
            while (_stackList.Count < _ammoStackData.Capacity)
            {
                _stackType = StackType.Ammo;
                _stackPositionCache = _ammoDynamicStackItemPosition.Execute(_stackPositionCache);
                var obj = PoolSignals.Instance.onGetPoolObject(PoolType.AmmoBox.ToString(), ammoArea);
                _objAddOnStack.Execute(obj,_stackPositionCache);
                yield return waitForSeconds;
            }
        }

        public void InteractTurretAmmoArea(GameObject AmmoArea)
        {
            if (_stackType == StackType.Ammo)
            {
                StackSignals.Instance.onInteractStackHolder?.Invoke(AmmoArea,_stackList);
            }
        }

        public void InteractBarrierArea()
        {
            switch (_stackType)
            {
                case StackType.None:
                    return;
                case StackType.Money:
                    _addMoneyStackToScore.Execute();
                    _stackType = StackType.None;
                    return;
                case StackType.Ammo:
                    _removeAmmoStackItems.Execute();
                    _stackType = StackType.None;
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnPlayerDeath()
        {
            _playerDeathClearStack.Execute();
            _stackType = StackType.None;
        }
    }
}