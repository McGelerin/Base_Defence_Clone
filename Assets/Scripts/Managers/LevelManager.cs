using Command;
using Data.UnityObject;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        [Header("Data")] public int Data;

        #endregion

        #region Serialized Variables

        [SerializeField] private GameObject levelHolder;

        #endregion

        #region Private Variables

        private LevelLoaderCommand _levelLoader;
        private ClearActiveLevelCommand _levelClearer;
        [ShowInInspector] private int _levelID;

        #endregion

        #endregion

        private void Awake()
        {
            _levelClearer = new ClearActiveLevelCommand();
            _levelLoader = new LevelLoaderCommand();
        }
        
        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            LevelSignals.Instance.onLevelInitialize += OnInitializeLevel;
            LevelSignals.Instance.onClearActiveLevel += OnClearActiveLevel;
            LevelSignals.Instance.onNextLevel += OnNextLevel;
            LevelSignals.Instance.onRestartLevel += OnRestartLevel;
        }

        private void UnsubscribeEvents()
        {
            LevelSignals.Instance.onLevelInitialize -= OnInitializeLevel;
            LevelSignals.Instance.onClearActiveLevel -= OnClearActiveLevel;
            LevelSignals.Instance.onNextLevel -= OnNextLevel;
            LevelSignals.Instance.onRestartLevel -= OnRestartLevel;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void Start()
        {
            _levelID = SaveSignals.Instance.onLoadCurrentLevel();
            OnInitializeLevel();
            //SetLevelText();
        }

        private void OnNextLevel()
        {
            _levelID++;
            LevelSignals.Instance.onClearActiveLevel?.Invoke();
            CoreGameSignals.Instance.onReset?.Invoke();
            LevelSignals.Instance.onLevelInitialize?.Invoke();
            //SetLevelText();
        }

        private void OnRestartLevel()
        {
            LevelSignals.Instance.onClearActiveLevel?.Invoke();
            CoreGameSignals.Instance.onReset?.Invoke();
            LevelSignals.Instance.onLevelInitialize?.Invoke();
        }
        
        private int OnGetLevelID()
        {
            return _levelID;
        }

        // private void SetLevelText()
        // {
        //     UISignals.Instance.onSetLevelText?.Invoke(_levelID);
        // }
        
        private void OnInitializeLevel()
        {
            int newLevelData = GetLevelCount();
            _levelLoader.InitializeLevel(newLevelData, levelHolder.transform);
        }
        
        private int GetLevelCount()
        {
            return _levelID % Resources.Load<CD_Level>("Data/CD_Level").LevelDatas.Count;
        }
        
        private void OnClearActiveLevel()
        {
            _levelClearer.ClearActiveLevel(levelHolder.transform);
        }
    }
}