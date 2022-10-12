using System;
using System.Collections;
using Data.ValueObject;
using Enums;
using Keys;
using Signals;
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
                _remainingAmound = _buyableTurretData.Cost - _payedAmound;
                if (_remainingAmound <=0)
                {
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

        private BuyableTurretData _buyableTurretData;
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
            OnSetData();
        }
        #endregion
        
        private void OnSetData()
        {
            _buyableTurretData = IdleSignals.Instance.onTurretData(turretName);
            PayedAmound = IdleSignals.Instance.onPayedTurretData(turretName);
            BuyAreaImageChange();
        }
        
        //bu kısımları command yapabilirim
        public void BuyAreaEnter()
        {
            _scoreCache = ScoreSignals.Instance.onScoreData();
            switch (_buyableTurretData.PayType)
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
                ScoreSignals.Instance.onSetScore?.Invoke(_buyableTurretData.PayType, -10);
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
            _textParentGameObject.transform.GetChild(((int)_buyableTurretData.PayType) + 1).gameObject.SetActive(false);
        }
        
    }
}