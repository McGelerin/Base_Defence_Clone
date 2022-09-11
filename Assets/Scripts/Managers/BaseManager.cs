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
using UnityEngine;

namespace Managers
{
    public class BaseManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables
        public BaseStage BaseStageStatus
        {
            get => baseStage; 
            set
            {
                baseStage = value;
            }
        }
        
        [Header("Data")] public BaseData Data;

        #endregion

        #region Serializefield Variables

        [SerializeField] private GameObject backDoor;
        [SerializeField] private BaseStage baseStage;
        [SerializeField] private List<BaseObjects> level = new List<BaseObjects>();
        [SerializeField] private List<RoomManager> Rooms = new List<RoomManager>();


        #endregion

        #region Private Variables

        [ShowInInspector]private AreaDataParams _areaDatas;
        private int _baseLevel;
        
        
        #endregion
        #endregion

        private void Awake()
        {
            BaseStageStatus = baseStage;
            ColoseGameObjects();
        }

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
        }

        private void UnsubscribeEvents()
        {
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        
        #endregion
        
        private void ColoseGameObjects()
        {
            foreach (var VARIABLE in level[(int)BaseStageStatus].GameObjects)
            {
                VARIABLE.SetActive(false);
            }
        }
        
        private void Start()
        {
            _baseLevel = LevelSignals.Instance.onGetLevelID();
            _areaDatas = SaveSignals.Instance.onLoadAreaData();
            Data = Resources.Load<CD_Level>("Data/CD_Level").LevelDatas[_baseLevel].BaseData;
            EmptyListChack();
            SetRoomData();
        }

        private void EmptyListChack()
        {
            if (!_areaDatas.RoomPayedAmound.IsNullOrEmpty()) return;
            _areaDatas.RoomPayedAmound = new List<int>(new int[Data.BaseRoomDatas.Rooms.Count]);
        }

        private void SetRoomData()
        {
            for (int i = 0; i < Rooms.Count; i++)
            {
                Rooms[i].SetRoomData(Data.BaseRoomDatas.Rooms[i],_areaDatas.RoomPayedAmound[i]);
            }
        }
    }
}
