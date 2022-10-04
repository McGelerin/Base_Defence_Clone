using System;
using Controller;
using Controllers;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Keys;
using Signals;
using UnityEngine;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        [Header("Data")] public PlayerData Data;
        
        #endregion

        #region Serialized Variables

        [Space] [SerializeField] private PlayerMovementController movementController;
        [SerializeField] private PlayerAnimationController animationController;
        //[SerializeField] private PlayerMeshController meshController;

        #endregion

        #region Private Variables

        private PlayerStateEnum _playerState;
        private PlayerAnimState _weaponAnimStateCache;
        
        #endregion
        
        #endregion
        
        private void Awake()
        {
            Data = GetPlayerData();
            _playerState = PlayerStateEnum.INSIDE;
            SendPlayerDataToControllers();
        }
        private PlayerData GetPlayerData() => Resources.Load<CD_PlayerData>("Data/CD_Player").Data;
        
        private void SendPlayerDataToControllers()
        {
            movementController.SetMovementData(Data.MovementData);
        }
        
        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            InputSignals.Instance.onJoystickDragged += OnSetIdleInputValues;
            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onReset += OnReset;
        }

        private void UnsubscribeEvents()
        {
            InputSignals.Instance.onJoystickDragged -= OnSetIdleInputValues;
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onReset -= OnReset;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void Start()
        {
            CoreGameSignals.Instance.onSetPlayerPosition?.Invoke(transform);
        }

        private void OnSetIdleInputValues(IdleInputParams inputParams)
        {
            switch (_playerState)
            {
                case PlayerStateEnum.INSIDE:
                    movementController.UpdateIdleInputValue(inputParams);
                    animationController.SetSpeedVariable(inputParams);
                    break;
                case PlayerStateEnum.OUTSIDE:
                    movementController.UpdateIdleInputValue(inputParams);
                    animationController.SetSpeedVariable(inputParams);
                    animationController.SetOutSideAnimState(inputParams,TargetPosition());
                    break;
                case PlayerStateEnum.TARET:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Transform TargetPosition()
        {
            return AttackSignals.Instance.onPlayerIsTarget();
        }

        public void SetPlayerState(PlayerStateEnum state)
        {
            _playerState = state;
            if (_playerState == PlayerStateEnum.INSIDE)
            {
                _weaponAnimStateCache = IdleSignals.Instance.onSelectedWeaponAnimState();
                animationController.SetBoolAnimState(PlayerAnimState.BaseState,true);
                animationController.SetBoolAnimState(_weaponAnimStateCache,false);
            }
            else if (_playerState == PlayerStateEnum.OUTSIDE)
            {
                animationController.SetBoolAnimState(PlayerAnimState.BaseState,false);
                animationController.SetBoolAnimState(_weaponAnimStateCache,true);
            }
        }

        private void OnPlay()
        {
            //  SetStackPosition();
            movementController.IsReadyToPlay(true);
            animationController.SetBoolAnimState(PlayerAnimState.BaseState,true);
        }
        
        private void OnReset()
        {
            gameObject.SetActive(true);
            movementController.OnReset();
           // SetStackPosition();
        }
    }
}