using System;
using AIBrain;
using UnityEngine;

namespace Controllers
{
    public class EnemyBodyPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private EnemyAIBrain enemyBrain;

        #endregion

        #endregion
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Bullet"))
            {
                enemyBrain.TakeBulletDamage();
                return;
            }

            if (other.CompareTag("TurretAmmo"))
            {
                enemyBrain.TakeAmmoDamage();
            }
        }
    }
}