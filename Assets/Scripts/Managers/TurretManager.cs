using System.Collections.Generic;
using Command;
using Command.StackCommand;
using Controllers;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
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
            StackSignals.Instance.onInteractStackHolder += OnInteractPlayer;
            StackSignals.Instance.onDecreseStackHolder += OnUnInteractPlayer;
        }

        private void UnsubscribeEvents()
        {
            AttackSignals.Instance.onGetAmmoDamage -= OnGetAmmoDamage;
            StackSignals.Instance.onInteractStackHolder -= OnInteractPlayer;
            StackSignals.Instance.onDecreseStackHolder -= OnUnInteractPlayer;
        }

        protected override void OnDisable()
        {
            UnsubscribeEvents();
            base.OnDisable();
        }

        #endregion

        private void OnInteractPlayer(GameObject holder , List<GameObject> ammoStack)
        {
            if (holder == ammoHolder)
            {
                turretAmmoAreaController.AmmoAddToStack(ammoStack);
            }
        }

        private void OnUnInteractPlayer(GameObject holder)
        {
            if (holder == ammoHolder)
            {
                turretAmmoAreaController.PlayerUnInteractAmmoArea();
            }
        }
        
        public void HasSolder()
        {
            _turretState = TurretStateEnum.WithBot;
            if (Enemys.Count <= 0) return;
            AttackCoroutine ??= StartCoroutine(Attack());
        }

        protected override void RangedAttack()
        {
            if (_turretState == TurretStateEnum.None) return;
            if (_ammoStack.Count == 0 )
            {
                movementController.LockTarget(false);
                return;
            }
            if (_turretState == TurretStateEnum.WithBot) movementController.LockTarget(true);
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
            EnemysCache = Enemys;
            movementController.LockTarget(true);
        }

        protected override void AttackEnd()
        {
            movementController.LockTarget(false);
        }
        private int OnGetAmmoDamage() => _data.AmmoDamage;
    }
}