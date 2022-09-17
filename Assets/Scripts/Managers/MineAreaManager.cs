﻿using System.Collections.Generic;
using System.Linq;
using Data.ValueObject;
using DG.Tweening;
using Enums;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class MineAreaManager : MonoBehaviour
    {
        #region Self Variables

        #region SerializeField Variables

        [SerializeField] private GameObject gemAreaHolder;
        [SerializeField] private List<GameObject> mines = new List<GameObject>();

        #endregion

        #region Private Variables

        private MineAreaData _data;
        private List<int> _capacity;
        [ShowInInspector]private List<GameObject>_gemHolderGameObjects = new List<GameObject>();
        [ShowInInspector]private List<GameObject>_gemHolderGameObjectsCache;
        private int _random;
        private int _squareMeters;
        private Vector3 _direct = Vector3.zero;


        #endregion

        #endregion

        #region Event Subscriptions
        
        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            IdleSignals.Instance.onGetMineGameObject += OnGetMineGameObject;
            IdleSignals.Instance.onGemAreaHolder += OnGetGemAreaHolder;
            IdleSignals.Instance.onGemHolderAddGem += OnGemHolderAddGem;
        }

        private void UnsubscribeEvents()
        {
            IdleSignals.Instance.onGetMineGameObject -= OnGetMineGameObject;
            IdleSignals.Instance.onGemAreaHolder -= OnGetGemAreaHolder;
            IdleSignals.Instance.onGemHolderAddGem -= OnGemHolderAddGem;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void Start()
        {
            _capacity = new List<int>(new int [mines.Count]);
            _data = IdleSignals.Instance.onGetMineAreaData();
            _squareMeters =_data.GemHolderData.GemCoundX * _data.GemHolderData.GemCoundZ;
        }

        public void PlayerTriggerEnter(Transform other)
        {
            _gemHolderGameObjectsCache = new List<GameObject>(_gemHolderGameObjects);
            _gemHolderGameObjects.Clear();
            for (int i = 0; i < _gemHolderGameObjectsCache.Count; i++)
            {
                var random = new Vector3(Random.Range(0f,3f),Random.Range(0f,3f),Random.Range(0f,3f));
                var obj =_gemHolderGameObjectsCache[i];
                obj.transform.SetParent(other);
                obj.transform
                    .DOLocalMove(obj.transform.localPosition + random, 0.5f);
                obj.transform.DOLocalMove(Vector3.zero, 0.5f).SetDelay(0.5f).OnComplete(()=>
                {
                    PoolSignals.Instance.onReleasePoolObject?.Invoke(PoolType.Gem,obj);
                });
            }
            ScoreSignals.Instance.onSetScore?.Invoke(PayTypeEnum.Gem,_gemHolderGameObjectsCache.Count);
            _gemHolderGameObjectsCache.Clear();
            SaveSignals.Instance.onScoreSave?.Invoke();
        }

        private void OnGemHolderAddGem(Transform miner)
        {
            var position = miner.position;
            position = new Vector3(position.x, position.y + 1, position.z);
            miner.position = position;
            GameObject gem = PoolSignals.Instance.onGetPoolObject(PoolType.Gem, miner);
            SetGemPosition(gem);
            _gemHolderGameObjects.Add(gem);
        }

        private void SetGemPosition(GameObject gem)
        {
            _direct = _data.GemHolderData.GemInitPoint + gemAreaHolder.transform.position;
            _direct.x = _direct.x + (int)(_gemHolderGameObjects.Count % _data.GemHolderData.GemCoundX) / _data.GemHolderData.OffsetFactor;
            _direct.y = _direct.y + (int)(_gemHolderGameObjects.Count / _squareMeters) / _data.GemHolderData.OffsetFactor;;
            _direct.z = _direct.z - (int)((_gemHolderGameObjects.Count % _squareMeters) / _data.GemHolderData.GemCoundZ) / _data.GemHolderData.OffsetFactor;
            gem.transform.DOLocalMove(_direct,0.5f);
        }
        
        private GameObject OnGetMineGameObject()
        {
            while (true)
            {
                _random = Random.Range(0,_capacity.Count);
                if (_capacity[_random] != _data.MaxWorkerFromMine)
                {
                    _capacity[_random]++;
                    break;
                }
                if (_capacity.Any(c => c != _data.MaxWorkerFromMine)) continue;
                throw new System.NotImplementedException();
            }
            return mines[_random];
        }
        private GameObject OnGetGemAreaHolder() => gemAreaHolder;
    }
}