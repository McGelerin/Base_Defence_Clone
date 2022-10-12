using System;
using Enums;
using Managers;
using Signals;
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
                stackManager.InteractBarrierArea();
                rangedAttackManager.PlayerInteractBase();
                return;
            }

            if (other.CompareTag("BarrierOutSide"))
            {
                gameObject.layer = LayerMask.NameToLayer("PlayerOutSideLayer");
                manager.SetPlayerState(PlayerStateEnum.OUTSIDE);
                rangedAttackManager.PlayerInteractOutSide();
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
                stackManager.AmmoStackChack();
            }
        }
    }
}