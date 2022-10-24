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
        [SerializeField] private RangedAttackManager rangedAttackManager;

        #endregion

        #endregion
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("BarrierInSide"))
            {
                BarrierInSide();
                return;
            }

            if (other.CompareTag("BarrierOutSide"))
            {
                BarrierOutSide();
            }
        }

        private void BarrierOutSide()
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerOutSideLayer");
            manager.SetPlayerState(PlayerStateEnum.Outside);
            rangedAttackManager.PlayerInteractOutSide();
        }

        private void BarrierInSide()
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            manager.SetPlayerState(PlayerStateEnum.Inside);
            
            rangedAttackManager.OnPlayerInteractBase();
        }
    }
}