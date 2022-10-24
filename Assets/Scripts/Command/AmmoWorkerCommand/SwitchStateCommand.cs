using System;
using Abstract;
using Enums;
using States.AmmoWorker;

namespace Command.AmmoWorkerCommand
{
    public class SwitchStateCommand
    {
        #region Self Variables

        #region Private Variables
        
        private AmmoWorkerBaseState _currentState;
        private MoveToWareHouseArea _moveToWareHouse;
        private WaitForFullStack _waitForFullStack;
        private MoveToTurretAmmoArea _moveToTurretAmmoArea;
        private WaitToAmmoArea _waitToAmmoArea;
        private AnyState _anyState;
        
        
        #endregion

        #endregion

        public SwitchStateCommand (ref MoveToWareHouseArea moveToWareHouse,ref WaitForFullStack waitForFullStack,
            ref MoveToTurretAmmoArea moveToTurretAmmoArea, ref WaitToAmmoArea waitToAmmoArea, ref AnyState anyState)
        {
            _moveToWareHouse = moveToWareHouse;
            _waitForFullStack = waitForFullStack;
            _moveToTurretAmmoArea = moveToTurretAmmoArea;
            _waitToAmmoArea = waitToAmmoArea;
            _anyState = anyState;
        }
        
        public AmmoWorkerBaseState Execute(AmmoWorkerStates state)
        {
            _currentState = state switch
            {
                AmmoWorkerStates.MoveToWareHouse => _moveToWareHouse,
                AmmoWorkerStates.WaitForFullStack => _waitForFullStack,
                AmmoWorkerStates.MoveToTurretAmmoArea => _moveToTurretAmmoArea,
                AmmoWorkerStates.WaitToAmmoArea => _waitToAmmoArea,
                AmmoWorkerStates.AnyState => _anyState,
                _ => _currentState
            };
            return _currentState;
        }
    }
}