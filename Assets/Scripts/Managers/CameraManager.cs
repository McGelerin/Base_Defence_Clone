using Cinemachine;
using Enums;
using Signals;
using UnityEngine;

namespace Managers
{
    public class CameraManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables
        
        public CameraStates CameraStateController
        {
            get => _cameraStateValue;
            set
            {
                _cameraStateValue = value;
                SetCameraStates();
            }
        }
        
        #endregion
        #region Serialized Variables
        [SerializeField]private CinemachineStateDrivenCamera stateDrivenCamera;

        #endregion

        #region Private Variables
        
        private Vector3 _initialPosition;
        private CameraStates _cameraStateValue = CameraStates.InitializeCam;
        private Animator _camAnimator;
        
        #endregion

        #endregion

        private void Awake()
        {
            GetReferences();
            GetInitialPosition();
        }

        private void GetReferences()
        {
            _camAnimator = GetComponent<Animator>();
        }
        
        #region Event Subscriptions
        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onSetPlayerPosition += OnSetCameraTarget;
            CoreGameSignals.Instance.onPlay += OnPlay;
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onSetPlayerPosition -= OnSetCameraTarget;
            CoreGameSignals.Instance.onPlay -= OnPlay;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion
        
        private void SetCameraStates()
        {
            if (CameraStateController == CameraStates.InitializeCam)
            {
                SetCameraState(CameraStateController);
            }
            else if (CameraStateController == CameraStates.IdleCam)
            {
                _camAnimator.Play(CameraStateController.ToString());
            }
        }
        
        private void GetInitialPosition()
        {
            _initialPosition = transform.GetChild(0).localPosition;
        }

        private void OnMoveToInitialPosition()
        {
            transform.GetChild(0).localPosition = _initialPosition;
        }

        private void OnSetCameraTarget(Transform target)
        {
            stateDrivenCamera.Follow = target;
        }
        
        private void SetCameraState(CameraStates cameraState)
        {
            _camAnimator.SetTrigger(cameraState.ToString());
        }

        private void OnPlay()
        {
            CameraStateController = CameraStates.IdleCam;
        }
        
        private void OnNextLevel()
        {
        }
        private void OnChangeGameStateToIdle()
        {
        }
        private void OnLevelSuccessful()
        {
        }

 

        private void OnReset()
        {
            CameraStateController = CameraStates.InitializeCam;
            stateDrivenCamera.Follow = null;
            stateDrivenCamera.LookAt = null;
            OnMoveToInitialPosition();
        }
    }
}