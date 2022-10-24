using Managers;
using UnityEngine;

namespace Controllers
{
    public class SoldierAreaPhysicsController : MonoBehaviour
    {
        [SerializeField] private SoldierAreaManager manager;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                manager.PlayerEntrySoldierArea();
            }
        }
    }
}