using UnityEngine;

namespace Managers.StateMachineWork
{
    public class AppleStateManager : MonoBehaviour
    {
        public AppleBaseState CurrentState;
        public AppleGrowState GrowState = new AppleGrowState();
        public AppleChewedState ChewedState = new AppleChewedState();
        public AppleRottenState RottenState = new AppleRottenState();
        public AppleWholeState WholeState = new AppleWholeState();

        void Start()
        {
            CurrentState = GrowState;
            CurrentState.EnterState(this);
        }

        // Update is called once per frame
        void Update()
        {
            CurrentState.UpdateState(this);
        }

        public void SwichState(AppleBaseState state)//get set ile yapılabilir
        {
            CurrentState = state;
            CurrentState.EnterState(this);
        }
    }
}
