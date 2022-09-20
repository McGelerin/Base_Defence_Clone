using Abstract;
using AIBrain;
using Enums;
using Signals;
using UnityEngine;

namespace States.Hostage
{
    public class HostageTerrifiedState : HostageBaseStates
    {
        public override void EnterState(HostageAIBrain hostage)
        {
            hostage.AnimTriggerState(HostageAnimState.Terrified);
        }

        public override void UpdateState(HostageAIBrain hostage)
        {
        }

        public override void OnTriggerEnter(HostageAIBrain hostage, Collider other)
        {
            if (other.CompareTag("Player"))
            {
                hostage.SwichState(hostage.HostageFlowState);
            }
        }
    }
}