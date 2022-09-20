using Abstract;
using Enums;
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

        public MoveToMineState MoveToMine = new MoveToMineState();
        public MinerDigState MinerDigState = new MinerDigState();
        public MoveToGemHolder MoveToGemHolder = new MoveToGemHolder();
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

        private void OnEnable()
        {
            Target = IdleSignals.Instance.onGetMineGameObject();
            GemAreaHolder = IdleSignals.Instance.onGemAreaHolder();
            Agent = GetComponent<NavMeshAgent>();
            _currentState = MoveToMine;
            _currentState.EnterState(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            _currentState.OnTriggerEnter(this,other);
        }

        public void SwichState(MinerBaseState state)//get set ile yapılabilir
        {
            _currentState = state;
            _currentState.EnterState(this);
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