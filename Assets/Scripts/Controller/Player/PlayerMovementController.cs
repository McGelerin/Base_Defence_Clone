using Data.ValueObject;
using Keys;
using UnityEngine;

namespace Controller
{
    public class PlayerMovementController : MonoBehaviour
    {
        #region Self Variables
        
        #region Serialized Variables

        //[SerializeField] private PlayerManager manager;
        [SerializeField] private new Rigidbody rigidbody;
        
        #endregion
        
        #region Private Variables
        
        private PlayerMovementData _movementData;
        private bool _isReadyToMove, _isReadyToPlay;
        //private float _inputValue;
        private float _inputValueX;
        private float _inputValueZ;

        private Vector3 _directCache;
      //  private Vector2 _clampValues;
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
            _directCache = new Vector3(velocity.x, 0, velocity.z);
            if (_directCache == Vector3.zero) return;
            //Burasi target geldigi zaman if kosuluna baglanacak
            Rotate();
        }

        private void Rotate()
        {
            var direct = Quaternion.LookRotation(_directCache);
            transform.GetChild(0).transform.rotation = direct;
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