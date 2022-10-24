using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Command.StackCommand
{
    public class PlayerDeathClearStack
    {
        #region Self Variables

        #region Private Variables

        private List<GameObject> _stackList;
        private Transform _stackParent;

        #endregion

        #endregion

        public PlayerDeathClearStack(ref List<GameObject> stackList,ref Transform stackParent)
        {
            _stackList = stackList;
            _stackParent = stackParent;
        }
        
        public void Execute()
        {
            for (int i = 0; i < _stackList.Count; i++)
            {
                var money = _stackList[i];
                money.transform.SetParent(_stackParent.parent);
                var position = _stackParent.position;
                money.transform.DOLocalJump(
                    new Vector3(position.x + Random.Range(-1f, 1f), 0.5f, 
                        position.z + Random.Range(0f, 1f)), 1f, 3, 0.5f);
                money.GetComponent<BoxCollider>().enabled = true;
            }
            _stackList.Clear();
        }
    }
}
