using System;
using Abstract;
using Enums;
using States.Hostage;
using UnityEngine;
using UnityEngine.AI;

namespace AIBrain
{
    public class HostageAIBrain : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public NavMeshAgent Agent;
        public HostageTerrifiedState HostageTerrifiedState;
        public HostageFollowState HostageFollowState;
        public GameObject Target;

        #endregion

        #region SerializField Variables

        [SerializeField] private Animator animator;
        [SerializeField] private float checkTimer;

        #endregion

        #region Private Variables

        private HostageBaseStates _currentState;
        private float timer;
        
        #endregion
        #endregion

        private void Awake()
        {
            HostageTerrifiedState = new HostageTerrifiedState();
            HostageFollowState = new HostageFollowState();
            Agent = GetComponent<NavMeshAgent>();
        }

        private void OnEnable()
        {
            _currentState = HostageTerrifiedState;
            _currentState.EnterState(this);
        }
        
        private void Update()
        {
            //timer += Time.deltaTime;
            //if (!(timer >= chackTimer)) return;
            _currentState.UpdateState(this);
            //timer = 0;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            _currentState.OnTriggerEnterState(this,other);
        }
        
        public void SwichState(HostageBaseStates state)//get set ile yapılabilir
        {
            _currentState = state;
            _currentState.EnterState(this);
        }
        
        public void AnimTriggerState(HostageAnimState animState)
        {
            animator.SetTrigger(animState.ToString());
        }
        
        public void AnimBoolState(HostageAnimState animState,bool isFollow)
        {
            animator.SetBool(animState.ToString(),isFollow);
        }
    }
}