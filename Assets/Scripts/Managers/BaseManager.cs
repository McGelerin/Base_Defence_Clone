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
        [ShowInInspector]private Dictionary<TurretNameEnum,int> _payedTurretDatas;
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
            GetReferences();
        }

        private void SubscribeEvents()
        {
            IdleSignals.Instance.onBaseAreaBuyedItem += OnSetPayedRoomData;
            IdleSignals.Instance.onTurretAreaBuyedItem += OnSetPayedTurretData;
            IdleSignals.Instance.onRoomData += OnGetRoomData;
            IdleSignals.Instance.onTurretData += OnGetTurretData;
            IdleSignals.Instance.onPayedRoomData += OnGetRoomPayedAmound;
            IdleSignals.Instance.onPayedTurretData += OnGetTurretPayedAmound;
            IdleSignals.Instance.onGetMineAreaData += OnGetMineAreaData;
            SaveSignals.Instance.onSaveAreaData += OnGetAreaDatas;
        }

        private void UnsubscribeEvents()
        {
            IdleSignals.Instance.onBaseAreaBuyedItem -= OnSetPayedRoomData;
            IdleSignals.Instance.onTurretAreaBuyedItem -= OnSetPayedTurretData;
            IdleSignals.Instance.onRoomData -= OnGetRoomData;
            IdleSignals.Instance.onTurretData -= OnGetTurretData;
            IdleSignals.Instance.onPayedRoomData -= OnGetRoomPayedAmound;
            IdleSignals.Instance.onPayedTurretData -= OnGetTurretPayedAmound;
            IdleSignals.Instance.onGetMineAreaData -= OnGetMineAreaData;
            SaveSignals.Instance.onSaveAreaData -= OnGetAreaDatas;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        #endregion

        private void Start()
        {
            IdleSignals.Instance.onGettedBaseData?.Invoke();
        }

        private void ColoseGameObjects()
        {
            foreach (var VARIABLE in level[(int)baseStage].GameObjects)
            {
                VARIABLE.SetActive(false);
            }
        }

        private void GetReferences()
        {
            _baseLevel = LevelSignals.Instance.onGetLevelID();
            Data = Resources.Load<CD_Level>("Data/CD_Level").LevelDatas[_baseLevel].BaseData;
            _payedRoomDatas = SaveSignals.Instance.onLoadAreaData().RoomPayedAmound;
            _payedTurretDatas = SaveSignals.Instance.onLoadAreaData().RoomTurretPayedAmound;
            IdleSignals.Instance.onGettedBaseData?.Invoke();
            SetBaseLevelText();
        }

        private RoomData OnGetRoomData(RoomNameEnum roomName) =>  Data.BaseRoomDatas.Rooms[(int)roomName];

        
        private TurretData OnGetTurretData(TurretNameEnum turret) => Data.BaseRoomDatas.Rooms[(int)turret].TurretData;

        private MineAreaData OnGetMineAreaData() => Data.MineAreaData;

        private int OnGetRoomPayedAmound(RoomNameEnum room)
        {
            if (!_payedRoomDatas.ContainsKey(room)) _payedRoomDatas.Add(room, 0);
            return _payedRoomDatas[room];
        }
        
        private int OnGetTurretPayedAmound(TurretNameEnum turret)
        {
            if (!_payedTurretDatas.ContainsKey(turret)) _payedTurretDatas.Add(turret, 0);
            return _payedTurretDatas[turret];
        }

        private void OnSetPayedRoomData(RoomNameEnum room,int payedAmound)
        {
            _payedRoomDatas[room] = payedAmound;
            AreaDataSave();
        }

        private void OnSetPayedTurretData(TurretNameEnum turret,int payedAmound)
        {
            _payedTurretDatas[turret] = payedAmound;
            AreaDataSave();
        }

        private void AreaDataSave()
        {
            _areaData = new AreaDataParams
            {
                RoomPayedAmound = _payedRoomDatas,
                RoomTurretPayedAmound = _payedTurretDatas
            };
            SaveSignals.Instance.onAreaDataSave?.Invoke();
            SaveSignals.Instance.onScoreSave?.Invoke();
        }
        
        private AreaDataParams OnGetAreaDatas() => _areaData;

        private void SetBaseLevelText()
        {
            tmp.text = "Base " + (SaveSignals.Instance.onLoadCurrentLevel() + 1).ToString();
        }
    }
}
