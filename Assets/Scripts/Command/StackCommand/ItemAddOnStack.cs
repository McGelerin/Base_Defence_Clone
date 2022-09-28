using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Data.ValueObject;
using DG.Tweening;
using UnityEngine;

namespace Command.StackCommand
{
    public class ItemAddOnStack
    {
        #region Self Variables

        #region Private Variables

        private List<GameObject> _stackList;
        private GameObject _stackHolder;
        private StackData _data;
        //private Vector3 _moneyPosition;

        #endregion

        #endregion

        public ItemAddOnStack(ref List<GameObject> stackList,ref GameObject stackHolder,ref StackData stackData)
        {
            _stackList = stackList;
            _stackHolder = stackHolder;
            _data = stackData;
            //_moneyPosition = moneyPosition;
        }

        public void Execute(GameObject obj , Vector3 position)
        {
            if (_stackList.Count >= _data.Capacity) return;
            obj.transform.SetParent(_stackHolder.transform);
            // if (_stackList.Count == 0)
            // {
            //     money.transform.DOLocalMove(Vector3.zero, 0.1f);
            //     money.transform.DOLocalRotate(Vector3.zero, 0.5f);
            // }
            // else
            // {
            obj.transform.DOLocalMove(position, 1f);
            obj.transform.DOLocalRotate(Vector3.zero, 0.5f);
            //}
            _stackList.Add(obj);
        }
    }
}