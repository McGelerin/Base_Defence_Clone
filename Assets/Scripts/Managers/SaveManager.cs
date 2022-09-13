using System;
using System.Collections.Generic;
using Enums;
using Keys;
using Signals;
using UnityEngine;

namespace Managers
{
    public class SaveManager : MonoBehaviour
    {
        #region Self Variables
        #region Private Variables

        private int _levelCache;
        private ScoreDataParams _scoreDataCache;
        private AreaDataParams _areaDataCache;
        
        #endregion

        #endregion
        
        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            SaveSignals.Instance.onLevelSave += OnLevelSave;
            SaveSignals.Instance.onScoreSave += OnScoreSave;
            SaveSignals.Instance.onAreaDataSave += OnAreaDataSave;
            SaveSignals.Instance.onLoadCurrentLevel += OnLevelLoad;
            SaveSignals.Instance.onLoadScoreData += OnLoadScoreData;
            SaveSignals.Instance.onLoadAreaData += OnLoadAreaData;
        }


        private void UnsubscribeEvents()
        {
            SaveSignals.Instance.onLevelSave -= OnLevelSave;
            SaveSignals.Instance.onScoreSave -= OnScoreSave;
            SaveSignals.Instance.onAreaDataSave -= OnAreaDataSave;
            SaveSignals.Instance.onLoadCurrentLevel -= OnLevelLoad;
            SaveSignals.Instance.onLoadScoreData -= OnLoadScoreData;
            SaveSignals.Instance.onLoadAreaData -= OnLoadAreaData;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion
        
        private void OnLevelSave()
        {
            _levelCache = SaveSignals.Instance.onSaveLevelData();
            if (_levelCache != 0) ES3.Save("Level", _levelCache,"Level.es3");
        }

        private void OnScoreSave()
        {
            _scoreDataCache = new ScoreDataParams()
            {
                MoneyScore = SaveSignals.Instance.onSaveScoreData().MoneyScore,
                GemScore = SaveSignals.Instance.onSaveScoreData().GemScore
            };
            if (_scoreDataCache.MoneyScore != 0) ES3.Save("MoneyScore", _scoreDataCache.MoneyScore,"ScoreData.es3");
            if (_scoreDataCache.GemScore != 0) ES3.Save("GemScore", _scoreDataCache.GemScore,"ScoreData.es3");
        }

        private void OnAreaDataSave()
        {
            _areaDataCache = new AreaDataParams()
            {
                RoomPayedAmound = SaveSignals.Instance.onSaveAreaData().RoomPayedAmound,
                RoomTurretPayedAmound = SaveSignals.Instance.onSaveAreaData().RoomTurretPayedAmound
            };
            if (_areaDataCache.RoomPayedAmound != null) ES3.Save("RoomPayedAmound",
                _areaDataCache.RoomPayedAmound,"AreaData.es3");
            if (_areaDataCache.RoomTurretPayedAmound != null) ES3.Save("RoomTurretPayedAmound",
                _areaDataCache.RoomTurretPayedAmound,"AreaData.es3");
        }

        private int OnLevelLoad()
        {
            return ES3.KeyExists("Level", "Level.es3") 
                ? ES3.Load<int>("Level", "Level.es3") 
                : 0;
        }

        private ScoreDataParams OnLoadScoreData()
        {
            return new ScoreDataParams
            {
                MoneyScore = ES3.KeyExists("MoneyScore", "ScoreData.es3")
                    ? ES3.Load<int>("MoneyScore", "ScoreData.es3")
                    : 1000,
                GemScore = ES3.KeyExists("GemScore", "ScoreData.es3")
                    ? ES3.Load<int>("GemScore", "ScoreData.es3")
                    : 1000
            };
        }

        private AreaDataParams OnLoadAreaData()
        {
            return new AreaDataParams
            {
                RoomPayedAmound = ES3.KeyExists("RoomPayedAmound", "AreaData.es3")
                    ? ES3.Load < Dictionary<RoomNameEnum, int>>("RoomPayedAmound", "AreaData.es3")
                    : new Dictionary<RoomNameEnum, int>(),
                RoomTurretPayedAmound = ES3.KeyExists("RoomTurretPayedAmound", "AreaData.es3")
                    ? ES3.Load < Dictionary<RoomNameEnum, int>>("RoomTurretPayedAmound", "AreaData.es3")
                    : new Dictionary<RoomNameEnum, int>()
            };
        }
    }
}