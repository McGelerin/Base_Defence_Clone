using System;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Signals;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Managers
{
    public class RangedAttackManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables



        #endregion

        #region Serialized Variables

        [SerializeField] private GameObject weaponHolder;
        
        #endregion

        #region Private Variables
        
        private WeaponType _selectedWeaponType;
        [ShowInInspector]private WeaponData _selectedWeaponData = new WeaponData();
        private GameObject _weapon;
        private bool _isPlayerOnBase;
        [ShowInInspector]private CD_Weapon _data;

        #endregion

        #endregion
        
        private void Awake()
        {
            _data = GetData();
            _isPlayerOnBase = true;
        }
        private CD_Weapon GetData() => Resources.Load<CD_Weapon>("Data/CD_Weapon");

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            AttackSignals.Instance.onGetWeaponDamage += OnGetDamage;
            IdleSignals.Instance.onSelectedWeaponAnimState += OnGetWeaponAnimState;
            IdleSignals.Instance.onSelectedWeaponAttackAnimState += OnGetWeaponAttackAnimState;
        }

        private void UnsubscribeEvents()
        {
            AttackSignals.Instance.onGetWeaponDamage -= OnGetDamage;
            IdleSignals.Instance.onSelectedWeaponAnimState -= OnGetWeaponAnimState;
            IdleSignals.Instance.onSelectedWeaponAttackAnimState -= OnGetWeaponAttackAnimState;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void Start()
        {
            SetWeapon();
        }

        private void SetWeapon()
        {
            _selectedWeaponType = IdleSignals.Instance.onSelectedWeapon();
            _selectedWeaponData = _data.Weapons[_selectedWeaponType];
        }

        public void PlayerIncreaseOutSide()
        {
            if (_isPlayerOnBase)
            {
                _weapon = PoolSignals.Instance.onGetPoolObject(_selectedWeaponType.ToString(), weaponHolder.transform);
                _weapon.transform.SetParent(weaponHolder.transform);
                _weapon.transform.localRotation = Quaternion.Euler(-95, -230, 145);
                _isPlayerOnBase = false;
            }
        }

        public void PlayerIncreaseBase()
        {
            if (!_isPlayerOnBase)
            {
                PoolSignals.Instance.onReleasePoolObject?.Invoke(_selectedWeaponType.ToString(),_weapon);
                _weapon = null;
                _isPlayerOnBase = true;
            }
        }

        private int OnGetDamage()
        {
            return _selectedWeaponData.Damage;
        }

        private PlayerAnimState OnGetWeaponAnimState()
        {
            return _selectedWeaponData.WeaponAnimState;
        }
        
        private PlayerAnimState OnGetWeaponAttackAnimState()
        {
            return _selectedWeaponData.WeaponAttackAnimState;
        }
    }
}