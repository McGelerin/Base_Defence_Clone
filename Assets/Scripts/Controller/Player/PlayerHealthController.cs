using System.Collections;
using Enums;
using Managers;
using TMPro;
using UnityEngine;

namespace Controller.Player
{
    public class PlayerHealthController : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables
        

        #endregion

        #region Serialized Variables

        [SerializeField] private PlayerManager manager;
        [SerializeField] private TextMeshPro tmp;
        [SerializeField] private GameObject healthBar;
        [SerializeField] private GameObject healthBarStatus;


        #endregion

        #region Private Variables

        private float _health;
        private float _currentHealth;
        private Coroutine _healthReload;

        #endregion

        #endregion
        
        public void GetHealth(int health)
        {
            _health = health;
            _currentHealth = health;
            SetText();
            SetHealthBar();
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            if (_currentHealth > 0)
            {
                SetHealthBar();
                SetText();
            }
            else
            {
                healthBar.SetActive(false);
                manager.SetPlayerState(PlayerStateEnum.Death);
            }
        }

        public void PlayerInTheBase(bool isBase)
        {
            if (isBase)
            {
                _healthReload = StartCoroutine(HealthReload());
            }
            else
            {
                healthBar.SetActive(true);
                SetText();
                if (_healthReload == null) return;
                StopCoroutine(_healthReload);
                _healthReload = null;
            }
        }

        private IEnumerator HealthReload()
        {
            WaitForSeconds wait = new WaitForSeconds(0.2f);
            while (_currentHealth < _health)
            {
                _currentHealth++;
                SetHealthBar();
                SetText();
                yield return wait;
            }
            healthBar.SetActive(false);
            _healthReload = null;
        }
        
        private void SetHealthBar()
        {
            var scale = (_currentHealth / _health);
            healthBarStatus.transform.localScale = new Vector3(scale,1,1);
        }

        private void SetText()
        {
            var scale = (int)((_currentHealth / _health) * 100);
            tmp.SetText(scale.ToString());
        }
    }
}