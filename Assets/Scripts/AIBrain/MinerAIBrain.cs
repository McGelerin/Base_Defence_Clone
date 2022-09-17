using System;
using System.Collections;
using Abstract;
using Signals;
using States.Miner;
using UnityEngine;
using UnityEngine.AI;

namespace AIBrain
{
    public class MinerAIBrain : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public NavMeshAgent Agent;
        public MinerBaseState CurrentState;
        public MoveToMineState MoveToMine = new MoveToMineState();
        public MinerDigState MinerDigState = new MinerDigState();
        public MoveToGemHolder MoveToGemHolder = new MoveToGemHolder();
        public GameObject GemAreaHolder;
        public GameObject Target;

        #endregion

        #endregion

        private void OnEnable()
        {
            Target = IdleSignals.Instance.onGetMineGameObject();
            GemAreaHolder = IdleSignals.Instance.onGemAreaHolder();
            Agent = GetComponent<NavMeshAgent>();
            CurrentState = MoveToMine;
            CurrentState.EnterState(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            CurrentState.OnTriggerEnter(this,other);
        }

        public void SwichState(MinerBaseState state)//get set ile yapılabilir
        {
            CurrentState = state;
            CurrentState.EnterState(this);
        }
    }
}