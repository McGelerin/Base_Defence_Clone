using System;
using DG.Tweening;
using Enums;
using UnityEngine;

namespace Managers.Barrier
{
    public class BarrierManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public BarrierEnum BarrierState
        {
            get => barrierState;
            set
            {
                barrierState = value;
                BarrierOpenClose();
            }
        }

        #endregion
        #region SerializeField Variables
        
        [SerializeField] private GameObject barrier;
        [SerializeField]private BarrierEnum barrierState=BarrierEnum.Close;
        
        #endregion
        #region Private Variables
        
        #endregion

        #endregion

        private void Awake()
        {
            BarrierState = barrierState;
        }

        private void BarrierOpenClose()
        {
            if (BarrierState == BarrierEnum.Open)
            {
                barrier.transform.DOLocalRotate(new Vector3(0, 0, 85), .5f );
            }
            else
            {
                barrier.transform.DOLocalRotate(Vector3.zero, .5f );
            }
        }
    }
}