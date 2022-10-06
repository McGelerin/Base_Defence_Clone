using Data.ValueObject;
using Keys;
using Signals;
using Unity.Mathematics;
using UnityEngine;

namespace Controller
{
    public class PlayerMovementController : MonoBehaviour
    {
        #region Self Variables
        
        #region Serialized Variables
        
        [SerializeField] private new Rigidbody rigidbody;
        
        #endregion
        
        #region Private Variables
        
        private PlayerMovementData _movementData;
        private bool _isReadyToMove, _isReadyToPlay,_lockTarget;
        private float _inputValueX;
        private float _inputValueZ;
        private GameObject _target;

        private Vector3 _directCache;
        private bool _isIdle = true;
        
        #endregion
        
        #endregion

        public void SetMovementData(PlayerMovementData dataMovementData)
        {
            _movementData = dataMovementData;
        }

        public void UpdateIdleInputValue(IdleInputParams inputParams)
        {
            _inputValueX = inputParams.ValueX;
            _inputValueZ = inputParams.ValueZ;
        }

        public void IsLockTarget(bool lockTarget)
        {
            _lockTarget = lockTarget;
            if (lockTarget)
            {
                _target = AttackSignals.Instance.onPlayerIsTarget();
            }
        }

        public void IsReadyToPlay(bool state)
        {
            _isReadyToPlay = state;
        }
        
        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            IdleMove();
        }
        
        private void IdleMove()
        {
            var velocity = rigidbody.velocity;
            velocity = new Vector3(_inputValueX * _movementData.PlayerJoystickSpeed, velocity.y,
                _inputValueZ*_movementData.PlayerJoystickSpeed);
            rigidbody.velocity = velocity;
            if (!_lockTarget)
            {
                _directCache = new Vector3(velocity.x, 0, velocity.z);
                if (_directCache == Vector3.zero) return;
                Rotate();
            }
            else
            {
                LockTarget();
            }
        }

        private void Rotate()
        {
            var direct = Quaternion.LookRotation(_directCache);
            transform.GetChild(0).transform.rotation = direct;
        }

        private void LockTarget()
        {
            var direct = _target.transform.position - transform.GetChild(0).transform.position;
            transform.GetChild(0).transform.rotation = Quaternion.Slerp(transform.GetChild(0).transform.rotation,
                Quaternion.LookRotation(direct), 0.2f);
        }

        private void Stop()
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
        
        public void OnReset()
        {
            Stop();
            _isReadyToPlay = false;
            _isReadyToMove = false;
        }
    }
}