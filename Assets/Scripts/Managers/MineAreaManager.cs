using System.Collections.Generic;
using System.Linq;
using Data.ValueObject;
using DG.Tweening;
using Enums;
using Signals;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class MineAreaManager : MonoBehaviour
    {
        #region Self Variables

        #region SerializeField Variables

        [SerializeField] private TextMeshPro tmp;
        [SerializeField] private GameObject gemAreaHolder;
        [SerializeField] private List<GameObject> mines = new List<GameObject>();

        #endregion

        #region Private Variables

        private MineAreaData _data;
        private List<int> _capacity;
        [ShowInInspector]private List<GameObject>_gemHolderGameObjects = new List<GameObject>();
        private List<GameObject>_gemHolderGameObjectsCache;
        private int _random;
        private int _squareMeters;
        private Vector3 _direct = Vector3.zero;
        [ShowInInspector]private List<GameObject> _hostageGameObjects = new List<GameObject>();
        private int _currentMiner;


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
            _currentMiner = 0;
            _capacity = new List<int>(new int [mines.Count]);
            _data = IdleSignals.Instance.onGetMineAreaData();
            _squareMeters =_data.GemHolderData.GemCountX * _data.GemHolderData.GemCountZ;
            SetText();

        }

        public void PlayerTriggerEnter(Transform other)
        {
            _gemHolderGameObjectsCache = new List<GameObject>(_gemHolderGameObjects);
            _gemHolderGameObjects.Clear();
            for (int i = 0; i < _gemHolderGameObjectsCache.Count; i++)
            {
                var random = new Vector3(Random.Range(-3f,3f),Random.Range(0f,3f),Random.Range(-3f,3f));
                var obj =_gemHolderGameObjectsCache[i];
                obj.transform.SetParent(other);
                obj.transform
                    .DOLocalMove(obj.transform.localPosition + random, 0.5f);
                obj.transform.DOLocalMove(Vector3.zero, 0.5f).SetDelay(0.5f).OnComplete(()=>
                {
                    PoolSignals.Instance.onReleasePoolObject?.Invoke(PoolType.Gem.ToString(),obj);
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
            GameObject gem = PoolSignals.Instance.onGetPoolObject(PoolType.Gem.ToString(), miner);
            SetGemPosition(gem);
            _gemHolderGameObjects.Add(gem);
        }

        private void SetGemPosition(GameObject gem)
        {
            _direct = _data.GemHolderData.GemInitPoint + gemAreaHolder.transform.position;
            _direct.x = _direct.x + (int)(_gemHolderGameObjects.Count % _data.GemHolderData.GemCountX) / _data.GemHolderData.OffsetFactor;
            _direct.y = _direct.y + (int)(_gemHolderGameObjects.Count / _squareMeters) / _data.GemHolderData.OffsetFactor;;
            _direct.z = _direct.z - (int)((_gemHolderGameObjects.Count % _squareMeters) / _data.GemHolderData.GemCountZ) / _data.GemHolderData.OffsetFactor;
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

        public void PlayerEntryGemArea()
        {
            _hostageGameObjects = StackSignals.Instance.onGetHostageList();
            if (_hostageGameObjects.Count <= 0) return;
            while (_hostageGameObjects.Count<_data.MaxWorkerAmound)
            {
                if(_currentMiner == _data.MaxWorkerAmound) break;
                if (_hostageGameObjects.Count <= 0) break;
                GameObject miner = PoolSignals.Instance.onGetPoolObject(PoolType.Miner.ToString(), _hostageGameObjects.Last().transform);
                miner.transform.rotation = _hostageGameObjects.Last().transform.rotation;
                StackSignals.Instance.onLastGameObjectRemone?.Invoke();
                _hostageGameObjects = StackSignals.Instance.onGetHostageList();
                _currentMiner++;
                SetText();
            }
        }

        private void SetText()
        {
            tmp.SetText(_currentMiner.ToString() + " / " + _data.MaxWorkerAmound);
        }
    }
}