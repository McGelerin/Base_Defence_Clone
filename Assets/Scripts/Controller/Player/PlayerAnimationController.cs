using Enums;
using Keys;
using Managers;
using UnityEngine;

namespace Controllers
{
    public class PlayerAnimationController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private PlayerManager manager;
        [SerializeField] private Animator animator;

        #endregion

        #endregion

        public void SetAnimState(PlayerAnimState animState)
        {
            animator.SetTrigger(animState.ToString());
        }

        public void SetSpeedVariable(IdleInputParams inputParams)
        {
            float speedX = Mathf.Abs(inputParams.ValueX);
            float speedZ = Mathf.Abs(inputParams.ValueZ);
            animator.SetFloat("Speed", (speedX + speedZ) / 2);
        }
    }
}