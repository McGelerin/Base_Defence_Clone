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
        #endregion
        #endregion
        
        private void Awake()
        {
            Data = GetPlayerData();
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
            InputSignals.Instance.onInputTaken += OnActivateMovement;
            InputSignals.Instance.onInputReleased += OnDeactiveMovement;
            InputSignals.Instance.onJoystickDragged += OnSetIdleInputValues;
            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onReset += OnReset;
            //CoreGameSignals.Instance.onChangeGameState += OnChangeMovementState;
            //LevelSignals.Instance.onLevelSuccessful += OnLevelSuccessful;
            //LevelSignals.Instance.onLevelFailed += OnLevelFailed;
            //IdleSignals.Instance.onIteractionBuild += OnInteractionBuyPoint;

        }

        private void UnsubscribeEvents()
        {
            InputSignals.Instance.onInputTaken -= OnActivateMovement;
            InputSignals.Instance.onInputReleased -= OnDeactiveMovement;
            InputSignals.Instance.onJoystickDragged -= OnSetIdleInputValues;
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onReset -= OnReset;
            //CoreGameSignals.Instance.onChangeGameState -= OnChangeMovementState;
            //LevelSignals.Instance.onLevelSuccessful -= OnLevelSuccessful;
            //LevelSignals.Instance.onLevelFailed -= OnLevelFailed;
            //IdleSignals.Instance.onIteractionBuild -= OnInteractionBuyPoint;
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

        private void OnActivateMovement()
        {
            movementController.EnableMovement();
        }

        private void OnDeactiveMovement()
        {
            movementController.DeactiveMovement();
        }

        private void OnSetIdleInputValues(IdleInputParams inputParams)
        {
            movementController.UpdateIdleInputValue(inputParams);
            animationController.SetSpeedVariable(inputParams);
        }

        // private void OnChangeMovementState()
        // {
        //     movementController.IsReadyToPlay(true);
        //     movementController.ChangeMovementState();
        //     movementController.EnableMovement();
        //     //_rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        // }
        
        private void OnPlay()
        {
            //  SetStackPosition();
            movementController.IsReadyToPlay(true);
            animationController.SetAnimState(PlayerAnimState.Run);
        }
        
        private void OnReset()
        {
            gameObject.SetActive(true);
            movementController.OnReset();
           // SetStackPosition();
        }
    }
}