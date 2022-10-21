using System.Collections;
using Abstract;
using Controller;
using Controllers;
using Data.UnityObject;
using Data.ValueObject;
using DG.Tweening;
using Enums;
using Signals;
using States.Soldier;
using UnityEngine;
using UnityEngine.AI;
namespace AIBrain
{
    public class SoldierAIWorker : AttackRadius
    {
        #region Self Variables

        #region Public Variables

        public GameObject SearchInitPosition;
        public GameObject Target;

        #endregion

        #region Serialized Variables
        
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject weapon;

        #endregion

        #region Private Variables


        private int _health;
        private SoldierAIData _data;
        private bool _isAttack;
        private bool _firstAttack;

        #region Stats
        
        private SoldierBaseStates _currentState;
        private MoveToInitPosition _moveToInitPosition;
        private MoveToSearchInitPosition _moveToSearchInitPosition;
        private SearchEnemy _searchEnemy;
        private MoveToEnemy _moveToEnemy;
        private RangedAttack _rangedAttack;
        private Dead _dead;
        

        #endregion
        #endregion
        #endregion
        
        protected override void Awake()
        {
            InitReferances();
            base.Awake();
        }

        private void InitReferances()
        {
            var brain = this;
            _data = Resources.Load<CD_AI>("Data/Cd_AI").SoldierAIData;
            AttackDelay = _data.AttackDelay;
            _moveToInitPosition = new MoveToInitPosition(ref brain, ref agent);
            _moveToSearchInitPosition = new MoveToSearchInitPosition(ref brain, ref agent);
            _searchEnemy = new SearchEnemy(ref brain, ref agent);
            _moveToEnemy = new MoveToEnemy(ref brain, ref agent, ref _data);
            _rangedAttack = new RangedAttack(ref brain, ref agent, ref _data);
            _dead = new Dead(ref brain, ref agent);
        }

        #region Event Subscription

        protected override void OnEnable()
        {
            SubscribeEvents();
            base.OnEnable();
            _health = _data.Health;
            _firstAttack = true;
            _currentState = _moveToInitPosition;
            _currentState.EnterState();
        }

        private void SubscribeEvents()
        {
            WorkerSignals.Instance.onSoldierAttack += MoveToSoldierAttackPosition;
        }

        private void UnsubscribeEvents()
        {            
            WorkerSignals.Instance.onSoldierAttack -= MoveToSoldierAttackPosition;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            UnsubscribeEvents();
        }

        #endregion

        private void Update()
        {
            _currentState.UpdateState();
        }

        private void MoveToSoldierAttackPosition()
        {
            if (_firstAttack)
            {
                _firstAttack = false;
                SwitchState(SoldierStates.MoveToSearchInitPosition);
            }
        }

        public void SwitchState(SoldierStates state)
        {
            _currentState = state switch
            {
                SoldierStates.MoveToInitPosition => _moveToInitPosition,
                SoldierStates.MoveToSearchInitPosition => _moveToSearchInitPosition,
                SoldierStates.SearchEnemy => _searchEnemy,
                SoldierStates.MoveToEnemy => _moveToEnemy,
                SoldierStates.RangedAttack => _rangedAttack,
                SoldierStates.Dead => _dead,
                _ => _currentState
            };
            _currentState.EnterState();
        }

        private void OnTakeDamage()
        {
            
        }
        
        public bool HealthCheck()
        {
            return _health <= 0;
        }

        public void IsAttack(bool isAttack)
        {
            _isAttack = isAttack;
        }
        
        public void IsDeath(){StartCoroutine(Death());}
        
        private IEnumerator Death()
        {
            //geliştirecem
            WaitForSeconds wait = new WaitForSeconds(1f);
            AnimTriggerState(SoldierAnimState.Death);
            transform.DOLocalMoveY(0f, 0.5f);
            yield return wait;
            WorkerSignals.Instance.onSoldierDeath?.Invoke();
            PoolSignals.Instance.onReleasePoolObject(PoolType.Soldier.ToString(), gameObject);
        }
        
        protected override void HasTarget()
        {
            Target = TargetEnemy;
            SwitchState(SoldierStates.MoveToEnemy);
        }

        protected override void RangedAttack()
        {
            if (_isAttack)
            {
                var bullet = PoolSignals.Instance.onGetPoolObject(PoolType.SoldierBullet.ToString(), weapon.transform);
                bullet.GetComponent<SoldierBulletPhysicsController>().SetAddForce(transform.forward * 10);
            }
        }

        protected override void AttackEnd()
        {
            AnimTriggerState(SoldierAnimState.AttackEnd);
            SwitchState(SoldierStates.SearchEnemy);
        }

        protected override bool TriggerEnter(Collider other)
        {
            return false;
        }

        protected override bool TriggerExit(Collider other)
        {
            return false;
        }
        
        public void AnimTriggerState(SoldierAnimState states)
        {
            animator.SetTrigger(states.ToString());
        }
        
        public void AnimSetFloat(float speed)
        {
            animator.SetFloat("Speed",speed);
        }
    }
}