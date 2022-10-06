using System.Collections;
using System.Collections.Generic;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Controllers
{
    public class AttackRadius : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables
        
        public SphereCollider SCollider;
        [ShowInInspector]protected List<GameObject> Enemys = new List<GameObject>();
        protected Coroutine AttackCoroutine;
        protected WeaponType SelectedWeaponType;
        protected WeaponData SelectedWeaponData = new WeaponData();
        [ShowInInspector]protected GameObject TargetEnemy;

        #endregion

        #region Private Variables
        
        private CD_Weapon _data;
        private bool _isRemoveEnemy;
        
        #endregion

        #endregion


        protected virtual void Awake()
        {
            SCollider = GetComponent<SphereCollider>();
            _isRemoveEnemy = true;
            _data = GetData();
        }

        private CD_Weapon GetData() => Resources.Load<CD_Weapon>("Data/CD_Weapon");

        #region Event Subscription

        protected virtual void OnEnable()
        {
            SubscribeEvents();
        }

        protected virtual void SubscribeEvents()
        {
            AttackSignals.Instance.onEnemyDead += OnEnemyDead;
        }

        protected virtual void UnsubscribeEvents()
        {
            AttackSignals.Instance.onEnemyDead -= OnEnemyDead;
        }

        protected virtual void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion
        
        protected void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                Enemys.Add(other.gameObject);
                
                if (AttackCoroutine == null)
                {
                    AttackCoroutine = StartCoroutine(Attack());
                }
            }
        }
        
        protected virtual void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                Enemys.Remove(other.gameObject);
                Enemys.TrimExcess();
                if (other.gameObject == TargetEnemy)
                {
                    //AttackSignals.Instance.onPlayerHasTarget?.Invoke(false);
                    //TargetEnemy = null;
                    _isRemoveEnemy = true;
                    //return;
                }

                if (Enemys.Count != 0) return;
                AttackSignals.Instance.onPlayerHasTarget?.Invoke(false);
                StopCoroutine(AttackCoroutine);
                AttackCoroutine = null;
                _isRemoveEnemy = true;
            }
        }
        
        protected virtual void SetWeapon()
        {
            SelectedWeaponType = IdleSignals.Instance.onSelectedWeapon();
            SelectedWeaponData = _data.Weapons[SelectedWeaponType];
        }

        protected virtual void OnEnemyDead(GameObject enemy)
        {
            Enemys.Remove(enemy);
            Enemys.TrimExcess();
            _isRemoveEnemy = true;
        }
        
        protected virtual IEnumerator Attack()
        {
            WaitForSeconds Wait = new WaitForSeconds(SelectedWeaponData.AttackDelay);
            yield return Wait;

            //TargetEnemy = null;
            
            while (Enemys.Count > 0)
            {
                if (_isRemoveEnemy)
                {
                    float closestDistance = float.MaxValue;
                    for (int i = 0; i < Enemys.Count; i++)
                    {
                        Transform enemyTransform = Enemys[i].transform;
                        float distance = Vector3.Distance(transform.position, enemyTransform.position);
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            TargetEnemy = Enemys[i];
                        }
                    }

                    TargetEnemy = TargetEnemy.gameObject;
                    AttackSignals.Instance.onPlayerHasTarget?.Invoke(true);
                    _isRemoveEnemy = false;
                }
                
                Debug.Log("target bulundu");
                yield return Wait;
            }
            AttackSignals.Instance.onPlayerHasTarget?.Invoke(false);
            TargetEnemy = null;
            _isRemoveEnemy = true;
            AttackCoroutine = null;
        }
    }
}