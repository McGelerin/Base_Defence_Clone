using System;
using Controller;
using Controllers;
using Enums;
using Keys;
using Signals;

namespace Command.PlayerCommand
{
    public class SetIdleInputValuesCommand
    {
        #region Self Variables

        #region Private Variables
        
        private PlayerMovementController _movementController;
        private PlayerAnimationController _animationController;
        
        
        #endregion

        #endregion

        public SetIdleInputValuesCommand (ref PlayerMovementController movementController,ref PlayerAnimationController animationController)
        {
            _movementController = movementController;
            _animationController = animationController;

        }
        
        public void Execute(IdleInputParams inputParams,PlayerStateEnum playerState)
        {
            switch (playerState)
            {
                case PlayerStateEnum.Inside:
                    _movementController.UpdateIdleInputValue(inputParams);
                    _animationController.SetSpeedVariable(inputParams);
                    break;
                case PlayerStateEnum.Outside:
                    _movementController.UpdateIdleInputValue(inputParams);
                    _animationController.SetSpeedVariable(inputParams);
                    _animationController.SetOutSideAnimState(inputParams,default,false);
                    break;
                case PlayerStateEnum.Taret:
                    _movementController.UpdateTurretInputValue(inputParams);
                    break;
                case PlayerStateEnum.LockTarget:
                    var target = AttackSignals.Instance.onPlayerIsTarget();
                    _movementController.UpdateIdleInputValue(inputParams);
                    _animationController.SetSpeedVariable(inputParams);
                    _animationController.SetOutSideAnimState(inputParams,target.transform,true);
                    break;
                case PlayerStateEnum.Death:
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}