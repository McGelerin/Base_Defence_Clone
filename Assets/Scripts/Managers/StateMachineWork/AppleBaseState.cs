namespace Managers.StateMachineWork
{
    public abstract class AppleBaseState
    {
       public abstract void EnterState(AppleStateManager apple);
       public abstract void UpdateState(AppleStateManager apple);
       public abstract void OnCollisionEnter(AppleStateManager apple);
    }
}