using System;
using Managers;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Controllers
{
    public class MineAreaPhysicsController : MonoBehaviour
    {
        [SerializeField] private MineAreaManager manager;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                manager.PlayerEntryGemArea();
            }
        }
    }
}