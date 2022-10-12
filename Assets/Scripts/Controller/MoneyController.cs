using System;
using UnityEngine;

namespace Controllers
{
    public class MoneyController : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables



        #endregion

        #region Serialized Variables

        [SerializeField] private BoxCollider boxCollider;

        #endregion

        #region Private Variables



        #endregion

        #endregion

        private void OnDisable()
        {
            transform.rotation = Quaternion.identity;
            boxCollider.enabled = true;
        }
    }
}