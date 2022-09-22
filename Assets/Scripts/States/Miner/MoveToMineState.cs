using Abstract;
using AIBrain;
using Cinemachine;
using Enums;
using UnityEngine;

namespace States.Miner
{
    public class MoveToMineState : MinerBaseState
    {
        public override void EnterState(MinerAIBrain miner)
        {
            miner.PickaxeController(true);
            miner.AnimState(MinerAnimState.Run);
            miner.Agent.SetDestination(miner.Target.transform.position);
        }

        public override void OnTriggerEnterState(MinerAIBrain miner, Collider other)
        {
            if (other.CompareTag("Mine"))
            {
                miner.Agent.enabled = false;
                miner.Obstacle.enabled = true;
                miner.SwichState(miner.MinerDigState);
            }
        }
    }
}