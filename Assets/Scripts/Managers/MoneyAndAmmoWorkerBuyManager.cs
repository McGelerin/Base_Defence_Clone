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
        private int _moneyWorkerPayedAmount;

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
            DataTransferSignals.Instance.onGetMoneyWorkerData += OnGetMoneyWorkerData;
            DataTransferSignals.Instance.onGetPayedAmmoWorkerData += OnGetAmmoWorkerAreaPayedAmount;
            DataTransferSignals.Instance.onGetPayedMoneyWorkerData += OnGetMoneyWorkerAreaPayedAmount; 
            DataTransferSignals.Instance.onAmmoWorkerAreaBuyedItems += OnSetPayedAmmoWorkerAreaData;
            DataTransferSignals.Instance.onMoneyWorkerAreaBuyedItems += OnSetPayedMoneyWorkerAreaData;
            SaveSignals.Instance.onSaveSupporterData += OnGetSupporterData;
        }

        private void UnsubscribeEvents()
        {
            DataTransferSignals.Instance.onGetAmmoWorkerData -= OnGetAmmoWorkerData;
            DataTransferSignals.Instance.onGetMoneyWorkerData -= OnGetMoneyWorkerData;
            DataTransferSignals.Instance.onGetPayedAmmoWorkerData -= OnGetAmmoWorkerAreaPayedAmount;
            DataTransferSignals.Instance.onGetPayedMoneyWorkerData -= OnGetMoneyWorkerAreaPayedAmount;
            DataTransferSignals.Instance.onAmmoWorkerAreaBuyedItems -= OnSetPayedAmmoWorkerAreaData;
            DataTransferSignals.Instance.onMoneyWorkerAreaBuyedItems -= OnSetPayedMoneyWorkerAreaData;
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
            _ammoWorkerPayedAmount = _payedAmounts.AmmoWorkerPayedAmount;
            _moneyWorkerPayedAmount = _payedAmounts.MoneyWorkerPayedAmount;
        }
        
        private AmmoWorkerBuyData OnGetAmmoWorkerData() => _data.AmmoWorkerBuyData;
        private MoneyWorkerBuyData OnGetMoneyWorkerData() => _data.MoneyWorkerBuyData;

        private int OnGetAmmoWorkerAreaPayedAmount()
        {
            return _payedAmounts.AmmoWorkerPayedAmount;
        }
        
        private int OnGetMoneyWorkerAreaPayedAmount()
        {
            return _payedAmounts.MoneyWorkerPayedAmount;
        }
        
        private void OnSetPayedAmmoWorkerAreaData(int payedAmount)
        {
            _ammoWorkerPayedAmount = payedAmount;
            SupportAreaDataSave();
        }
        
        private void OnSetPayedMoneyWorkerAreaData(int payedAmount)
        {
            _moneyWorkerPayedAmount = payedAmount;
            SupportAreaDataSave();
        }

        private void SupportAreaDataSave()
        {
            _payedAmounts = new SupporterBuyableDataParams()
            {
                AmmoWorkerPayedAmount = _ammoWorkerPayedAmount,
                MoneyWorkerPayedAmount = _moneyWorkerPayedAmount
            };
            SaveSignals.Instance.onSupporterDataSave?.Invoke();
            SaveSignals.Instance.onScoreSave?.Invoke();
        }

        private SupporterBuyableDataParams OnGetSupporterData() => _payedAmounts;
    }
}