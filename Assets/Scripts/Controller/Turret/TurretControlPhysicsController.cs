using System;
using Managers;
using UnityEngine;

namespace Controllers
{
    public class TurretControlPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private TurretManager manager;
        
        #endregion

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GameObject player = other.transform.parent.gameObject;
                manager.InteractPlayerWithTurret(player);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                manager.UnInteractPlayerWithTurret();
            }
        }
    }
}