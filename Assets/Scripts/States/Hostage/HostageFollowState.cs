using Abstract;
using AIBrain;
using Enums;
using Signals;
using UnityEngine;

namespace States.Hostage
{
    public class HostageFollowState : HostageBaseStates
    {
        public override void EnterState(HostageAIBrain hostage)
        {
            hostage.Target = StackSignals.Instance.onGetHostageTarget(hostage.gameObject);
            hostage.AnimTriggerState(HostageAnimState.Idle);
            hostage.Agent.SetDestination(hostage.Target.transform.position);
        }

        public override void UpdateState(HostageAIBrain hostage)
        {
            FlowPlayer(hostage);
        }

        public override void OnTriggerEnterState(HostageAIBrain hostage, Collider other)
        {
        }

        private void FlowPlayer(HostageAIBrain hostage)
        {
            hostage.Agent.SetDestination(hostage.Target.transform.position);
            AnimStateChack(hostage);
        }

        private void AnimStateChack(HostageAIBrain hostage)
        {
            hostage.AnimBoolState(HostageAnimState.Follow, hostage.Agent.velocity.magnitude > 0.1f);
        }
    }
}