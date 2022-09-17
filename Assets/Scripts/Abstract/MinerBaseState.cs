using AIBrain;
using UnityEngine;

namespace Abstract
{
    public abstract class MinerBaseState
    {
        public abstract void EnterState(MinerAIBrain miner);

        public abstract void OnTriggerEnter(MinerAIBrain miner, Collider other);
    }
}