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

        public void SetOutSideAnimState(IdleInputParams inputParams/*,Transform target*//*,bool hasTarget*/)
        {
            // if (hasTarget)
            // {
            //     PlayerHasTarget(inputParams, target);
            // }
            // else
            // {
                var speedZ = Mathf.Abs(inputParams.ValueZ);
                var speedX = Mathf.Abs(inputParams.ValueX);
                animator.SetFloat("SpeedZ",Mathf.Clamp(speedX + speedZ,0,1));
            //}
        }

        //alttaki kısmı sonra yapacam kurgulayamadım yapamazsam distance gore yapacam sadece ileri geri ve ıdle şeklinde
        
        // private void PlayerHasTarget(IdleInputParams inputParams, Transform target)
        // {
        //     //  var playerPosition = transform.parent.position;
        //     //  var targetPosition = target.position;
        //     // // playerPosition.x += inputParams.ValueX;
        //     // // playerPosition.z += inputParams.ValueZ;
        //     // // var distance =  targetPosition - playerPosition;
        //     // // //var distance2 = targetPosition - playerPosition;
        //     // //
        //     // // //var speedX = distance.x - distance2.x;
        //     // // //var speedZ = distance.z - distance2.z;
        //     // // var speedX = Mathf.Clamp(-distance.x,-1,1);
        //     // // var speedZ = Mathf.Clamp(-distance.z,-1,1) ;
        //     //
        //     // var distance = Vector3.Distance(targetPosition, playerPosition);
        //     // float y = distance / 5;
        //     // var xDistance = (playerPosition.x - targetPosition.x) * y;
        //     // var zDistance = (playerPosition.z - targetPosition.z) * y;
        //     
        //     //animator.SetFloat("SpeedX", inputParams.ValueX);
        //     //animator.SetFloat("SpeedZ", inputParams.ValueZ);
        // }
    }
}