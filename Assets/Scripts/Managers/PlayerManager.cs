using System;
using System.Collections;
using Command.PlayerCommand;
using Command.StackCommand;
using Controller;
using Controller.Player;
using Controllers;
using Data.UnityObject;
using Data.ValueObject;
using DG.Tweening;
using Enums;
using Keys;
using Signals;
using UnityEngine;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private PlayerStackPhysicsController stackPhysicsController;
        [SerializeField] private PlayerMovementController movementController;
        [SerializeField] private PlayerAnimationController animationController;
        [SerializeField] private PlayerHealthController playerHealthController;
        [SerializeField] private GameObject playerPhysics;
        [SerializeField] private GameObject mesh;

        #endregion

        #region Private Variables

        private PlayerData _data;
        private Coroutine _death;
        private Transform _currentParent;
        private PlayerStateEnum _playerState;
        private PlayerStateEnum _playerStateCache;
        private PlayerAnimState _weaponAnimStateCache;
        private PlayerAnimState _weaponAttackAnimState;
        private SetIdleInputValuesCommand _setIdleInputValuesCommand;
        private SetPlayerStateCommand _setPlayerStateCommand;

        #endregion
        
        #endregion
        
        private void Awake()
        {
            var manager = this;
            _currentParent = transform.parent;
            _data = GetPlayerData();
            _playerState = PlayerStateEnum.Inside;
            SendPlayerDataToControllers();
            _setIdleInputValuesCommand = new SetIdleInputValuesCommand(ref movementController, ref animationController);
            _setPlayerStateCommand = new SetPlayerStateCommand(ref manager, ref movementController,
                ref animationController, ref playerHealthController);
        }
        private PlayerData GetPlayerData() => Resources.Load<CD_PlayerData>("Data/CD_Player").Data;
        
        private void SendPlayerDataToControllers()
        {
            movementController.SetMovementData(_data.MovementData);
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
            AttackSignals.Instance.onGiveDamageToPlayer += OnTakeDamage;
        }

        private void UnsubscribeEvents()
        {
            InputSignals.Instance.onJoystickDragged -= OnSetIdleInputValues;
            IdleSignals.Instance.onInteractPlayerWithTurret -= OnPlayerInTurret;
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onReset -= OnReset;
            AttackSignals.Instance.onPlayerHasTarget -= OnPlayerHasTarget;
            AttackSignals.Instance.onGiveDamageToPlayer -= OnTakeDamage;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void Start()
        {
            CoreGameSignals.Instance.onSetPlayerPosition?.Invoke(transform);
            playerHealthController.GetHealth(_data.PlayerHealth);
        }

        private void OnSetIdleInputValues(IdleInputParams inputParams)
        {
            _setIdleInputValuesCommand.Execute(inputParams,_playerState);
        }

        public void SetPlayerState(PlayerStateEnum state)
        {
            _playerState = state;
            if (state == PlayerStateEnum.Inside || state == PlayerStateEnum.Death)
            {
                _weaponAnimStateCache = IdleSignals.Instance.onSelectedWeaponAnimState();
            }
            _setPlayerStateCommand.Execute(state,_weaponAnimStateCache);
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
            transform.rotation = Quaternion.identity;
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

        private void OnTakeDamage(int damage)
        {
            playerHealthController.TakeDamage(damage);
        }

        public void PlayerDeath()
        {
            _death ??= StartCoroutine(Death());
        }

        private IEnumerator Death()
        {
            DeathStart();
            yield return new WaitForSeconds(3f);
            DeathEnd();
            _death = null;
        }

        private void DeathStart()
        {
            playerPhysics.layer = LayerMask.NameToLayer("Default");
            stackPhysicsController.isEnable = false;
            IdleSignals.Instance.onPlayerDeath?.Invoke();
            animationController.SetAnimState(PlayerAnimState.Death);
            mesh.transform.DOLocalMoveY(-0.5f, 0.5f);
        }
        
        private void DeathEnd()
        {
            transform.position = WorkerSignals.Instance.onGetBaseCenter().transform.position;
            stackPhysicsController.isEnable = true;
            animationController.SetAnimState(PlayerAnimState.Reborn);
            mesh.transform.localPosition = Vector3.zero;
            movementController.IsReadyToPlay(true);
            SetPlayerState(PlayerStateEnum.Inside);
            playerHealthController.GetHealth(_data.PlayerHealth);
        }

    }
}