using System.Collections;
using Keys;
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
        private float _turretRotation;
        private bool _playerUseIt;
        private Coroutine _rotate;

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
                _rotate ??= StartCoroutine(Rotate());
            }
            else
            {
                if (_rotate != null)
                {
                    StopCoroutine(_rotate);
                    _rotate = null;
                    DefaultPosition();
                }
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

        public void SetTurnValue(IdleInputParams data)
        {
            _turretRotation += data.ValueX;
            _turretRotation = Mathf.Clamp(_turretRotation, -35, 35);
            if (_turretRotation > -35 && _turretRotation < 35)
            {
                transform.rotation = Quaternion.Slerp(Quaternion.Euler(0, transform.rotation.y, 0), 
                    Quaternion.Euler(0,_turretRotation, 1f),0.5f);
            }
        }
        
        private void DefaultPosition()
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.identity, 0.5f);
        }
    }
}