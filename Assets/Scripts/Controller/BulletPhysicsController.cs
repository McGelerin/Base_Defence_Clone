using System;
using System.Collections;
using Enums;
using Signals;
using UnityEngine;

namespace Controllers
{
    public class BulletPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables
        
        [SerializeField]private Rigidbody rb;

        #endregion

        #region Private Variables

        private Vector3 _direct;

        #endregion
        #endregion

        private void OnEnable()
        {
            _direct = AttackSignals.Instance.onGetBulletDirect();
            rb.velocity = Vector3.zero;
            rb.AddForce(_direct,ForceMode.VelocityChange);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                PoolSignals.Instance.onReleasePoolObject?.Invoke(PoolType.Bullet.ToString(), gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("WeaponAttackRadius"))
            {
                PoolSignals.Instance.onReleasePoolObject?.Invoke(PoolType.Bullet.ToString(), gameObject);
            }
        }
    }
}