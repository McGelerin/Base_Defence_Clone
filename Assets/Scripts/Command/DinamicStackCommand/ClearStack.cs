using System.Collections.Generic;
using Enums;
using Signals;
using UnityEngine;

namespace Command.StackCommand
{
    public class ClearStack
    {
        #region Self Variables

        #region Private Variables

        private List<GameObject> _stackList;

        #endregion

        #endregion

        public ClearStack(ref List<GameObject> stackList)
        {
            _stackList = stackList;
        }
        
        public void Execute(PoolType poolType)
        {
            foreach (var money in _stackList)
            {
                PoolSignals.Instance.onReleasePoolObject?.Invoke(poolType.ToString(),money);
            }
            _stackList.Clear();
        }
    }
}