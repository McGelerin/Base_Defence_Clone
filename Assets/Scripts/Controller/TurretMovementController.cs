using System.Collections;
using Managers;
using Unity.Mathematics;
using UnityEngine;

namespace Controllers
{
    public class TurretMovementController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private TurretManager manager;

        #endregion

        #region Private Variables

        private WaitForSeconds _rotateDelay;
        private bool _isAttack;
        
        #endregion

        #endregion

        public void SetRotateDelay(float rotateDelay)
        {
            _rotateDelay = new WaitForSeconds(rotateDelay);
        }

        public void LockTarget(bool isAttack)
        {
            _isAttack = isAttack;
            if (isAttack)
            {
                StartCoroutine(Rotate());
            }
            else
            {
                StopAllCoroutines();
                DefaultPosition();
            }
        }

        private IEnumerator Rotate()
        {
            while (manager.EnemysCache.Count > 0)
            {
                var transform1 = transform;
                var direct = manager.Target.transform.position - transform1.position;
                transform.rotation = Quaternion.Slerp(transform1.rotation,
                    Quaternion.LookRotation(direct), 0.2f);
                yield return _rotateDelay;
            }
        }

        private void DefaultPosition()
        {
            transform.rotation = Quaternion.Slerp(transform.transform.rotation,
                quaternion.identity, 0.2f);
        }
        
    }
}