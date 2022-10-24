using System.Threading.Tasks;
using Abstract;
using AIBrain;
using Enums;
using UnityEngine;

namespace States.Miner
{
    public class MinerDigState : MinerBaseState
    {
        public override void EnterState(MinerAIBrain miner)
        {
            miner.transform.LookAt(miner.Target.transform);
            //Waiter(miner);
            miner.DigWaiter();
        }

        public override void OnTriggerEnterState(MinerAIBrain miner, Collider other)
        {
        }
        
        // private async void Waiter(MinerAIBrain miner)
        // {
        //     miner.AnimState(MinerAnimState.Dig);
        //     await Task.Delay(5000);
        //     miner.PickaxeController(false);
        //     //miner.Agent.isStopped = false;
        //     miner.Obstacle.enabled = false;
        //     await Task.Delay(100);
        //     miner.Agent.enabled = true;
        //     miner.SwichState(miner.MoveToGemHolder);
        // }
    }
}