using System;
using Enums;
using Managers;
using UnityEngine;

namespace Controllers
{
    public class PlayerPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region SerializeField Variables

        [SerializeField] private PlayerManager manager;
        [SerializeField] private PlayerStackManager stackManager;
        [SerializeField] private RangedAttackManager rangedAttackManager;

        #endregion

        #endregion
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("BarrierInSide"))
            {
                gameObject.layer = LayerMask.NameToLayer("Default");
                manager.SetPlayerState(PlayerStateEnum.INSIDE);
                stackManager.IncreaseBarrierArea();
                rangedAttackManager.PlayerIncreaseBase();
                return;
            }

            if (other.CompareTag("BarrierOutSide"))
            {
                gameObject.layer = LayerMask.NameToLayer("PlayerOutSideLayer");
                manager.SetPlayerState(PlayerStateEnum.OUTSIDE);
                rangedAttackManager.PlayerIncreaseOutSide();
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