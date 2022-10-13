using System.Collections;
using Abstract;
using Data.UnityObject;
using Data.ValueObject;
using DG.Tweening;
using Enums;
using Signals;
using States.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace AIBrain
{
    public class EnemyAIBrain : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public GameObject TurretTarget;
        public GameObject PlayerTarget;

        #endregion

        #region SerializField Variables

        [SerializeField] private EnemyType enemyType;
        [SerializeField]private NavMeshAgent agent;
        [SerializeField] private Animator animator;
        [SerializeField] private float checkTimer;
        [SerializeField] private GameObject enemyBody;

        #endregion

        #region Private Variables

        private MoveToTurret _moveToTurret;
        private ChaseToPlayer _chaseToPlayer;
        private AttackToPlayer _attackToPlayer;
        private EnemyDeath _enemyDeath;
        private EnemyBaseState _currentState;
        private EnemyTypeData _data;
        private GameObject _money;
        private int _health;
        private float timer;
        
        #endregion
        #endregion

        private void Awake()
        {
            var brain = this;
            _data = Resources.Load<Cd_AI>("Data/Cd_AI").EnemyAIData.EnemyTypeDatas[enemyType];
            _moveToTurret = new MoveToTurret(ref brain, ref agent,ref _data);
            _chaseToPlayer = new ChaseToPlayer(ref brain, ref agent, ref _data);
            _attackToPlayer = new AttackToPlayer(ref brain, ref agent, ref _data);
            _enemyDeath = new EnemyDeath(ref brain, ref agent, ref _data);
        }
        private void OnEnable()
        {
            _health = _data.Health;
            TurretTarget = IdleSignals.Instance.onEnemyTarget();
            _currentState = _moveToTurret;
            _currentState.EnterState();
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (!(timer >= checkTimer)) return;
            _currentState.UpdateState();
            timer = 0;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            _currentState.OnTriggerEnterState(other);
        }

        private void OnTriggerExit(Collider other)
        {
            _currentState.OnTriggerExitState(other);
        }

        public void SwitchState(EnemyStates state)//get set ile yapılabilir
        {
            _currentState = state switch
            {
                EnemyStates.Walk => _moveToTurret,
                EnemyStates.Chase => _chaseToPlayer,
                EnemyStates.Attack => _attackToPlayer,
                EnemyStates.Death => _enemyDeath,
                _ => _currentState
            };
            _currentState.EnterState();
        }

        public void AttackStatus(bool isAttack)
        {
            if (isAttack)StartCoroutine(Attack());
            else StopAllCoroutines();
        }

        public void TakeBulletDamage()
        {
            _health -= AttackSignals.Instance.onGetWeaponDamage();
        }
        public void TakeAmmoDamage()
        {
            _health -= AttackSignals.Instance.onGetWeaponDamage();
        }

        public bool HealthCheck()
        {
            return _health <= 0;
        }

        private IEnumerator Attack()
        {
            WaitForSeconds wait = new WaitForSeconds(1.1f);
            while (true)
            {
                AnimTriggerState(EnemyStates.Attack);
                yield return wait;
                //invoke atılacak
            }
        }
        
        private IEnumerator Death()
        {
            WaitForSeconds wait = new WaitForSeconds(1f);
            AnimBoolState(EnemyStates.Death , true);
            AttackSignals.Instance.onEnemyDead?.Invoke(enemyBody);
            IdleSignals.Instance.onEnemyDead?.Invoke(TurretTarget,enemyType);
            yield return wait;
            PrizeMoney();
            PoolSignals.Instance.onReleasePoolObject?.Invoke(enemyType.ToString(), gameObject);
        }

        private void PrizeMoney()
        {
            Vector3 position = transform.position;
            for (int i = 0; i < _data.PrizeMoney; i++)
            {
                _money = PoolSignals.Instance.onGetPoolObject?.Invoke(PoolType.Money.ToString(), transform);
                _money.transform.DOLocalJump(
                    new Vector3(position.x + Random.Range(-1f, 1f), 0.5f, position.z + Random.Range(-1f, 1f)), 
                    1f, 3, 0.5f);
            }
        }

        public void IsDeath(){StartCoroutine(Death());}
        
        public void AnimTriggerState(EnemyStates states)
        {
            animator.SetTrigger(states.ToString());
        }
        
        public void AnimBoolState(EnemyStates animState,bool isAttack)
        {
            animator.SetBool(animState.ToString(),isAttack);
        }
    }
}