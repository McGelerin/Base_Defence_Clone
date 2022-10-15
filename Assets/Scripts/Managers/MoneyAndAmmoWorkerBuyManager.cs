using Data.UnityObject;
using Data.ValueObject;
using Keys;
using Signals;
using UnityEngine;

namespace Managers
{
    public class MoneyAndAmmoWorkerBuyManager : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables

        private SupporterData _data;
        private SupporterBuyableDataParams _payedAmounts;
        private int _baseLevel;
        private int _ammoWorkerPayedAmount;

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
            DataTransferSignals.Instance.onGetAmmoWorkerData += OnGetAmmoWorkerData;
            DataTransferSignals.Instance.onGetPayedWorkerAreaData += OnGetAmmoWorkerAreaPayedAmount;
            DataTransferSignals.Instance.onAmmoWorkerAreaBuyedItems += OnSetPayedAmmoWorkerAreaData;
            SaveSignals.Instance.onSaveSupporterData += OnGetSupporterData;
        }

        private void UnsubscribeEvents()
        {
            DataTransferSignals.Instance.onGetAmmoWorkerData -= OnGetAmmoWorkerData;
            DataTransferSignals.Instance.onGetPayedWorkerAreaData -= OnGetAmmoWorkerAreaPayedAmount;
            DataTransferSignals.Instance.onAmmoWorkerAreaBuyedItems -= OnSetPayedAmmoWorkerAreaData;
            SaveSignals.Instance.onSaveSupporterData -= OnGetSupporterData;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void Start()
        {
            DataTransferSignals.Instance.onGettedSupporterData?.Invoke();
        }

        private void GetReferences()
        {
            _baseLevel = LevelSignals.Instance.onGetLevelID();
            _data = Resources.Load<CD_Level>("Data/CD_Level").LevelDatas[_baseLevel].SupporterData;
            _payedAmounts = SaveSignals.Instance.onLoadSupporterData();
        }
        
        private AmmoWorkerBuyData OnGetAmmoWorkerData() => _data.AmmoWorkerBuyData;

        private int OnGetAmmoWorkerAreaPayedAmount()
        {
            return _payedAmounts.AmmoWorkerPayedAmount;
        }
        
        private void OnSetPayedAmmoWorkerAreaData(int payedAmount)
        {
            _ammoWorkerPayedAmount = payedAmount;
            SupportAreaDataSave();
        }

        private void SupportAreaDataSave()
        {
            _payedAmounts = new SupporterBuyableDataParams()
            {
                AmmoWorkerPayedAmount = _ammoWorkerPayedAmount
            };
            SaveSignals.Instance.onSupporterDataSave?.Invoke();
            SaveSignals.Instance.onScoreSave?.Invoke();
        }

        private SupporterBuyableDataParams OnGetSupporterData() => _payedAmounts;
    }
}