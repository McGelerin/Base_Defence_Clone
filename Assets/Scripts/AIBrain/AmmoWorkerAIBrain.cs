using System;
using System.Collections;
using System.Collections.Generic;
using Command.StackCommand;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Signals;
using UnityEngine;

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
        
        #endregion

        #region Private Variables

        private AmmoWorkerAIData _data;
        private Vector3 _stackPositionCache;
        private List<GameObject> _stackList = new List<GameObject>();
        private ItemAddOnStack _objAddOnStack;
        private DinamicStackItemPosition _ammoDinamicStackItemPosition;

        #endregion

        #endregion

        private void Awake()
        {
            _data = GetTurretData();
            _ammoDinamicStackItemPosition = new DinamicStackItemPosition(ref _stackList, ref _data.WorkerStackData, ref stackHolder);
            _objAddOnStack = new ItemAddOnStack(ref _stackList, ref stackHolder, ref _data.WorkerStackData);
        }

        private AmmoWorkerAIData GetTurretData() => Resources.Load<Cd_AI>("Data/CD_AI").AmmoWorkerAIData;

        private void Update()
        {
            throw new NotImplementedException();
        }

        private void OnTriggerEnter(Collider other)
        {
            throw new NotImplementedException();
        }

        private void OnTriggerExit(Collider other)
        {
            throw new NotImplementedException();
        }

        public void SwitchState()
        {
            
            
        }

        public void InteractWareHouseArea(Transform ammoArea)
        {
            StartCoroutine(AddAmmoToStack(ammoArea));
        }

        public IEnumerator AddAmmoToStack(Transform ammoArea)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(0.3f);
            while (_stackList.Count < _data.WorkerStackData.Capacity)
            {
                _stackPositionCache = _ammoDinamicStackItemPosition.Execute(_stackPositionCache);
                var obj = PoolSignals.Instance.onGetPoolObject(PoolType.AmmoBox.ToString(), ammoArea);
                _objAddOnStack.Execute(obj,_stackPositionCache);
                yield return waitForSeconds;
            }
            //swicthstate yapılacak
        }
    }
}