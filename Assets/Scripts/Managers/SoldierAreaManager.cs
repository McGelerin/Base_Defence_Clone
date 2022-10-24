using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data.ValueObject;
using Enums;
using Signals;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class SoldierAreaManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables
        
        [SerializeField] private TextMeshPro tmp;
        [SerializeField] private GameObject soldierBarrackFrontDoor;
        [SerializeField] private GameObject soldierBarrackBackDoor;
        [SerializeField] private GameObject soldierSearchPosition;
        [SerializeField] private List<GameObject> soldierInitPositions;
        

        #endregion

        #region Private Variables

        private Coroutine _getSoldier;
        private SoldierAreaData _data;
        private List<GameObject> _hostageGameObjects;
        private int _currentSoldier;
        private int _entrySoldier = 0;
        private int _initPositionCache;
        private WaitForSeconds _wait = new WaitForSeconds(5f);

        #endregion

        #endregion

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            IdleSignals.Instance.onSoldierInitPosition += OnGetInitPosition;
            IdleSignals.Instance.onSoldierSearchInitPosition += OnGetSoldierSearchPosition;
            IdleSignals.Instance.onHostageEntryBarrack += OnHostageEntryBarrack;
            IdleSignals.Instance.onGetSoldierBarrack += OnGetSoldierBarrack;
            WorkerSignals.Instance.onSoldierDeath += OnSoldierDead;
        }

        private void UnsubscribeEvents()
        {
            IdleSignals.Instance.onSoldierInitPosition -= OnGetInitPosition;
            IdleSignals.Instance.onSoldierSearchInitPosition -= OnGetSoldierSearchPosition;
            IdleSignals.Instance.onHostageEntryBarrack -= OnHostageEntryBarrack;
            IdleSignals.Instance.onGetSoldierBarrack -= OnGetSoldierBarrack;
            WorkerSignals.Instance.onSoldierDeath -= OnSoldierDead;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void Start()
        {
            _currentSoldier = 0;
            _data = DataTransferSignals.Instance.onGetSoldierAreaData();
            _hostageGameObjects = StackSignals.Instance.onGetHostageList();
            SetText();
        }

        public void PlayerEntrySoldierArea()
        {
            _hostageGameObjects = StackSignals.Instance.onGetHostageList();
            if (_hostageGameObjects.Count <= 0) return;
            while (_currentSoldier < _data.MaxWorkerAmount)
            {
                if (_hostageGameObjects.Count < 1) break;
                IdleSignals.Instance.onPlayerEntrySoldierArea?.Invoke(_hostageGameObjects.Last());
                StackSignals.Instance.onLastGameObjectRemove?.Invoke(false);
                _currentSoldier++;
                _entrySoldier++;
                SetText();
            }
            
        }

        private void OnHostageEntryBarrack()
        {
            _getSoldier ??= StartCoroutine(GetSoldier());
        }

        private GameObject OnGetSoldierBarrack() => soldierBarrackFrontDoor;

        private GameObject OnGetInitPosition()
        {
            var initPosition = soldierInitPositions[_initPositionCache];
            _initPositionCache++;
            return initPosition;
        }

        private GameObject OnGetSoldierSearchPosition() => soldierSearchPosition;

        private void OnSoldierDead()
        {
            _currentSoldier--;
            _initPositionCache--;
            SetText();
        }
        
        private void SetText()
        {
            tmp.SetText(_currentSoldier+ " / " + _data.MaxWorkerAmount);
        }

        private IEnumerator GetSoldier()
        {
            yield return _wait;
            for (int i = 0; i < _entrySoldier; i++)
            {
                PoolSignals.Instance.onGetPoolObject(PoolType.Soldier.ToString(), soldierBarrackBackDoor.transform);
            }

            _entrySoldier = 0;
            _getSoldier = null;
        }

    }
}