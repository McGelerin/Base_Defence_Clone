using UnityEngine;

namespace Managers.StateMachineWork
{
    public class AppleGrowState : AppleBaseState
    {
        private Vector3 _startingAppleSize = new Vector3(1, 1, 1);
        private Vector3 _appleGrowingMultipler = new Vector3(0.1f, 0.1f, 0.1f);
        public override void EnterState(AppleStateManager apple)
        {
            apple.transform.localScale = _startingAppleSize;
        }

        public override void UpdateState(AppleStateManager apple)
        {
            if (apple.transform.localScale.x < 1)
            {
                apple.transform.localScale += _appleGrowingMultipler * Time.deltaTime;
            }
            else
            {
                apple.SwichState(apple.WholeState);
            }
        }

        public override void OnCollisionEnter(AppleStateManager apple)
        {
            throw new System.NotImplementedException();
        }
    }
}