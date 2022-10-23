using System.Collections.Generic;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Keys;
using Signals;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class BaseManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables
        

        #endregion

        #region Serializefield Variables

        [SerializeField] private GameObject baseCenter;
        [SerializeField] private GameObject backDoor;
        [SerializeField] private BaseStage baseStage;
        [SerializeField] private TextMeshPro tmp;
        [SerializeField] private List<BaseObjects> level = new List<BaseObjects>();
        [SerializeField] private GameObject wareHouse;


        #endregion

        #region Private Variables

        private AreaDataParams _areaData;
        [ShowInInspector]private Dictionary<RoomNameEnum,int> _payedRoomDatas;
        [ShowInInspector]private Dictionary<TurretNameEnum,int> _payedTurretDatas;
        [ShowInInspector]private int _baseLevel = 0;
        public BaseData _data;
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
            DataTransferSignals.Instance.onBaseAreaBuyedItem += OnSetPayedRoomData;
            DataTransferSignals.Instance.onTurretAreaBuyedItem += OnSetPayedTurretData;
            DataTransferSignals.Instance.onRoomData += OnGetRoomData;
            DataTransferSignals.Instance.onTurretData += OnGetTurretData;
            DataTransferSignals.Instance.onPayedRoomData += OnGetRoomPayedAmound;
            DataTransferSignals.Instance.onPayedTurretData += OnGetTurretPayedAmound;
            DataTransferSignals.Instance.onGetMineAreaData += OnGetMineAreaData;
            DataTransferSignals.Instance.onGetSoldierAreaData += OnGetSoldierAreaData;
            IdleSignals.Instance.onGetWarHousePositon += OnGetWareHousePosition;
            SaveSignals.Instance.onSaveAreaData += OnGetAreaDatas;
            WorkerSignals.Instance.onGetBaseCenter += OnGetBaseCenter;
        }

        private void UnsubscribeEvents()
        {
            DataTransferSignals.Instance.onBaseAreaBuyedItem -= OnSetPayedRoomData;
            DataTransferSignals.Instance.onTurretAreaBuyedItem -= OnSetPayedTurretData;
            DataTransferSignals.Instance.onRoomData -= OnGetRoomData;
            DataTransferSignals.Instance.onTurretData -= OnGetTurretData;
            DataTransferSignals.Instance.onPayedRoomData -= OnGetRoomPayedAmound;
            DataTransferSignals.Instance.onPayedTurretData -= OnGetTurretPayedAmound;
            DataTransferSignals.Instance.onGetMineAreaData -= OnGetMineAreaData;
            DataTransferSignals.Instance.onGetSoldierAreaData -= OnGetSoldierAreaData;
            IdleSignals.Instance.onGetWarHousePositon -= OnGetWareHousePosition;
            SaveSignals.Instance.onSaveAreaData -= OnGetAreaDatas;
            WorkerSignals.Instance.onGetBaseCenter -= OnGetBaseCenter;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        #endregion

        private void Start()
        {
            DataTransferSignals.Instance.onGettedBaseData?.Invoke();
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
            _data = Resources.Load<CD_Level>("Data/CD_Level").LevelDatas[_baseLevel].BaseData;
            _payedRoomDatas = SaveSignals.Instance.onLoadAreaData().RoomPayedAmound;
            _payedTurretDatas = SaveSignals.Instance.onLoadAreaData().RoomTurretPayedAmound;
            DataTransferSignals.Instance.onGettedBaseData?.Invoke();
            SetBaseLevelText();
        }

        private RoomData OnGetRoomData(RoomNameEnum roomName) =>  _data.BaseRoomDatas.Rooms[(int)roomName];

        private BuyableTurretData OnGetTurretData(TurretNameEnum turret) => _data.BaseRoomDatas.Rooms[(int)turret].buyableTurretData;

        private MineAreaData OnGetMineAreaData() => _data.MineAreaData;
        
        private SoldierAreaData OnGetSoldierAreaData() => _data.SoldierAreaData;

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
        
        private Transform OnGetWareHousePosition() => wareHouse.transform;

        private GameObject OnGetBaseCenter() => baseCenter;

        private void SetBaseLevelText()
        {
            tmp.text = "Base " + (SaveSignals.Instance.onLoadCurrentLevel() + 1).ToString();
        }
    }
}
