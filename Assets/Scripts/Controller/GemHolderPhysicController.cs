using System;
using Managers;
using UnityEngine;

namespace Controllers
{
    public class GemHolderPhysicController : MonoBehaviour
    {
        #region Self Variables

        #region SerializeField Variables

        [SerializeField] private MineAreaManager manager;

        #endregion

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                manager.PlayerTriggerEnter(other.transform.parent);
                Debug.Log("triggerlandı");
            }
        }
    }
}