using System;
using Signals;
using UnityEngine;

namespace Controller
{
    public class SoldierAttackController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                WorkerSignals.Instance.onSoldierAttack?.Invoke();
            }
        }
    }
}