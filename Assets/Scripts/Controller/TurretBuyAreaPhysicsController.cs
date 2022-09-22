using Managers;
using UnityEngine;

namespace Controllers
{
    public class TurretBuyAreaPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region SerializeField Variables

        [SerializeField] private TurretManager manager;

        #endregion

        #endregion
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                manager.BuyAreaEnter();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                manager.BuyAreaExit();
            }
        }
    }
}