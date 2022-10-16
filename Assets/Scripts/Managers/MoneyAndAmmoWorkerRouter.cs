using System;
using System.Collections.Generic;
using Data.UnityObject;
using Data.ValueObject;
using Signals;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Managers
{
    public class MoneyAndAmmoWorkerRouter : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables

        private GameObject _turretArea;
        private StaticStackData _turretData;
        private Dictionary<GameObject,List<GameObject>> _turretAmmoAreas = new Dictionary<GameObject, List<GameObject>>();
        private List<GameObject> _moneyList = new List<GameObject>();
        private GameObject _targetMoneyCache;

        #endregion

        private void Awake()
        {
            _turretData = GetTurretData();
        }
        
        private StaticStackData GetTurretData() => Resources.Load<CD_Turret>("Data/CD_Turret").TurretData.TurretStackData;

        #endregion

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            WorkerSignals.Instance.onTurretAmmoAreas += OnTurretAmmoAreas;
            WorkerSignals.Instance.onGetTurretArea += OnGetTurretArea;
            WorkerSignals.Instance.onAddListToMoney += OnAddListToMoney;
            WorkerSignals.Instance.onRemoveMoneyFromList += OnRemoveMoneyFromList;
            WorkerSignals.Instance.onGetMoneyGameObject += OnGetMoneyGameObject;
        }

        private void UnsubscribeEvents()
        {
            WorkerSignals.Instance.onTurretAmmoAreas -= OnTurretAmmoAreas;
            WorkerSignals.Instance.onGetTurretArea -= OnGetTurretArea;
            WorkerSignals.Instance.onAddListToMoney -= OnAddListToMoney;
            WorkerSignals.Instance.onRemoveMoneyFromList -= OnRemoveMoneyFromList;
            WorkerSignals.Instance.onGetMoneyGameObject -= OnGetMoneyGameObject;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void OnTurretAmmoAreas(GameObject ammoArea, List<GameObject> ammoAreaStackList)
        {
            _turretAmmoAreas.Add(ammoArea,ammoAreaStackList);
        }

        private GameObject OnGetTurretArea()
        {
            var maxCapacity = int.MinValue;
            foreach (var VARIABLE in _turretAmmoAreas)
            {
                var remainingCapacity = _turretData.Capacity - VARIABLE.Value.Count;
                if (remainingCapacity <= maxCapacity) continue;
                maxCapacity = remainingCapacity;
                _turretArea = VARIABLE.Key;
            }
            return _turretArea;
        }

        private void OnAddListToMoney(GameObject money)
        {
            if (!_moneyList.Contains(money))
            {
                _moneyList.Add(money);
            }
        }

        private void OnRemoveMoneyFromList(GameObject money)
        {
            _moneyList.Remove(money);
            _moneyList.TrimExcess();
            if (money == _targetMoneyCache)
            {
                WorkerSignals.Instance.onChangeDestination?.Invoke();
            }
        }

        private GameObject OnGetMoneyGameObject()
        {
            if (_moneyList.IsNullOrEmpty()) return null;
            _targetMoneyCache = _moneyList[0];
            return _targetMoneyCache;
        }
    }
}