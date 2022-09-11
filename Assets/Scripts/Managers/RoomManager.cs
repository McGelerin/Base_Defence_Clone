using Data.ValueObject;
using Enums;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace Managers
{
    public class RoomManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables
        
        public int PayedAmound
        {
            get => _payedAmound;
            set
            {
                _payedAmound = value;
                if (_roomData.Cost - _payedAmound >=0)
                {
                    area.SetActive(true);
                    fencles.SetActive(false);
                    invisibleWall.SetActive(false);
                    tmp.transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    SetText();
                }
            }
        }

        #endregion

        #region SerializeField Variables

        [SerializeField] private GameObject area;
        [SerializeField] private GameObject fencles;
        [SerializeField] private GameObject invisibleWall;
        [SerializeField] private TextMeshPro tmp;

        #endregion
        
        #region Private Variables

        [ShowInInspector]private RoomData _roomData;
        private int _payedAmound;
        private int _remainingAmound;

        #endregion

        #endregion

        public void SetRoomData(RoomData roomData,int payedAmound)
        {
            _payedAmound = payedAmound;
            _roomData = roomData;
            SetText();
        }

        private void SetText()
        {
            _remainingAmound = _roomData.Cost - _payedAmound;
            tmp.text = _remainingAmound.ToString();
        }
    }
}