using System;
using System.Collections.Generic;
using Command;
using Command.StackCommand;
using Controllers;
using Data.UnityObject;
using Data.ValueObject;
using DG.Tweening;
using Enums;
using Keys;
using Signals;
using UnityEngine;

namespace Managers
{
    public class TurretManager : AttackRadius
    {
        #region Self Variables

        #region Public Variables

        public StaticStackItemPosition ItemPosition;
        public StaticItemAddOnStack AddOnStack;
        [HideInInspector]public GameObject Target;
        [HideInInspector]public List<GameObject> EnemysCache;

        #endregion

        #region Serialized Variables
        
        [SerializeField] private GameObject ammoSpawnPoint;
        [SerializeField] private GameObject turretOperator;
        [SerializeField] private GameObject ammoHolder;
        [SerializeField] private TurretMovementController movementController;
        [SerializeField] private TurretAmmoAreaController turretAmmoAreaController;

        #endregion

        #region Private Variables

        private List<GameObject> _ammoStack = new List<GameObject>();
        private TurretStateEnum _turretState;
        private GameObject _ammoPrefab;
        private TurretData _data;
        private int _ammoCache;

        #endregion

        #endregion

        protected override void Awake()
        {
            _ammoCache = 0;
            EnemysCache = Enemys;
            _data = GetTurretData();
            ItemPosition = new StaticStackItemPosition(ref _ammoStack,ref _data.TurretStackData, ref ammoHolder);
            AddOnStack = new StaticItemAddOnStack(ref _ammoStack, ref _data.TurretStackData, ref ammoHolder);
            AttackDelay = _data.AttackDelay;
            turretAmmoAreaController.SetData(_data.TurretStackData,_ammoStack);
            movementController.SetRotateDelay(_data.RotateDelay);
            base.Awake();
        }

        private TurretData GetTurretData() => Resources.Load<CD_Turret>("Data/CD_Turret").TurretData;

        #region Event Subscription

        protected override void OnEnable()
        {
            SubscribeEvents();
            base.OnEnable();
        }

        private void SubscribeEvents()
        {
            AttackSignals.Instance.onGetAmmoDamage += OnGetAmmoDamage;
            StackSignals.Instance.onInteractStackHolder += OnInteractPlayerWithAmmoArea;
            StackSignals.Instance.onDecreseStackHolder += OnUnInteractPlayerWithAmmoArea;
            InputSignals.Instance.onJoystickDragged += OnSetInputValue;
        }

        private void UnsubscribeEvents()
        {
            AttackSignals.Instance.onGetAmmoDamage -= OnGetAmmoDamage;
            StackSignals.Instance.onInteractStackHolder -= OnInteractPlayerWithAmmoArea;
            StackSignals.Instance.onDecreseStackHolder -= OnUnInteractPlayerWithAmmoArea;
            InputSignals.Instance.onJoystickDragged -= OnSetInputValue;
        }

        protected override void OnDisable()
        {
            UnsubscribeEvents();
            base.OnDisable();
        }

        #endregion
        
        private void OnInteractPlayerWithAmmoArea(GameObject holder , List<GameObject> ammoStack)
        {
            if (holder != ammoHolder) return;
            turretAmmoAreaController.AmmoAddToStack(ammoStack);
        }

        private void OnUnInteractPlayerWithAmmoArea(GameObject holder)
        {
            if (holder != ammoHolder) return;
            turretAmmoAreaController.PlayerUnInteractAmmoArea();
        }
        
        public void HasSolder()
        {
            turretOperator.SetActive(true);
            _turretState = TurretStateEnum.WithBot;
            if (Enemys.Count <= 0) return;
            AttackCoroutine ??= StartCoroutine(Attack());
        }

        protected override void RangedAttack()
        {
            switch (_turretState)
            {
                case TurretStateEnum.None:
                    return;                
                case TurretStateEnum.WithBot:
                    movementController.LockTarget(_ammoStack.Count != 0);
                    break;
                case TurretStateEnum.WithPlayer:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (_ammoStack.Count == 0) return;
            Fire();
        }
        
        private void Fire()
        {
            _ammoPrefab = PoolSignals.Instance.onGetPoolObject(PoolType.Ammo.ToString(), ammoSpawnPoint.transform);
            _ammoPrefab.GetComponent<AmmoPhysicsController>().SetAddForce(transform.forward * 10);
            _ammoPrefab.transform.rotation = transform.rotation;
            _ammoCache++;
            if (_ammoCache != 4) return;
            turretAmmoAreaController.DecreaseStackList();
            _ammoCache = 0;
        }
        
        protected override void HasTarget()
        {
            Target = TargetEnemy;
        }

        protected override void AttackEnd()
        {
            movementController.LockTarget(false);
        }
        private int OnGetAmmoDamage() => _data.AmmoDamage;
        
        protected override bool TriggerEnter(Collider other)
        {
            if (_turretState != TurretStateEnum.None) return false;
            if (other.CompareTag("Enemy"))
            {
                Enemys.Add(other.gameObject);
            }
            return true;
        }

        protected override bool TriggerExit(Collider other)
        {
            if (_turretState != TurretStateEnum.None) return false;
            if (other.CompareTag("Enemy"))
            {
                Enemys.Remove(other.gameObject);
            }
            return true;
        }

        private void OnSetInputValue(IdleInputParams input)
        {
            if (_turretState != TurretStateEnum.WithPlayer) return;
            movementController.SetTurnValue(input);
        }
        
        public void InteractPlayerWithTurret(GameObject player)
        {
            if (_turretState == TurretStateEnum.WithBot) return;
            _turretState = TurretStateEnum.WithPlayer;
            player.transform.SetParent(transform);
            player.transform.DOLocalMove(turretOperator.transform.localPosition, 0.1f).OnComplete(() =>
            {
                player.transform.GetChild(0).rotation = transform.rotation;
                IdleSignals.Instance.onInteractPlayerWithTurret?.Invoke();
            });
            AttackCoroutine ??= StartCoroutine(Attack());
        }

        public void UnInteractPlayerWithTurret()
        {
            if (_turretState != TurretStateEnum.WithPlayer) return;
            _turretState = TurretStateEnum.None;
        }
    }
}