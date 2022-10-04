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
    public class RoomManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables
        
        public int PayedAmound
        {
            get => _payedAmound;
            set
            {
                _payedAmound = value;
                _remainingAmound = _roomData.Cost - _payedAmound;
                if (_remainingAmound ==0)
                {
                    _textParentGameObject.SetActive(false);
                    area.SetActive(true);
                    fencles.SetActive(false);
//                    invisibleWall.SetActive(false);
                }
                else
                {
                    SetText(_remainingAmound);
                }
            }
        }

        #endregion

        #region SerializeField Variables

        [SerializeField] private RoomNameEnum roomName;
        [SerializeField] private GameObject area;
        [SerializeField] private GameObject fencles;
        [SerializeField] private GameObject invisibleWall;
        [SerializeField] private TextMeshPro tmp;

        #endregion
        
        #region Private Variables

        [ShowInInspector]private RoomData _roomData;
        private bool _isBase;
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
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            IdleSignals.Instance.onGettedBaseData += OnSetData;
        }

        private void UnsubscribeEvents()
        {
            IdleSignals.Instance.onGettedBaseData -= OnSetData;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        
        #endregion
        private void OnSetData()
        {
            _roomData = IdleSignals.Instance.onRoomData(roomName);
            PayedAmound = IdleSignals.Instance.onPayedRoomData(roomName);
            BuyAreaImageChange();
        }
        
        public void BuyAreaEnter()
        {
            _scoreCache = ScoreSignals.Instance.onScoreData();
            switch (_roomData.PayType)
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
            IdleSignals.Instance.onBaseAreaBuyedItem?.Invoke(roomName,_payedAmound);
        }

        private IEnumerator Buy()
        {
            var waitForSecond = new WaitForSeconds(0.05f);
            while (_remainingAmound > 0)
            {
                PayedAmound += 10;
                ScoreSignals.Instance.onSetScore?.Invoke(_roomData.PayType, -10);
                yield return waitForSecond;
            }
            IdleSignals.Instance.onBaseAreaBuyedItem?.Invoke(roomName,_payedAmound);
        }

        private void SetText(int remainingAmount)
        {
            tmp.text = remainingAmount.ToString();
        }

        private void BuyAreaImageChange()
        {
            _textParentGameObject.transform.GetChild(((int)_roomData.PayType) + 1).gameObject.SetActive(false);
        }
    }
}