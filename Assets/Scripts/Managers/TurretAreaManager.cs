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
    public class TurretAreaManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables
        
        public int PayedAmound
        {
            get => _payedAmound;
            set
            {
                _payedAmound = value;
                _remainingAmound = _turretData.Cost - _payedAmound;
                if (_remainingAmound <=0)
                {
                    Debug.Log("turret bot alındı");
                    turretManager.HasSolder();
                    _textParentGameObject.SetActive(false);
                }
                else
                {
                    SetText(_remainingAmound);
                }
            }
        }

        #endregion

        #region SerializeField Variables

        [SerializeField] private TurretNameEnum turretName;
        [SerializeField] private TextMeshPro tmp;
        [SerializeField] private TurretManager turretManager;

        #endregion
        
        #region Private Variables

        private TurretData _turretData;
        private int _payedAmound;
        private int _remainingAmound;
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
            //SubscribeEvents();
            OnSetData();
            turretManager.SetData(_turretData);
        }

        private void SubscribeEvents()
        {
            //dleSignals.Instance.onGettedBaseData += OnSetData;
        }

        private void UnsubscribeEvents()
        {
            //IdleSignals.Instance.onGettedBaseData -= OnSetData;
        }

        private void OnDisable()
        {
            //UnsubscribeEvents();
        }
        
        #endregion
        private void OnSetData()
        {
            _turretData = IdleSignals.Instance.onTurretData(turretName);
            PayedAmound = IdleSignals.Instance.onPayedTurretData(turretName);
            BuyAreaImageChange();
        }
        
        public void BuyAreaEnter()
        {
            _scoreCache = ScoreSignals.Instance.onScoreData();
            switch (_turretData.PayType)
            {
                case PayTypeEnum.Money:
                    if (_scoreCache.MoneyScore >= _remainingAmound)
                    {
                        StartCoroutine(Buy());
                    }
                    break;
                case PayTypeEnum.Gem :
                    if (_scoreCache.GemScore >= _remainingAmound)
                    {
                        StartCoroutine(Buy());
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void BuyAreaExit()
        {
            StopAllCoroutines();
            IdleSignals.Instance.onTurretAreaBuyedItem?.Invoke(turretName,_payedAmound);
        }

        private IEnumerator Buy()
        {
            var waitForSecond = new WaitForSeconds(0.02f);
            while (_remainingAmound > 0)
            {
                PayedAmound += 10;
                ScoreSignals.Instance.onSetScore?.Invoke(_turretData.PayType, -10);
                yield return waitForSecond;
            }
            IdleSignals.Instance.onTurretAreaBuyedItem?.Invoke(turretName,_payedAmound);
        }

        private void SetText(int remainingAmound)
        {
            tmp.text = remainingAmound.ToString();
        }

        private void BuyAreaImageChange()
        {
            _textParentGameObject.transform.GetChild(((int)_turretData.PayType) + 1).gameObject.SetActive(false);
        }
        
    }
}