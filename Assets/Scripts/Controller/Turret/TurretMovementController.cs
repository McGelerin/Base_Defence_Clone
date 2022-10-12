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
        private Coroutine rotate;

        #endregion

        #endregion

        public void SetRotateDelay(float rotateDelay)
        {
            _rotateDelay = new WaitForSeconds(rotateDelay);
        }

        public void LockTarget(bool isAttack)
        {
            if (isAttack)
            {
                rotate ??= StartCoroutine(Rotate());
            }
            else
            {
                if (rotate != null)
                {
                    StopCoroutine(rotate);
                    rotate = null;
                }
                DefaultPosition();
            }
        }

        private IEnumerator Rotate()
        {
            while (manager.EnemysCache.Count > 0)
            {
                var direct = manager.Target.transform.position - transform.position;
                var lookRotation = Quaternion.LookRotation(direct,Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    lookRotation, 0.2f);
                yield return _rotateDelay;
            }
        }

        private void DefaultPosition()
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.identity, 0.5f);
        }
    }
}