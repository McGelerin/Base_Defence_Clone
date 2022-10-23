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
using Random = UnityEngine.Random;

namespace AIBrain
{
    public class EnemyAIBrain : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public GameObject TurretTarget;
        public GameObject Target;

        #endregion

        #region SerializField Variables

        [SerializeField] private EnemyType enemyType;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject enemyBody;
        [SerializeField] private float checkTimer;

        #endregion

        #region Private Variables


        private EnemyTypeData _data;
        private GameObject _money;
        private Coroutine _attack;
        private int _health;
        private float _timer;

        #region Status

        private EnemyBaseState _currentState;
        private MoveToTurret _moveToTurret;
        private ChaseToPlayer _chaseToPlayer;
        private ChaseToSoldier _chaseToSoldier;
        private AttackToPlayer _attackToPlayer;
        private AttackToSoldier _attackToSoldier;
        private EnemyDeath _enemyDeath;
        
        #endregion
        #endregion
        #endregion

        private void Awake()
        {
            var brain = this;
            _data = Resources.Load<CD_AI>("Data/Cd_AI").EnemyAIData.EnemyTypeDatas[enemyType];
            _moveToTurret = new MoveToTurret(ref brain, ref agent,ref _data);
            _chaseToPlayer = new ChaseToPlayer(ref brain, ref agent, ref _data);
            _chaseToSoldier = new ChaseToSoldier(ref brain, ref agent, ref _data);
            _attackToPlayer = new AttackToPlayer(ref brain, ref agent, ref _data);
            _attackToSoldier = new AttackToSoldier(ref brain, ref agent, ref _data);
            _enemyDeath = new EnemyDeath(ref brain, ref agent, ref _data);
        }

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
            _health = _data.Health;
            TurretTarget = IdleSignals.Instance.onEnemyTarget();
            _currentState = _moveToTurret;
            _currentState.EnterState();
        }

        private void SubscribeEvents()
        {
            AttackSignals.Instance.onSoldierDeath += OnSoldierDeath;
        }

        private void UnsubscribeEvents()
        {
            AttackSignals.Instance.onSoldierDeath -= OnSoldierDeath;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            UnsubscribeEvents();
            AttackSignals.Instance.onEnemyDead?.Invoke(enemyBody);
        }

        #endregion
        
        private void Update()
        {
            _timer += Time.deltaTime;
            if (!(_timer >= checkTimer)) return;
            _currentState.UpdateState();
            _timer = 0;
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
                EnemyStates.MoveToTurret => _moveToTurret,
                EnemyStates.ChaseToPlayer => _chaseToPlayer,
                EnemyStates.ChaseToSoldier => _chaseToSoldier,
                EnemyStates.AttackToPlayer => _attackToPlayer,
                EnemyStates.AttackToSoldier => _attackToSoldier,
                EnemyStates.EnemyDeath => _enemyDeath,
                _ => _currentState
            };
            _currentState.EnterState();
        }

        public void AttackToPlayerStatus(bool isAttack)
        {
            if (isAttack)
            {
                _attack = StartCoroutine(AttackToPlayer(true));
            }
            else
            {
                if (_attack == null) return;
                StopCoroutine(_attack);
                _attack = null;
            }
        }

        public void AttackToSoldierStatus(bool isAttack)
        {
            if (isAttack)
            {
                _attack = StartCoroutine(AttackToPlayer(false));
            }
            else
            {
                if (_attack == null) return;
                StopCoroutine(_attack);
                _attack = null;
            }
        }

        public void TakeBulletDamage()
        {
            _health -= AttackSignals.Instance.onGetWeaponDamage();
        }
        public void TakeAmmoDamage()
        {
            _health -= AttackSignals.Instance.onGetAmmoDamage();
        }

        public void TakeSoldierDamage()
        {
            _health -= AttackSignals.Instance.onGetSoldierDamage();
        }

        public bool HealthCheck()
        {
            return _health <= 0;
        }

        private IEnumerator AttackToPlayer(bool isPlayer)
        {
            WaitForSeconds wait = new WaitForSeconds(1.1f);
            while (true)
            {
                AnimTriggerState(EnemyAnimState.Attack);
                yield return wait;
                if (isPlayer)
                {
                    AttackSignals.Instance.onGiveDamageToPlayer?.Invoke(_data.Damage);
                }
                else
                {
                    AttackSignals.Instance.onGiveDamegeToSoldier?.Invoke(Target,_data.Damage);
                }
            }
        }
        
        private IEnumerator Death()
        {
            WaitForSeconds wait = new WaitForSeconds(1f);
            AnimBoolState(EnemyAnimState.Death , true);
            IdleSignals.Instance.onEnemyDead?.Invoke(TurretTarget,enemyType);
            yield return wait;
            PrizeMoney();
            AttackSignals.Instance.onEnemyDead?.Invoke(enemyBody);
            yield return new WaitForSeconds(0.1f);
            PoolSignals.Instance.onReleasePoolObject?.Invoke(enemyType.ToString(), gameObject);
        }

        private void PrizeMoney()
        {
            Vector3 position = transform.position;
            for (int i = 0; i < _data.PrizeMoney; i++)
            {
                _money = PoolSignals.Instance.onGetPoolObject?.Invoke(PoolType.Money.ToString(), transform);
                _money.transform.DOLocalJump(
                    new Vector3(position.x + Random.Range(-1f, 1f), 0.5f, position.z + Random.Range(0f, 1f)), 
                    1f, 3, 0.5f);
            }
        }

        private void OnSoldierDeath(GameObject soldier)
        {
            if (soldier == Target)
            {
                AttackToSoldierStatus(false);
                SwitchState(EnemyStates.MoveToTurret);
            }
        }

        public void IsDeath(){StartCoroutine(Death());}
        
        public void AnimTriggerState(EnemyAnimState states)
        {
            animator.SetTrigger(states.ToString());
        }
        
        public void AnimBoolState(EnemyAnimState animState,bool isAttack)
        {
            animator.SetBool(animState.ToString(),isAttack);
        }
    }
}