using System;
using Enums;
using Signals;
using UnityEngine;

namespace Controllers
{
    public class AmmoPhysicsController : MonoBehaviour
    {
        #region Self Variables
        
        #region Serialized Variables

        [SerializeField]
        private Rigidbody rb;

        #endregion

        #endregion

        private void OnEnable()
        {
            rb.velocity = Vector3.zero;
        }

        public void SetAddForce(Vector3 direct)
        {
            rb.AddForce(direct,ForceMode.VelocityChange);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                PoolSignals.Instance.onReleasePoolObject?.Invoke(PoolType.Ammo.ToString(), gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("WeaponAttackRadius"))
            {
                PoolSignals.Instance.onReleasePoolObject?.Invoke(PoolType.Ammo.ToString(), gameObject);
            }
        }
    }
}