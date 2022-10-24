using System.Collections.Generic;
using Data.ValueObject;
using UnityEngine;

namespace Command
{
    public class StaticStackItemPosition
    {
        #region Self Variables

        #region Private Variables
        
        private List<GameObject> _stackList;
        private GameObject _stackHolder;
        private StaticStackData _data;
        private int _squareMeters;

        #endregion
        #endregion

        public StaticStackItemPosition(ref List<GameObject> stackList,ref StaticStackData stackData, ref GameObject stackHolder)
        {
            _stackList = stackList;
            _data = stackData;
            _stackHolder = stackHolder;
            _squareMeters = _data.StackCountX * _data.StackCountZ;
        }

        public Vector3 Execute(Vector3 staticStackPosition)
        {
            staticStackPosition = _stackHolder.transform.localPosition + _data.InitPosition;
            staticStackPosition.x += _stackList.Count % _data.StackCountX / _data.StackOffset.x;
            staticStackPosition.y += _stackList.Count / _squareMeters / _data.StackOffset.y;
            staticStackPosition.z -= _stackList.Count / _data.StackCountX % _data.StackCountZ / _data.StackOffset.z;
            return staticStackPosition;
        }

    }
}