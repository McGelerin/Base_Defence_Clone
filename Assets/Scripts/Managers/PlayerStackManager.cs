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
        [ShowInInspector]private List<GameObject> _stackList = new List<GameObject>();
        private StackType _stackType;
        private StackData _moneyStackData;
        private StackData _ammoStackData;
        private Vector3 _stackPositionCache;

        private StackItemPosition _moneyStackItemPosition;
        private StackItemPosition _ammoStackItemPosition;
        private ItemAddOnStack _objAddOnStack;
        private AddMoneyStackToScore _addMoneyStackToScore;
        

        #endregion
        #endregion

        private void Awake()
        {
            _moneyStackData = Resources.Load<CD_PlayerData>("Data/CD_Player").MoneyStackData;
            _ammoStackData = Resources.Load<CD_PlayerData>("Data/CD_Player").AmmoStackData;
            _moneyStackItemPosition = new StackItemPosition(ref _stackList,ref _moneyStackData, ref stackHolder);
            _ammoStackItemPosition = new StackItemPosition(ref _stackList, ref _ammoStackData, ref stackHolder);
            _objAddOnStack = new ItemAddOnStack(ref _stackList, ref stackHolder, ref _moneyStackData);
            _addMoneyStackToScore = new AddMoneyStackToScore(ref _stackList, ref stackHolder);
        }
        
        #region Event Subscription

        private void OnEnable()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            StackSignals.Instance.onGetHostageTarget += OnGetHostageTarget;
            StackSignals.Instance.onLastGameObjectRemone += OnLastHostageRemove;
            StackSignals.Instance.onGetHostageList += onGetHostageList;
        }


        private void Unsubscribe()
        {
            StackSignals.Instance.onGetHostageTarget -= OnGetHostageTarget;
            StackSignals.Instance.onLastGameObjectRemone -= OnLastHostageRemove;
            StackSignals.Instance.onGetHostageList -= onGetHostageList;
        }
        
        private void OnDisable()
        {
            Unsubscribe();
        }

        #endregion
        
        
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

        private void OnLastHostageRemove ()
        {
            PoolSignals.Instance.onReleasePoolObject?.Invoke(PoolType.Hostage.ToString(),_hostageList.Last());
            _hostageList.Remove(_hostageList.Last());
            _hostageList.TrimExcess();
        }
        
        private List<GameObject> onGetHostageList() => _hostageList;

        public void IncreaseMoney(GameObject money)
        {
            if (_stackType == StackType.AMMO) return;
            money.GetComponent<BoxCollider>().enabled = false;
            _stackType = StackType.MONEY;
            _stackPositionCache = _moneyStackItemPosition.Execute(_stackPositionCache);
            _objAddOnStack.Execute(money,_stackPositionCache);
        }

        public void IncreaseAmmoArea(Transform ammoArea,bool isTriggerAmmoArea)
        {
            if (_stackType == StackType.MONEY) return;
            if (isTriggerAmmoArea)
            {
                StartCoroutine(AddAmmoToStack(ammoArea));
            }
            else
            {
                StopAllCoroutines();
            }
        }

        private IEnumerator AddAmmoToStack(Transform ammoArea)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(0.3f);
            while (_stackList.Count < _ammoStackData.Capacity)
            {
                _stackType = StackType.AMMO;
                _stackPositionCache = _ammoStackItemPosition.Execute(_stackPositionCache);
                var obj = PoolSignals.Instance.onGetPoolObject(PoolType.Ammo.ToString(), ammoArea);
                _objAddOnStack.Execute(obj,_stackPositionCache);
                yield return waitForSeconds;
            }
        }

        public void IncreaseAmmoHolderArea()
        {
            
        }
        
        public void IncreaseBarrierArea()
        {
            switch (_stackType)
            {
                case StackType.NONE:
                    return;
                case StackType.MONEY:
                    _addMoneyStackToScore.Execute();
                    _stackType = StackType.NONE;
                    return;
                case StackType.AMMO:
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}