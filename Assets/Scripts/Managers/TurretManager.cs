using Data.ValueObject;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class TurretManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables
        
        public int PayedAmound
        {
            get => _payedAmound;
            set
            {
                _payedAmound = value;
                if (_turretData.Cost - _payedAmound <=0)
                {
                    
                }
                else
                {
                    SetText();
                }
            }
        }

        #endregion

        #region SerializeField Variables

        [SerializeField] private TextMeshPro tmp;

        #endregion
        
        #region Private Variables

        private TurretData _turretData;
        private int _payedAmound;
        private int _remainingAmound;

        #endregion

        #endregion

        public void SetTurretData(TurretData roomData,int payedAmound)
        {
            _turretData = roomData;
            PayedAmound = payedAmound;
        }

        private void SetText()
        {
            _remainingAmound = _turretData.Cost - _payedAmound;
            tmp.text = _remainingAmound.ToString();
        }
    }
}