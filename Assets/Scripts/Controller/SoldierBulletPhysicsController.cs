using Enums;
using Signals;
using UnityEngine;

namespace Controller
{
    public class SoldierBulletPhysicsController : MonoBehaviour
    {
        #region Self Variables
        
        #region Serialized Variables

        [SerializeField]
        private Rigidbody rb;

        #endregion

        #endregion

        private void OnDisable()
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
                PoolSignals.Instance.onReleasePoolObject?.Invoke(PoolType.SoldierBullet.ToString(), gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("SoldierAttackRadius"))
            {
                PoolSignals.Instance.onReleasePoolObject?.Invoke(PoolType.SoldierBullet.ToString(), gameObject);
            }
        }
    }
}