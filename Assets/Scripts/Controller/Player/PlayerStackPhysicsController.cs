using System;
using Managers;
using Signals;
using UnityEngine;

namespace Controller.Player
{
    public class PlayerStackPhysicsController : MonoBehaviour
    {
        #region Self Variables
        #region Public Variables

        public bool isEnable = true;
        
        #endregion

        #region Serialized Variables
        
        [SerializeField] private PlayerStackManager stackManager;
        
        #endregion


        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (!isEnable) return;
            
            if (other.CompareTag("BarrierInSide"))
            {
                stackManager.InteractBarrierArea();
                return;
            }

            if (other.CompareTag("Money"))
            {
                stackManager.InteractMoney(other.gameObject);
                return;
            }

            if (other.CompareTag("AmmoReloadArea"))
            {
                stackManager.InteractWareHouseArea(other.transform,true);
                return;
            }

            if (other.CompareTag("TurretAmmoArea"))
            {
                stackManager.InteractTurretAmmoArea(other.gameObject);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("AmmoReloadArea"))
            {
                stackManager.InteractWareHouseArea(default,false);
            }

            if (other.CompareTag("TurretAmmoArea"))
            {
                StackSignals.Instance.onDecreseStackHolder?.Invoke(other.gameObject);
                stackManager.AmmoStackCheck();
            }
        }
    }
}