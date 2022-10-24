using System.Collections.Generic;
using System.Linq;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class OutSideSpawnManager : MonoBehaviour
    {
        #region Self Variables

        #region SerializeField Variables

        [SerializeField] private List<GameObject> enemySpawnPoints;
        [SerializeField] private List<GameObject> hostageSpawnPoint;
        [SerializeField] private List<GameObject> turretPoints;
        [SerializeField] private int spawnTimer;

        #endregion

        #region Private Variables

        [ShowInInspector]private Dictionary<GameObject, List<SpawnData>> _turretsSpawnDatas =
            new Dictionary<GameObject, List<SpawnData>>();

        private List<GameObject> _hostageSpawnControlList = new List<GameObject>();
        private List<SpawnData> _spawnDatasCache;
        private SpawnData _spawnDataCache;
        private FrontYardData _data;
        private SpawnData _randomSpawnDataCache;
        private int _currentLevel;
        private int _randomTurretPoint;
        private int _hostageCache = 0;
        private float _enemyTimer = 0;
        private float _hostageTimer = 0;

        #region Random Variables

        private int _randomTurretPoints;
        private int _randomSpawnDatas;

        #endregion

        #endregion

        #endregion

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
            _currentLevel = LevelSignals.Instance.onGetLevelID();
            _data = Resources.Load<CD_Level>("Data/CD_Level").LevelDatas[_currentLevel].FrontYardData;
        }

        private void SubscribeEvents()
        {
            IdleSignals.Instance.onEnemyTarget += OnGetTarget;
            IdleSignals.Instance.onHostageCollected += OnHostageRemoveList;
            IdleSignals.Instance.onEnemyDead += OnEnemyRemoveDic;
        }

        private void UnsubscribeEvents()
        {
            IdleSignals.Instance.onEnemyTarget -= OnGetTarget;
            IdleSignals.Instance.onHostageCollected -= OnHostageRemoveList;
            IdleSignals.Instance.onEnemyDead -= OnEnemyRemoveDic;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void Start()
        {
            InitSpawnDictionary();
        }

        private void Update()
        {
            _enemyTimer += Time.deltaTime;
            _hostageTimer += Time.deltaTime;
            if (_enemyTimer >= spawnTimer)
            {
                if (CurrentEnemyCheck())
                {
                    EnemySpawn();
                    _enemyTimer = 0;
                }
            }

            if (_hostageTimer >= spawnTimer)
            {
                if (CurrentHostageCheck())
                {
                    HostageSpawn();
                    _hostageTimer = 0;
                }
            }
        }

        private void InitSpawnDictionary()
        {
            EnemyDict();
            _hostageSpawnControlList = new List<GameObject>(new GameObject[hostageSpawnPoint.Count]);
        }

        private void EnemyDict()
        {
            foreach (var VARIABLE in turretPoints)
            {
                _spawnDatasCache = new List<SpawnData>();
                foreach (var spawnData in _data.SpawnDatas)
                {
                    _spawnDataCache = new SpawnData
                    {
                        EnemyType = spawnData.EnemyType,
                        EnemyCount = spawnData.EnemyCount,
                        CurrentCount = spawnData.CurrentCount
                    };
                    _spawnDatasCache.Add(_spawnDataCache);
                }

                _turretsSpawnDatas.Add(VARIABLE, _spawnDatasCache);
            }
        }

        private bool CurrentEnemyCheck() => _turretsSpawnDatas.Values.Any(SpawnDatas =>
            SpawnDatas.Any(spawnData => spawnData.CurrentCount < spawnData.EnemyCount));

        private bool CurrentHostageCheck() => _hostageSpawnControlList.Any(obj => obj == null);

        private void EnemySpawn()
        {
            RandomEnemy();
            PoolSignals.Instance.onGetPoolObject(_randomSpawnDataCache.EnemyType.ToString(),
                enemySpawnPoints[Random.Range(0, enemySpawnPoints.Count)].transform);
        }

        private void HostageSpawn()
        {
            if (_hostageSpawnControlList[_hostageCache] == null)
            {
                _hostageSpawnControlList[_hostageCache] =
                    PoolSignals.Instance.onGetPoolObject(PoolType.Hostage.ToString(),
                        hostageSpawnPoint[_hostageCache].transform);
                return;
            }
            if (_hostageCache == hostageSpawnPoint.Count - 1)
            {
                _hostageCache = 0;
                return;
            }
            _hostageCache++;
        }

        private void RandomEnemy()
        {
            while (true)
            {
                _randomTurretPoints = Random.Range(0, turretPoints.Count);
                _randomSpawnDatas = Random.Range(0, _spawnDatasCache.Count);
                _randomSpawnDataCache = _turretsSpawnDatas[turretPoints[_randomTurretPoints]][_randomSpawnDatas];
                if (_randomSpawnDataCache.CurrentCount >= _randomSpawnDataCache.EnemyCount) continue;
                _turretsSpawnDatas[turretPoints[_randomTurretPoints]][_randomSpawnDatas].CurrentCount++;
                break;
            }
        }

        private void OnEnemyRemoveDic(GameObject enemyTarget, EnemyType type)
        {
            _spawnDatasCache = _turretsSpawnDatas[enemyTarget];
            _spawnDatasCache[(int)type].CurrentCount--;
        }

        private void OnHostageRemoveList(GameObject hostage)
        {
            _hostageSpawnControlList[_hostageSpawnControlList.IndexOf(hostage)] = null;
        }

        private GameObject OnGetTarget() => turretPoints[_randomTurretPoints];
    }
}