using System.Threading.Tasks;
using Abstract;
using AIBrain;
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
            await Task.Delay(2000);
            miner.SwichState(miner.MoveToGemHolder);
        }
    }
}