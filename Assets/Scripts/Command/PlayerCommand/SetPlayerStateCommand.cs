using System;
using Controller;
using Controller.Player;
using Controllers;
using Enums;
using Managers;
using Signals;

namespace Command.PlayerCommand
{
    public class SetPlayerStateCommand
    {
        #region Self Variables

        #region Private Variables
        
        private PlayerMovementController _movementController;
        private PlayerAnimationController _animationController;
        private PlayerHealthController _playerHealthController;
        private PlayerManager _playerManager;


        #endregion

        #endregion

        public SetPlayerStateCommand (ref PlayerManager manager,ref PlayerMovementController movementController,ref PlayerAnimationController animationController,
            ref PlayerHealthController playerHealthController)
        {
            _playerManager = manager;
            _movementController = movementController;
            _animationController = animationController;
            _playerHealthController = playerHealthController;
        }
        
        public void Execute(PlayerStateEnum playerState,PlayerAnimState weaponAnimStateCache)
        {
            switch (playerState)
            {
                case PlayerStateEnum.Inside:
                    _animationController.SetBoolAnimState(PlayerAnimState.BaseState,true);
                    _animationController.SetBoolAnimState(weaponAnimStateCache,false);
                    _playerHealthController.PlayerInTheBase(true);
                    break;
                case PlayerStateEnum.Outside:
                    _animationController.SetBoolAnimState(PlayerAnimState.BaseState,false);
                    _animationController.SetBoolAnimState(weaponAnimStateCache,true);
                    _playerHealthController.PlayerInTheBase(false);
                    break;
                case PlayerStateEnum.LockTarget:
                    break;
                case PlayerStateEnum.Taret:
                    break;
                case PlayerStateEnum.Death:
                    _movementController.IsReadyToPlay(false);
                    _animationController.SetBoolAnimState(PlayerAnimState.BaseState,true);
                    _animationController.SetBoolAnimState(weaponAnimStateCache,false);
                    _playerManager.PlayerDeath();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}