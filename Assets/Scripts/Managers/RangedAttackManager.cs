using Controllers;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Managers
{
    public class RangedAttackManager : AttackRadius
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private GameObject weaponHolder;
        
        #endregion

        #region Private Variables
        
        private GameObject _weapon;
        private bool _isPlayerOnBase;


        #endregion

        #endregion

        protected override void Awake()
        {
            base.Awake();
            _isPlayerOnBase = true;
        }

        #region Event Subscription

        protected override void OnEnable()
        {
            base.OnEnable();
            SubscribeEvents();
        }

        protected override void SubscribeEvents()
        {
            AttackSignals.Instance.onGetWeaponDamage += OnGetDamage;
            IdleSignals.Instance.onSelectedWeaponAnimState += OnGetWeaponAnimState;
            IdleSignals.Instance.onSelectedWeaponAttackAnimState += OnGetWeaponAttackAnimState;
            AttackSignals.Instance.onPlayerIsTarget += OnGetTarget;
        }

        protected override void UnsubscribeEvents()
        {
            AttackSignals.Instance.onGetWeaponDamage -= OnGetDamage;
            IdleSignals.Instance.onSelectedWeaponAnimState -= OnGetWeaponAnimState;
            IdleSignals.Instance.onSelectedWeaponAttackAnimState -= OnGetWeaponAttackAnimState;
            AttackSignals.Instance.onPlayerIsTarget -= OnGetTarget;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            UnsubscribeEvents();
        }

        #endregion

        private void Start()
        {
            SetWeapon();
        }

        public void PlayerIncreaseOutSide()
        {
            if (!_isPlayerOnBase) return;
            _weapon = PoolSignals.Instance.onGetPoolObject(SelectedWeaponType.ToString(), weaponHolder.transform);
            
            _weapon.transform.SetParent(weaponHolder.transform);
            _weapon.transform.localRotation = Quaternion.Euler(-95, -230, 145);
            _isPlayerOnBase = false;
        }

        public void PlayerIncreaseBase()
        {
            if (_isPlayerOnBase) return;
            PoolSignals.Instance.onReleasePoolObject?.Invoke(SelectedWeaponType.ToString(),_weapon);
            _weapon = null;
            _isPlayerOnBase = true;
        }

        private GameObject OnGetTarget() => TargetEnemy;

        private int OnGetDamage() => SelectedWeaponData.Damage;

        private PlayerAnimState OnGetWeaponAnimState() => SelectedWeaponData.WeaponAnimState;

        private PlayerAnimState OnGetWeaponAttackAnimState() => SelectedWeaponData.WeaponAttackAnimState;
    }
}