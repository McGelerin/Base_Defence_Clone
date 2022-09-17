using Abstract;
using AIBrain;
using Cinemachine;
using UnityEngine;

namespace States.Miner
{
    public class MoveToMineState : MinerBaseState
    {
        public override void EnterState(MinerAIBrain miner)
        {
            miner.Agent.SetDestination(miner.Target.transform.position);
        }

        public override void OnTriggerEnter(MinerAIBrain miner, Collider other)
        {
            if (other.CompareTag("Mine"))
            {
                miner.SwichState(miner.MinerDigState);
            }
        }
    }
}