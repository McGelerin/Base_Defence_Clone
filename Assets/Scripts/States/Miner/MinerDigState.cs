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
            Waiter(miner);
        }

        public override void OnTriggerEnter(MinerAIBrain miner, Collider other)
        {
        }
        
        private async void Waiter(MinerAIBrain miner)
        {
            miner.AnimState(MinerAnimState.Dig);
            await Task.Delay(5000);
            miner.PickaxeController(false);
            miner.SwichState(miner.MoveToGemHolder);
        }
    }
}