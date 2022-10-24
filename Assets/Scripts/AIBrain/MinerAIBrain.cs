using System;
using System.Collections;
using System.Threading;
using Abstract;
using Enums;
using Signals;
using States.Miner;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace AIBrain
{
    public class MinerAIBrain : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public NavMeshAgent Agent;
        public NavMeshObstacle Obstacle;

        public MoveToMineState MoveToMine;
        public MinerDigState MinerDigState;
        public MoveToGemHolder MoveToGemHolder;
        public GameObject GemAreaHolder;
        public GameObject Target;

        #endregion

        #region SerializField Variables

        [SerializeField] private Animator animator;
        [SerializeField] private GameObject pickaxe;
        [SerializeField] private GameObject diamond;

        #endregion

        #region Private Variables

        private MinerBaseState _currentState;

        #endregion
        #endregion

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            MoveToMine = new MoveToMineState();
            MinerDigState = new MinerDigState();
            MoveToGemHolder = new MoveToGemHolder();
        }

        private void OnEnable()
        {
            Target = IdleSignals.Instance.onGetMineGameObject();
            GemAreaHolder = IdleSignals.Instance.onGemAreaHolder();
            _currentState = MoveToMine;
            _currentState.EnterState(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            _currentState.OnTriggerEnterState(this,other);
        }

        public void SwichState(MinerBaseState state)
        {
            _currentState = state;
            _currentState.EnterState(this);
        }

        public void GemHolderWaiter()
        {
            StartCoroutine(GemHoldeWait());
        }
        
        public void DigWaiter()
        {
            StartCoroutine(DigWait());
        }

        private IEnumerator GemHoldeWait()
        {
            Agent.isStopped = true;
            DiamondController(false);
            IdleSignals.Instance.onGemHolderAddGem?.Invoke(transform);
            yield return new WaitForSeconds(0.3f);
            Agent.isStopped = false;
            SwichState(MoveToMine);
        }

        private IEnumerator DigWait()
        {
            AnimState(MinerAnimState.Dig);
            yield return new WaitForSeconds(5f);
            PickaxeController(false);
            Obstacle.enabled = false;
            yield return new WaitForSeconds(0.1f);
            Agent.enabled = true;
            SwichState(MoveToGemHolder);
        }
        

        public void PickaxeController(bool isOn)
        {
            pickaxe.SetActive(isOn);
        }
        
        public void DiamondController(bool isOn)
        {
            diamond.SetActive(isOn);
        }

        public void AnimState(MinerAnimState animState)
        {
            animator.SetTrigger(animState.ToString());
        }
    }
}