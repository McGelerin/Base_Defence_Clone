using AIBrain;
using UnityEngine;

namespace Abstract
{
    public abstract class HostageBaseStates
    {
        public abstract void EnterState(HostageAIBrain hostage);
        
        public abstract void UpdateState(HostageAIBrain hostage);
        
        public abstract void OnTriggerEnterState(HostageAIBrain hostage, Collider other);
    }
}