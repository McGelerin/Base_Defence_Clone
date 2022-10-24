using Managers;
using UnityEngine;

namespace Controller
{
    public class MoneyWorkerBuyAreaPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region SerializeField Variables

        [SerializeField] private MoneyWorkerBuyAreaManager areaManager;

        #endregion

        #endregion
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                areaManager.BuyAreaEnter();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                areaManager.BuyAreaExit();
            }
        }
    }
}