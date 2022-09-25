using System.Collections.Generic;
using System.Linq;
using Data.UnityObject;
using Data.ValueObject;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class OutSideManager : MonoBehaviour
    {
        #region Self Variables

        #region SerializeField Variables

        [SerializeField] private List<GameObject> spawnPoints;
        [SerializeField] private List<GameObject> turretPoints;
        [SerializeField] private int spawnTimer;
        
        #endregion

        #region Private Variables

        [ShowInInspector]private Dictionary<GameObject, List<SpawnData>> _turretsSpawnDatas = new Dictionary<GameObject, List<SpawnData>>();
        private List<GameObject> _moneyGameObjects;
        private List<SpawnData> _spawnDatasCache;
        private FrontYardData _data;
        private SpawnData _randomSpawnDataCache;
        private int _currentLevel;
        private int _randomTurretPoint;
        private float timer = 0;

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
        }

        private void UnsubscribeEvents()
        {
            IdleSignals.Instance.onEnemyTarget -= OnGetTarget;
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
            timer += Time.deltaTime;
            if (timer >= spawnTimer)
            {
                if (CurrentEnemyCheck())
                {
                    Spawn();
                    timer = 0;
                }
            }
        }

        private void InitSpawnDictionary()
        {
            foreach (var VARIABLE in turretPoints)
            {
                foreach (var spawnData in _data.SpawnDatas)
                {
                    _spawnDatasCache = new List<SpawnData>(new SpawnData[]
                    {
                        new SpawnData
                        {
                            EnemyType = spawnData.EnemyType,
                            EnemyCount = spawnData.EnemyCount,
                            CurrentCount = spawnData.CurrentCount
                        }
                    });
                }
                _turretsSpawnDatas.Add(VARIABLE,_spawnDatasCache);
            }
        }

        private bool CurrentEnemyCheck()
        {
            return _turretsSpawnDatas.Values.Any(SpawnDatas=> SpawnDatas.Any(spawnData => spawnData.CurrentCount < spawnData.EnemyCount));
        }

        private void Spawn()
        {
            RandomEnemy();
            PoolSignals.Instance.onGetPoolObject(_randomSpawnDataCache.EnemyType.ToString(),
                spawnPoints[Random.Range(0, spawnPoints.Count)].transform);
        }

        private void RandomEnemy()
        {
            while (true)
            {
                _randomTurretPoints = Random.Range(0, turretPoints.Count);
                _randomSpawnDatas = Random.Range(0, _spawnDatasCache.Count);
                _randomSpawnDataCache = _turretsSpawnDatas[turretPoints[_randomTurretPoints]][_randomSpawnDatas];
                if (_randomSpawnDataCache.CurrentCount < _randomSpawnDataCache.EnemyCount)
                {
                    _turretsSpawnDatas[turretPoints[_randomTurretPoints]][_randomSpawnDatas].CurrentCount++;
                    break;
                }
            }
        }

        private GameObject OnGetTarget()
        {
            return turretPoints[_randomTurretPoints];
        }
    }
}