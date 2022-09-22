using System;
using Enums;
using Managers.Barrier;
using UnityEngine;

namespace Controller.Barrier
{
    public class BarrierPhysicController : MonoBehaviour
    {
        #region Self Variables

        #region SerializeField Variables

        [SerializeField] private BarrierManager manager;
        [SerializeField] private bool isInside;
        

        #endregion

        #endregion
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")/*Buraya boşatma ve yoldurma yapmak için bir değer gelmesi gerekmektedir*/)
            {
                if (isInside)
                {
                    //manager.BarrierState = BarrierEnum.Open;
                }
                else
                {
                    manager.BarrierState = BarrierEnum.Open;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (isInside)
                {
                    //manager.BarrierState = BarrierEnum.Close;
                }
                else
                {
                    manager.BarrierState = BarrierEnum.Close;
                }
            }
        }
    }
}