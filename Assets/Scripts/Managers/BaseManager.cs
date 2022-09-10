using System;
using System.Collections;
using System.Collections.Generic;
using Data.ValueObject;
using Enums;
using UnityEngine;

namespace Managers
{
    public class BaseManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public BaseStage BaseStageStatus
        {
            get => baseStage; 
            set
            {
                baseStage = value;
            }
        }

        #endregion

        #region Serializefield Variables

        [SerializeField] private GameObject backDoor;
        [SerializeField] private BaseStage baseStage;
        [SerializeField] private List<BaseObjects> level = new List<BaseObjects>();

        #endregion

        #endregion

        private void Awake()
        {
            BaseStageStatus = baseStage;
            ColoseGameObjects();
        }

        private void ColoseGameObjects()
        {
            foreach (var VARIABLE in level[(int)BaseStageStatus].GameObjects)
            {
                VARIABLE.SetActive(false);
            }
        }
    }
}
