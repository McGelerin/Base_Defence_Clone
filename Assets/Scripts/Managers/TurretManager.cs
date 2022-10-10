using System.Collections.Generic;
using Controllers;
using Data.ValueObject;
using Enums;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Managers
{
    public class TurretManager : AttackRadius
    {
        #region Self Variables

        #region Public Variables

        public GameObject Target;
        public List<GameObject> EnemysCache;

        #endregion

        #region Serialized Variables

        [SerializeField] private GameObject ammoStackHolder;
        [SerializeField] private GameObject ammoSpawnPoint;
        [SerializeField] private TurretMovementController movementController;

        #endregion

        #region Private Variables

        [ShowInInspector]private TurretStateEnum _turretState;
        private List<GameObject> _ammoStack = new List<GameObject>();
        private TurretData _data;
        private GameObject _ammoPrefab;

        #endregion

        #endregion

        protected override void Awake()
        {
            GameObject obj = new GameObject();
            EnemysCache = Enemys;
            _ammoStack.Add(obj);
            movementController.SetRotateDelay(0.1f);
            AttackDelay = 0.5f;
            base.Awake();
            //_turretState = TurretStateEnum.None;
        }

        public void SetData(TurretData data)
        {
            //almama gerek kalmayabilir
            _data = data;
        }

        public void HasSolder()
        {
            _turretState = TurretStateEnum.WithBot;
            if (Enemys.Count > 0)
            {
                if (AttackCoroutine == null)
                {
                    AttackCoroutine = StartCoroutine(Attack());
                }
            }
        }

        protected override void RangedAttack()
        {
            _ammoPrefab = PoolSignals.Instance.onGetPoolObject(PoolType.Ammo.ToString(), ammoSpawnPoint.transform);
            _ammoPrefab.GetComponent<AmmoPhysicsController>().SetAddForce(ammoSpawnPoint.transform.forward * 10);
        }
        
        protected override void HasTarget()
        {
            Target = TargetEnemy;
            movementController.LockTarget(true);
        }

        protected override void AttackEnd()
        {
            movementController.LockTarget(false);
        }

        private void AddEnemy(Component other)
        {
            if (other.CompareTag("Enemy"))
            {
                Enemys.Add(other.gameObject);
            }
        }

        private void RemoveEnemy(Component other)
        {
            if (other.CompareTag("Enemy"))
            {
                Enemys.Remove(other.gameObject);
                Enemys.TrimExcess();
            }
        }
        
        protected override void OnTriggerEnter(Collider other)
        {
            if (_turretState != TurretStateEnum.None)
            {
                if (_ammoStack.Count > 0)
                {
                    if (_turretState == TurretStateEnum.WithBot)
                    {
                        base.OnTriggerEnter(other);
                    }
                }
                else
                {
                    AddEnemy(other);
                    if (AttackCoroutine != null)
                    {
                        StopCoroutine(AttackCoroutine);
                        AttackCoroutine = null;
                    }
                }
            }
            else AddEnemy(other);
        }

        protected override void OnTriggerExit(Collider other)
        {
            if (_turretState != TurretStateEnum.None)
            {
                base.OnTriggerExit(other);
            }
            else
            {
                RemoveEnemy(other);
            }
        }
    }
}