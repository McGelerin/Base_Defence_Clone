using Abstract;
using AIBrain;
using Signals;
using UnityEngine;

namespace States.Miner
{
    public class MoveToGemHolder : MinerBaseState
    {
        public override void EnterState(MinerAIBrain miner)
        {
            miner.Agent.SetDestination(miner.GemAreaHolder.transform.position);
        }

        public override void OnTriggerEnter(MinerAIBrain miner, Collider other)
        {
            if (other.CompareTag("GemHolder"))
            {
                IdleSignals.Instance.onGemHolderAddGem?.Invoke(miner.transform);
                miner.SwichState(miner.MoveToMine);
            }
        }
    }
}