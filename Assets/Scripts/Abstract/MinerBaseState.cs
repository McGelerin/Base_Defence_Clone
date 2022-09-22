using AIBrain;
using UnityEngine;

namespace Abstract
{
    public abstract class MinerBaseState
    {
        public abstract void EnterState(MinerAIBrain miner);

        public abstract void OnTriggerEnterState(MinerAIBrain miner, Collider other);
    }
}