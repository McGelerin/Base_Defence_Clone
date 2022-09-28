using System;
using Managers;
using UnityEngine;

namespace Controllers
{
    public class PlayerPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region SerializeField Variables

        [SerializeField] private PlayerStackManager stackManager;

        #endregion

        #endregion
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("BarrierInSide"))
            {
                gameObject.layer = LayerMask.NameToLayer("Default");
                stackManager.IncreaseBarrierArea();
                return;
            }

            if (other.CompareTag("BarrierOutSide"))
            {
                gameObject.layer = LayerMask.NameToLayer("PlayerOutSideLayer");
                return;
            }

            if (other.CompareTag("Money"))
            {
                stackManager.IncreaseMoney(other.gameObject);
                return;
            }

            if (other.CompareTag("AmmoReloadArea"))
            {
                stackManager.IncreaseAmmoArea(other.transform,true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("AmmoReloadArea"))
            {
                stackManager.IncreaseAmmoArea(default,false);
            }
        }
    }
}