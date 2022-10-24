using System.Collections;
using System.Collections.Generic;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Controllers
{
    public class AttackRadius : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables
        
        public Collider TCollider;

        #endregion

        #region Protected Variables

        [ShowInInspector]protected List<GameObject> Enemys = new List<GameObject>();
        protected float AttackDelay;
        protected Coroutine AttackCoroutine;
        protected bool IsRemoveEnemy; 
        protected GameObject TargetEnemy;
        
        #endregion
        #endregion
        
        protected virtual void Awake()
        {
            IsRemoveEnemy = true;
        }
        
        #region Event Subscription

        protected virtual void OnEnable()
        {
            IsRemoveEnemy = true;
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            AttackSignals.Instance.onEnemyDead += OnEnemyDead;
        }

        private void UnsubscribeEvents()
        {
            AttackSignals.Instance.onEnemyDead -= OnEnemyDead;
        }

        protected virtual void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion
        
        private void OnTriggerEnter(Collider other)
        {
            if (TriggerEnter(other))
            {
                return;
            }
            if (other.CompareTag("Enemy"))
            {
                Enemys.Add(other.gameObject);
                AttackCoroutine ??= StartCoroutine(Attack());
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (TriggerExit(other))
            {
                return;
            }

            if (other.CompareTag("Enemy"))
            {
                Enemys.Remove(other.gameObject);
                Enemys.TrimExcess();
                if (other.gameObject == TargetEnemy)
                {
                    IsRemoveEnemy = true;
                }
                if (Enemys.Count != 0) return;
                if (AttackCoroutine == null) return;
                AttackEnd();
                StopCoroutine(AttackCoroutine);
                AttackCoroutine = null;
                IsRemoveEnemy = true;
            }
        }
        
        private void OnEnemyDead(GameObject enemy)
        {
            if (!Enemys.Contains(enemy)) return;
            Enemys.Remove(enemy);
            if (enemy == TargetEnemy)
            {
                IsRemoveEnemy = true;
            }
        }
        
        protected IEnumerator Attack()
        {
            WaitForSeconds Wait = new WaitForSeconds(AttackDelay);
            yield return Wait;

            while (Enemys.Count > 0)
            {
                if (IsRemoveEnemy)
                {
                    var closestDistance = float.MaxValue;
                    foreach (var t in Enemys)
                    {
                        var enemyTransform = t.transform;
                        var distance = Vector3.Distance(transform.position, enemyTransform.position);
                        if (!(distance < closestDistance)) continue;
                        closestDistance = distance;
                        TargetEnemy = t;
                    }

                    TargetEnemy = TargetEnemy.gameObject;
                    HasTarget();
                    IsRemoveEnemy = false;
                }
                RangedAttack();
                yield return Wait;
            }

            AttackEnd();
            //TargetEnemy = null;
            IsRemoveEnemy = true;
            AttackCoroutine = null;
        }
        protected virtual void RangedAttack() { }
        protected virtual void AttackEnd() { }
        protected virtual void HasTarget(){ }
        protected virtual bool TriggerEnter(Collider other){ return false;}
        protected virtual bool TriggerExit(Collider other){ return false;}
    }
}