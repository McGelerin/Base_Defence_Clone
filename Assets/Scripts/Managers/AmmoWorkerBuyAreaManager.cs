using System;
using System.Collections;
using Data.ValueObject;
using Enums;
using Keys;
using Signals;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class AmmoWorkerBuyAreaManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables
        
        public int PayedAmount
        {
            get => _payedAmount;
            set
            {
                _payedAmount = value;
                _remainingAmount = _data.Cost - _payedAmount;
                if (_remainingAmount <=0)
                {
                    if (_buyCoroutine != null)
                    {
                        StopCoroutine(_buyCoroutine);
                        _buyCoroutine = null;
                        DataTransferSignals.Instance.onAmmoWorkerAreaBuyedItems?.Invoke(_payedAmount);
                    }

                    var ammoWorker =PoolSignals.Instance.onGetPoolObject(PoolType.AmmoWorker.ToString(), botPosition.transform);
                    ammoWorker.transform.localRotation = botPosition.transform.localRotation;
                    gameObject.SetActive(false);
                }
                else
                {
                    SetText(_remainingAmount);
                }
            }
        }
        
        #endregion

        #region Serialized Variables

        [SerializeField] private GameObject botPosition;
        [SerializeField] private TextMeshPro tmp;
        
        
        #endregion

        #region Private Variables

        [ShowInInspector]private AmmoWorkerBuyData _data;
        [ShowInInspector]private int _payedAmount;
        private Coroutine _buyCoroutine;
        private int _remainingAmount;
        private ScoreDataParams _scoreCache;
        private GameObject _textParentGameObject;


        #endregion

        #endregion
        
        private void Awake()
        {
            _textParentGameObject = tmp.transform.parent.gameObject;
        }

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            DataTransferSignals.Instance.onGettedSupporterData += OnSetData;
        }

        private void UnsubscribeEvents()
        {
            DataTransferSignals.Instance.onGettedSupporterData -= OnSetData;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion
        
        private void OnSetData()
        {
            _data = DataTransferSignals.Instance.onGetAmmoWorkerData();
            PayedAmount = DataTransferSignals.Instance.onGetPayedAmmoWorkerData();
            BuyAreaImageChange();
        }
        
        public void BuyAreaEnter()
        {
            _scoreCache = ScoreSignals.Instance.onScoreData();
            switch (_data.PayType)
            {
                case PayTypeEnum.Money:
                    if (_scoreCache.MoneyScore >= _remainingAmount)
                    {
                        _buyCoroutine = StartCoroutine(Buy());
                    }
                    break;
                case PayTypeEnum.Gem :
                    if (_scoreCache.GemScore >= _remainingAmount)
                    {
                        _buyCoroutine = StartCoroutine(Buy());
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void BuyAreaExit()
        {
            if (_buyCoroutine == null) return;
            StopCoroutine(_buyCoroutine);
            _buyCoroutine = null;
            DataTransferSignals.Instance.onAmmoWorkerAreaBuyedItems?.Invoke(_payedAmount);
        }
        
        private IEnumerator Buy()
        {
            var waitForSecond = new WaitForSeconds(0.05f);
            while (_remainingAmount > 0)
            {
                PayedAmount += 10;
                ScoreSignals.Instance.onSetScore?.Invoke(_data.PayType, -10);
                yield return waitForSecond;
            }
            _buyCoroutine = null;
            DataTransferSignals.Instance.onAmmoWorkerAreaBuyedItems?.Invoke(_payedAmount);
        }
        
        private void SetText(int remainingAmount)
        {
            tmp.text = remainingAmount.ToString();
        }
        
        private void BuyAreaImageChange()
        {
            _textParentGameObject.transform.GetChild(((int)_data.PayType) + 1).gameObject.SetActive(false);
        }
    }
}