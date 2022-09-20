using System.Threading.Tasks;
using Abstract;
using AIBrain;
using Enums;
using Signals;
using UnityEngine;

namespace States.Miner
{
    public class MoveToGemHolder : MinerBaseState
    {
        public override void EnterState(MinerAIBrain miner)
        {
            miner.DiamondController(true);
            miner.AnimState(MinerAnimState.Transport);
            miner.Agent.SetDestination(miner.GemAreaHolder.transform.position);
        }

        public override void OnTriggerEnter(MinerAIBrain miner, Collider other)
        {
            if (other.CompareTag("GemHolder"))
            {
                Waiter(miner);
            }
        }
        
        private async void Waiter(MinerAIBrain miner)
        {
            miner.Agent.isStopped = true;
            miner.DiamondController(false);
            IdleSignals.Instance.onGemHolderAddGem?.Invoke(miner.transform);
            await Task.Delay(300);
            miner.Agent.isStopped = false;
            miner.SwichState(miner.MoveToMine);
        }
    }
}