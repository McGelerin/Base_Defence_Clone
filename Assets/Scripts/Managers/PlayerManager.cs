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

        #endregion

        #region Private Variables

        private Transform _currentParent;
        private PlayerStateEnum _playerState;
        private PlayerStateEnum _playerStateCache;
        private PlayerAnimState _weaponAnimStateCache;
        private PlayerAnimState _weaponAttackAnimState;

        #endregion
        
        #endregion
        
        private void Awake()
        {
            _currentParent = transform.parent;
            Data = GetPlayerData();
            _playerState = PlayerStateEnum.Inside;
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
            IdleSignals.Instance.onInteractPlayerWithTurret += OnPlayerInTurret;
            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onReset += OnReset;
            AttackSignals.Instance.onPlayerHasTarget += OnPlayerHasTarget;
        }

        private void UnsubscribeEvents()
        {
            InputSignals.Instance.onJoystickDragged -= OnSetIdleInputValues;
            IdleSignals.Instance.onInteractPlayerWithTurret -= OnPlayerInTurret;
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onReset -= OnReset;
            AttackSignals.Instance.onPlayerHasTarget -= OnPlayerHasTarget;
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
                case PlayerStateEnum.Inside:
                    movementController.UpdateIdleInputValue(inputParams);
                    animationController.SetSpeedVariable(inputParams);
                    break;
                case PlayerStateEnum.Outside:
                    movementController.UpdateIdleInputValue(inputParams);
                    animationController.SetSpeedVariable(inputParams);
                    animationController.SetOutSideAnimState(inputParams,default,false);
                    break;
                case PlayerStateEnum.Taret:
                    movementController.UpdateTurretInputValue(inputParams);
                    break;
                case PlayerStateEnum.LockTarget:
                    var target = AttackSignals.Instance.onPlayerIsTarget();
                    movementController.UpdateIdleInputValue(inputParams);
                    animationController.SetSpeedVariable(inputParams);
                    animationController.SetOutSideAnimState(inputParams,target.transform,true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetPlayerState(PlayerStateEnum state)
        {
            _playerState = state;
            switch (_playerState)
            {
                case PlayerStateEnum.Inside:
                    _weaponAnimStateCache = IdleSignals.Instance.onSelectedWeaponAnimState();
                    animationController.SetBoolAnimState(PlayerAnimState.BaseState,true);
                    animationController.SetBoolAnimState(_weaponAnimStateCache,false);
                    break;
                case PlayerStateEnum.Outside:
                    animationController.SetBoolAnimState(PlayerAnimState.BaseState,false);
                    animationController.SetBoolAnimState(_weaponAnimStateCache,true);
                    break;
                case PlayerStateEnum.LockTarget:
                    break;
                case PlayerStateEnum.Taret:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void OnPlayerHasTarget(bool hasTarget)
        {
            if (hasTarget)
            {
                movementController.IsLockTarget(true);
                _weaponAttackAnimState = IdleSignals.Instance.onSelectedWeaponAttackAnimState();
                animationController.SetAnimState(_weaponAttackAnimState);
                _playerState = PlayerStateEnum.LockTarget;
            }
            else
            {
                animationController.SetAnimState(PlayerAnimState.AttackEnd);
                movementController.IsLockTarget(false);
                _playerState = PlayerStateEnum.Outside;
            }
        }

        private void OnPlayerInTurret()
        {
            _playerState = PlayerStateEnum.Taret;
            animationController.SetAnimState(PlayerAnimState.PlayerInTurret);
            movementController.IsReadyToPlay(false);
        }

        public void PlayerOutTurret()
        {
            _playerState = PlayerStateEnum.Inside;
            animationController.SetAnimState(PlayerAnimState.PlayerOutTurret);
            movementController.IsReadyToPlay(true);
            transform.SetParent(_currentParent);
        }

        private void OnPlay()
        {
            movementController.IsReadyToPlay(true);
            animationController.SetBoolAnimState(PlayerAnimState.BaseState,true);
        }
        
        private void OnReset()
        {
            gameObject.SetActive(true);
            movementController.OnReset();
        }
    }
}