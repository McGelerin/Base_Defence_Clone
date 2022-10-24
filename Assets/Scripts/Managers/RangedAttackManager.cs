using Controllers;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Signals;
using UnityEngine;

namespace Managers
{
    public class RangedAttackManager : AttackRadius
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private GameObject weaponHolder;
        [SerializeField] private GameObject direct;

        #endregion

        #region Private Variables
        
        private GameObject _weapon;
        private Transform _firePoint;
        private bool _isPlayerOnBase;
        private CD_Weapon _data;
        private WeaponType _selectedWeaponType;
        private WeaponData _selectedWeaponData = new WeaponData();
        #endregion
        
        #endregion
        protected override void Awake()
        {
            _data = GetData();
            base.Awake();
            _isPlayerOnBase = true;
        }

        private CD_Weapon GetData() => Resources.Load<CD_Weapon>("Data/CD_Weapon");
        
        private void SetWeapon()
        {
            _selectedWeaponType = IdleSignals.Instance.onSelectedWeapon();
            _selectedWeaponData = _data.Weapons[_selectedWeaponType];
        }

        #region Event Subscription

        protected override void OnEnable()
        {
            base.OnEnable();
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            AttackSignals.Instance.onGetWeaponDamage += OnGetDamage;
            IdleSignals.Instance.onSelectedWeaponAnimState += OnGetWeaponAnimState;
            IdleSignals.Instance.onSelectedWeaponAttackAnimState += OnGetWeaponAttackAnimState;
            AttackSignals.Instance.onPlayerIsTarget += OnGetTarget;
            AttackSignals.Instance.onGetBulletDirect += OnGetDirect;
            IdleSignals.Instance.onPlayerDeath += OnPlayerInteractBase;
        }

        private void UnsubscribeEvents()
        {
            AttackSignals.Instance.onGetWeaponDamage -= OnGetDamage;
            IdleSignals.Instance.onSelectedWeaponAnimState -= OnGetWeaponAnimState;
            IdleSignals.Instance.onSelectedWeaponAttackAnimState -= OnGetWeaponAttackAnimState;
            AttackSignals.Instance.onPlayerIsTarget -= OnGetTarget;
            AttackSignals.Instance.onGetBulletDirect -= OnGetDirect;
            IdleSignals.Instance.onPlayerDeath -= OnPlayerInteractBase;
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
            AttackDelay = _selectedWeaponData.AttackDelay;
        }

        public void PlayerInteractOutSide()
        {
            if (!_isPlayerOnBase) return;
            TCollider.enabled = true;
            _weapon = PoolSignals.Instance.onGetPoolObject(_selectedWeaponType.ToString(), weaponHolder.transform);
            _weapon.transform.SetParent(weaponHolder.transform);
            _weapon.transform.localRotation = Quaternion.Euler(270,94.6f , 183.9f);
            _firePoint = _weapon.transform.GetChild(0);
            _isPlayerOnBase = false;
        }

        public void OnPlayerInteractBase()
        {
            if (_isPlayerOnBase) return;
            PoolSignals.Instance.onReleasePoolObject?.Invoke(_selectedWeaponType.ToString(),_weapon);
            TCollider.enabled = false;
            if (AttackCoroutine != null)
            {
                StopCoroutine(AttackCoroutine);
                AttackCoroutine = null;
                Enemys.Clear();
                IsRemoveEnemy = true;
                AttackSignals.Instance.onPlayerHasTarget?.Invoke(false);
            }
            _weapon = null;
            _isPlayerOnBase = true;
        }

        protected override void HasTarget()
        {
            AttackSignals.Instance.onPlayerHasTarget?.Invoke(true);
        }

        protected override void RangedAttack()
        {
            PoolSignals.Instance.onGetPoolObject(PoolType.Bullet.ToString(), _firePoint);
        }

        protected override void AttackEnd()
        {
            AttackSignals.Instance.onPlayerHasTarget?.Invoke(false);
        }

        protected override bool TriggerEnter(Collider other)
        {
            if (!_isPlayerOnBase) return false;
            if (other.CompareTag("Enemy"))
            {
                Enemys.Add(other.gameObject);
            }
            return true;
        }

        protected override bool TriggerExit(Collider other)
        {
            if (!_isPlayerOnBase) return false;
            if (other.CompareTag("Enemy"))
            {
                Enemys.Remove(other.gameObject);
            }
            return true;
        }

        private Vector3 OnGetDirect() => direct.transform.forward * _selectedWeaponData.BulletSpeed;
        
        private GameObject OnGetTarget() => TargetEnemy;

        private int OnGetDamage() => _selectedWeaponData.Damage;

        private PlayerAnimState OnGetWeaponAnimState() => _selectedWeaponData.WeaponAnimState;

        private PlayerAnimState OnGetWeaponAttackAnimState() => _selectedWeaponData.WeaponAttackAnimState;
    }
}