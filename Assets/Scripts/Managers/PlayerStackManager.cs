using System.Collections.Generic;
using Signals;
using UnityEngine;

namespace Managers
{
    public class PlayerStackManager : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables

        private List<GameObject> _hostageList = new List<GameObject>();

        #endregion

        #endregion

        #region Event Subscription

        private void OnEnable()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            StackSignals.Instance.onGetHostageTarget += OnGetHostageTarget;
        }


        private void Unsubscribe()
        {
            StackSignals.Instance.onGetHostageTarget -= OnGetHostageTarget;
        }
        
        private void OnDisable()
        {
            Unsubscribe();
        }

        #endregion
        
        
        private GameObject OnGetHostageTarget(GameObject hostage)
        {
            if (_hostageList.Count == 0)
            {
                _hostageList.Add(hostage);
                return transform.gameObject;
            }
            _hostageList.Add(hostage);
            return _hostageList[_hostageList.Count - 2];
        }
    }
}