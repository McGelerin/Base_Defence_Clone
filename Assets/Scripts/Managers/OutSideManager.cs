using System;
using System.Collections.Generic;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Managers
{
    public class OutSideManager : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables

        [ShowInInspector]private FrontYardData _data;
        private Dictionary<OutSideStateLevels,int> _payedStageAreas;
        private int _baseLevel;
        
        #endregion

        #endregion

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
            GetReferences();
        }

        private void SubscribeEvents()
        {
            DataTransferSignals.Instance.onGetOutsideData += OnGetOutsideData;
            DataTransferSignals.Instance.onGetPayedStageData += OnGetOutsideStagePayedAmount;
            DataTransferSignals.Instance.onOutsideBuyedItems += OnSetPayedStageData;
            SaveSignals.Instance.onSaveOutsideData += OnGetOutsideData;
        }

        private void UnsubscribeEvents()
        {
            DataTransferSignals.Instance.onGetOutsideData -= OnGetOutsideData;
            DataTransferSignals.Instance.onGetPayedStageData -= OnGetOutsideStagePayedAmount;
            DataTransferSignals.Instance.onOutsideBuyedItems -= OnSetPayedStageData;
            SaveSignals.Instance.onSaveOutsideData -= OnGetOutsideData;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void Start()
        {
            DataTransferSignals.Instance.onGettedOutSideData?.Invoke();
        }

        private void GetReferences()
        {
            _baseLevel = LevelSignals.Instance.onGetLevelID();
            _data = Resources.Load<CD_Level>("Data/CD_Level").LevelDatas[_baseLevel].FrontYardData;
            _payedStageAreas = SaveSignals.Instance.onLoadOutsideData();
        }

        private OutsideData OnGetOutsideData(OutSideStateLevels level) => _data.OutsideLevelData[(int)level];

        private int OnGetOutsideStagePayedAmount(OutSideStateLevels level)
        {
            if (!_payedStageAreas.ContainsKey(level)) _payedStageAreas.Add(level, 0);
            return _payedStageAreas[level];
        }
        
        private void OnSetPayedStageData(OutSideStateLevels level,int payedAmount)
        {
            _payedStageAreas[level] = payedAmount;
            OutsideDataSave();
        }
        
        private void OutsideDataSave()
        {
            SaveSignals.Instance.onOutSideDataSave?.Invoke();
        }

        private Dictionary<OutSideStateLevels, int> OnGetOutsideData() => _payedStageAreas;
        
    }
}