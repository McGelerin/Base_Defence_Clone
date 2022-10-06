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

        [SerializeField] private float bulletClosingTimer;
        [SerializeField] private Rigidbody rb;

        #endregion

        #region Private Variables

        private WaitForSeconds _wait;
        private Vector3 _direct;

        #endregion
        #endregion

        private void Awake()
        {
            _wait = new WaitForSeconds(bulletClosingTimer);
        }

        private void OnEnable()
        {
            StartCoroutine(BulletCloser());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void BulletMove()
        {
            _direct = AttackSignals.Instance.onGetBulletDirect();
            rb.AddForce(_direct,ForceMode.VelocityChange);
        }
        
        private IEnumerator BulletCloser()
        {
            BulletMove();
            yield return bulletClosingTimer;
            PoolSignals.Instance.onReleasePoolObject?.Invoke(PoolType.Bullet.ToString(), gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            PoolSignals.Instance.onReleasePoolObject?.Invoke(PoolType.Bullet.ToString(), gameObject);
        }
    }
}