using Abstract;
using AIBrain;
using Enums;
using Signals;
using UnityEngine;

namespace States.Hostage
{
    public class HostageFlowState : HostageBaseStates
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
            AnimStateChack(hostage);
        }

        public override void OnTriggerEnter(HostageAIBrain hostage, Collider other)
        {
        }

        private void FlowPlayer(HostageAIBrain hostage)
        {
            hostage.Agent.SetDestination(hostage.Target.transform.position);
        }

        private void AnimStateChack(HostageAIBrain hostage)
        {
            if (hostage.Agent.remainingDistance <= hostage.Agent.stoppingDistance)
            {
                hostage.AnimBoolState(HostageAnimState.Follow,false);
                return;
            }
            hostage.AnimBoolState(HostageAnimState.Follow,true);
        }
    }
}