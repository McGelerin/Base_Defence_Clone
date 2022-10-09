using System;
using AIBrain;
using UnityEngine;

namespace Controllers
{
    public class EnemyBodyPhysichsController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private EnemyAIBrain enemyBrain;

        #endregion

        #endregion
        private void OnTriggerEnter(Collider other)
        {
            enemyBrain.TakeDamage();
        }
    }
}