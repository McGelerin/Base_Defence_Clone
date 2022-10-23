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

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                manager.BarrierState = BarrierEnum.Open;
            }

            if (other.CompareTag("Soldier"))
            {
                manager.BarrierState = BarrierEnum.Open;
            }

            if (other.CompareTag("MoneyWorker"))
            {
                manager.BarrierState = BarrierEnum.Open;
            }

            if (other.CompareTag("Hostage"))
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
            
            if (other.CompareTag("Soldier"))
            {
                manager.BarrierState = BarrierEnum.Close;
            }

            if (other.CompareTag("MoneyWorker"))
            {
                manager.BarrierState = BarrierEnum.Close;
            }

            if (other.CompareTag("Hostage"))
            {
                manager.BarrierState = BarrierEnum.Close;
            }
        }
    }
}