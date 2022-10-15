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

        #endregion

        #endregion
        // private void OnTriggerEnter(Collider other)
        // {
        //     if (other.CompareTag("Player")/*Buraya boşatma ve yoldurma yapmak için bir değer gelmesi gerekmektedir*/)
        //     {
        //         manager.BarrierState = BarrierEnum.Open;
        //     }
        // }
        //
        //

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                manager.BarrierState = BarrierEnum.Open;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                manager.BarrierState = BarrierEnum.Close;
            }
        }
    }
}