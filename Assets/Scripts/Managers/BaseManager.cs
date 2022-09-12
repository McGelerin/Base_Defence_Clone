using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Keys;
using Signals;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class BaseManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables
        
        [Header("Data")] public BaseData Data;

        #endregion

        #region Serializefield Variables

        [SerializeField] private GameObject backDoor;
        [SerializeField] private BaseStage baseStage;
        [SerializeField] private TextMeshPro tmp;
        [SerializeField] private List<BaseObjects> level = new List<BaseObjects>();
        [SerializeField] private List<RoomManager> Rooms = new List<RoomManager>();
        [SerializeField] private List<TurretManager> Turrets = new List<TurretManager>();


        #endregion

        #region Private Variables

        [ShowInInspector]private AreaDataParams _areaDatas;
        private int _baseLevel;
        
        
        #endregion
        #endregion

        private void Awake()
        {
            ColoseGameObjects();
        }

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            IdleSignals.Instance.onBaseAreaBuyedItem += OnSetAreaDatas;
            SaveSignals.Instance.onSaveAreaData += OnGetAreaDatas;
        }

        private void UnsubscribeEvents()
        {
            IdleSignals.Instance.onBaseAreaBuyedItem -= OnSetAreaDatas;
            SaveSignals.Instance.onSaveAreaData -= OnGetAreaDatas;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        
        #endregion
        
        private void ColoseGameObjects()
        {
            foreach (var VARIABLE in level[(int)baseStage].GameObjects)
            {
                VARIABLE.SetActive(false);
            }
        }
        
        private void Start()
        {
            _baseLevel = LevelSignals.Instance.onGetLevelID();
            _areaDatas = SaveSignals.Instance.onLoadAreaData();
            Data = Resources.Load<CD_Level>("Data/CD_Level").LevelDatas[_baseLevel].BaseData;
            SetBaseLevelText();
            EmptyListChack();
            SetRoomData();
        }

        private void EmptyListChack()
        {
            if (!_areaDatas.RoomPayedAmound.IsNullOrEmpty()) return;
            _areaDatas.RoomPayedAmound = new List<int>(new int[Data.BaseRoomDatas.Rooms.Count]);
            _areaDatas.RoomTurretPayedAmound = new List<int>(new int[Data.BaseRoomDatas.Rooms.Count]);
        }

        private void SetRoomData()
        {
            for (int i = 0; i < Rooms.Count; i++)
            {
                Rooms[i].SetRoomData(Data.BaseRoomDatas.Rooms[i],_areaDatas.RoomPayedAmound[i]);
                //Turrets[i].SetTurretData(Data.BaseRoomDatas.Rooms[i].TurretData,_areaDatas.RoomTurretPayedAmound[i]);
            }
        }

        private void OnSetAreaDatas()
        {
            for (int i = 0; i < Rooms.Count; i++)
            {
                _areaDatas.RoomPayedAmound[i] = Rooms[i].PayedAmound;
                Debug.Log(Rooms[i].PayedAmound);
                //_areaDatas.RoomTurretPayedAmound[i] = Turrets[i].PayedAmound;
            }
            SaveSignals.Instance.onAreaDataSave?.Invoke();
            SaveSignals.Instance.onScoreSave?.Invoke();
        }
        
        private AreaDataParams OnGetAreaDatas()
        {
            return _areaDatas;
        }

        private void SetBaseLevelText()
        {
            tmp.text = "Base " + (SaveSignals.Instance.onLoadCurrentLevel() + 1).ToString();
        }
    }
}
