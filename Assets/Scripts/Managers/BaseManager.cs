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


        #endregion

        #region Private Variables

        private AreaDataParams _areaData;
        [ShowInInspector]private Dictionary<RoomNameEnum,int> _payedRoomDatas;
        [ShowInInspector]private Dictionary<RoomNameEnum,int> _payedTurretDatas;
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
            IdleSignals.Instance.onRoomData += OnGetRoomData;
            IdleSignals.Instance.onPayedRoomData += OnGetRoomPayedAmound;
            SaveSignals.Instance.onSaveAreaData += OnGetAreaDatas;
        }

        private void UnsubscribeEvents()
        {
            IdleSignals.Instance.onBaseAreaBuyedItem -= OnSetAreaDatas;
            IdleSignals.Instance.onRoomData -= OnGetRoomData;
            IdleSignals.Instance.onPayedRoomData -= OnGetRoomPayedAmound;
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
            GetReferances();
        }

        private void GetReferances()
        {
            _baseLevel = LevelSignals.Instance.onGetLevelID();
            Data = Resources.Load<CD_Level>("Data/CD_Level").LevelDatas[_baseLevel].BaseData;
            _payedRoomDatas = SaveSignals.Instance.onLoadAreaData().RoomPayedAmound;
            _payedTurretDatas = SaveSignals.Instance.onLoadAreaData().RoomTurretPayedAmound;
            IdleSignals.Instance.onGettedBaseData?.Invoke();
            SetBaseLevelText();
        }

        private RoomData OnGetRoomData(RoomNameEnum roomName)
        {
            return Data.BaseRoomDatas.Rooms[(int)roomName];
        }

        private int OnGetRoomPayedAmound(RoomNameEnum room)
        {
            if (!_payedRoomDatas.ContainsKey(room)) _payedRoomDatas.Add(room, 0);
            return _payedRoomDatas[room];
        }

        private void OnSetAreaDatas(RoomNameEnum room,int payedAmound)
        {
            _payedRoomDatas[room] = payedAmound;
            _areaData = new AreaDataParams
            {
                RoomPayedAmound = _payedRoomDatas
            };
            SaveSignals.Instance.onAreaDataSave?.Invoke();
            SaveSignals.Instance.onScoreSave?.Invoke();
        }
        
        private AreaDataParams OnGetAreaDatas()
        {
            return _areaData;
        }

        private void SetBaseLevelText()
        {
            tmp.text = "Base " + (SaveSignals.Instance.onLoadCurrentLevel() + 1).ToString();
        }
    }
}
