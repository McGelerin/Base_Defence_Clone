using Signals;
using UnityEngine;

namespace Controllers
{
    public class MoneyController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private BoxCollider boxCollider;

        #endregion

        #endregion

        private void OnEnable()
        {
            WorkerSignals.Instance.onAddListToMoney?.Invoke(gameObject);
        }
        
        private void OnDisable()
        {
            transform.rotation = Quaternion.identity;
            boxCollider.enabled = true;
        }
    }
}