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

        public void SetBoolAnimState(PlayerAnimState animState, bool animStateBool)
        {
            animator.SetBool(animState.ToString(),animStateBool);
        }

        public void SetSpeedVariable(IdleInputParams inputParams)
        {
            var speedX = Mathf.Abs(inputParams.ValueX);
            var speedZ = Mathf.Abs(inputParams.ValueZ);
            animator.SetFloat("Speed", Mathf.Clamp(speedX + speedZ,0,1));
        }

        public void SetOutSideAnimState(IdleInputParams inputParams,Transform target)
        {
            if (target != null)
            {
                var position = target.position;
                var speedX = - position.x + (position.x + inputParams.ValueX);
                var speedZ = - position.z + (position.z + inputParams.ValueZ);
                animator.SetFloat("SpeedX",speedX);
                animator.SetFloat("SpeedZ",speedZ);
            }
            else
            {
                var speedZ = Mathf.Abs(inputParams.ValueZ);
                var speedX = Mathf.Abs(inputParams.ValueX);
                animator.SetFloat("SpeedZ",Mathf.Clamp(speedX + speedZ,0,1));
            }
        }
    }
}