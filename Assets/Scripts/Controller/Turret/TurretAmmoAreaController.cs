using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Data.ValueObject;
using Enums;
using Managers;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Controllers
{
    public class TurretAmmoAreaController : MonoBehaviour
    {
        
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private TurretManager manager;

        #endregion

        #region Private Variables
        
        private Coroutine _addStack;
        [ShowInInspector]private List<GameObject> _managerStackListCache;
        private List<GameObject> _ammoListCache;
        private StaticStackData _data;
        private WaitForSeconds _delay;
        private Vector3 _direct;
        private bool _isInteractPlayer;
        private bool _isInteractAmmoWorker;
        
        #endregion

        #endregion

        public void SetData(StaticStackData data,List<GameObject> managerStackList)
        {
            _managerStackListCache = managerStackList;
            _data = data;
            _delay = new WaitForSeconds(data.Delay);
            WorkerSignals.Instance.onTurretAmmoAreas?.Invoke(gameObject,managerStackList);
        }

        public void AmmoAddToStack(List<GameObject> AmmoBoxs)
        {
            _ammoListCache = AmmoBoxs;
            if (_addStack != null) return;
            _isInteractPlayer = true;
            _addStack = StartCoroutine(AmmoAdd());
        }

        public void PlayerUnInteractAmmoArea()
        {
            _isInteractPlayer = false;
        }

        public void DecreaseStackList()
        {
            var ammoBox = _managerStackListCache.Last();
            PoolSignals.Instance.onReleasePoolObject(PoolType.AmmoBox.ToString(), ammoBox);
            _managerStackListCache.Remove(ammoBox);
        }

        private IEnumerator AmmoAdd()
        {
            while (_managerStackListCache.Count < _data.Capacity && (_isInteractPlayer || _isInteractAmmoWorker))
            {
                if (_ammoListCache.Count == 0) break;
                var ammoBox = _ammoListCache.Last();
                _direct = manager.ItemPosition.Execute(_direct);
                manager.AddOnStack.Execute(ammoBox, _direct);
                _ammoListCache.Remove(ammoBox);
                yield return _delay;
            }
            WorkerSignals.Instance.onAmmoAreaFull?.Invoke(gameObject);
            _addStack = null;
        }
    }
}