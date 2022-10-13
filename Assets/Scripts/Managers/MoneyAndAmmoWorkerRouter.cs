using System;
using System.Collections.Generic;
using Data.UnityObject;
using Data.ValueObject;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Managers
{
    public class MoneyAndAmmoWorkerRouter : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables
        

        #endregion

        #region Serialized Variables
        
        

        #endregion

        #region Private Variables

        private GameObject _warHouse;
        private GameObject _turretArea;
        private StaticStackData _turretData;
        [ShowInInspector]private Dictionary<GameObject,List<GameObject>> _turretAmmoAreas = new Dictionary<GameObject, List<GameObject>>();
        
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
            //WorkerSignals.Instance.onWareHouse += OnWareHouseGO;
            WorkerSignals.Instance.onTurretAmmoAreas += OnTurretAmmoAreas;
            //WorkerSignals.Instance.onGetWarHouseArea += OnGetWarHouseArea;
            WorkerSignals.Instance.onGetTurretArea += OnGetTurretArea;
            //WorkerSignals.Instance.onRemainingCapacity += OnGetRemainingCapacity;
        }

        private void UnsubscribeEvents()
        {
            //WorkerSignals.Instance.onWareHouse -= OnWareHouseGO;
            WorkerSignals.Instance.onTurretAmmoAreas -= OnTurretAmmoAreas;
            //WorkerSignals.Instance.onGetWarHouseArea -= OnGetWarHouseArea;
            WorkerSignals.Instance.onGetTurretArea -= OnGetTurretArea;
            //WorkerSignals.Instance.onRemainingCapacity -= OnGetRemainingCapacity;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        // private void OnWareHouseGO(GameObject wareHouse)
        // {
        //     _warHouse = wareHouse;
        // }

        private void OnTurretAmmoAreas(GameObject ammoArea, List<GameObject> ammoAreaStackList)
        {
            _turretAmmoAreas.Add(ammoArea,ammoAreaStackList);
        }

       // private GameObject OnGetWarHouseArea() => _warHouse;

        private GameObject OnGetTurretArea()
        {
            var capacity = int.MaxValue;
            foreach (var VARIABLE in _turretAmmoAreas)
            {
                if (capacity > _turretData.Capacity - VARIABLE.Value.Count) continue;
                capacity = VARIABLE.Value.Count;
                _turretArea = VARIABLE.Key;
            }
            return _turretArea;
        }

        //private int OnGetRemainingCapacity(GameObject Area) => _turretData.Capacity - _turretAmmoAreas[Area].Count;
    }
}