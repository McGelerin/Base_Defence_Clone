using System.Collections.Generic;
using Controllers;
using Enums;
using Keys;
using Signals;
using TMPro;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables
        [SerializeField] private List<GameObject> panels;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI gemText;

        #endregion

        #region Private Variables
        private UIPanelController _uiPanelController;
        private ScoreDataParams _scoreData;
        
        #endregion

        #endregion

        private void Awake()
        {
            _uiPanelController = new UIPanelController();
        }

        #region Event Subscriptions

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onPlay += OnPlay;
            UISignals.Instance.onOpenPanel += OnOpenPanel;
            UISignals.Instance.onClosePanel += OnClosePanel;
            ScoreSignals.Instance.onSetScoreToUI += OnSetScoreText;
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onPlay -= OnPlay;
            UISignals.Instance.onOpenPanel -= OnOpenPanel;
            UISignals.Instance.onClosePanel -= OnClosePanel;
            ScoreSignals.Instance.onSetScoreToUI -= OnSetScoreText;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void OnOpenPanel(UIPanels panelParam)
        {
            _uiPanelController.OpenPanel(panelParam , panels);
        }

        private void OnClosePanel(UIPanels panelParam)
        {
            _uiPanelController.ClosePanel(panelParam , panels);
        }
        
        private void OnSetScoreText()
        {
            _scoreData = ScoreSignals.Instance.onScoreData();
            moneyText.text = _scoreData.MoneyScore.ToString();
            gemText.text = _scoreData.GemScore.ToString();
        }
        
        private void OnPlay()
        {
            UISignals.Instance.onClosePanel?.Invoke(UIPanels.StartPanel);
            UISignals.Instance.onOpenPanel?.Invoke(UIPanels.IdlePanel);
            UISignals.Instance.onOpenPanel?.Invoke(UIPanels.ScorePanel);
        }

        public void Play()
        {
            CoreGameSignals.Instance.onPlay?.Invoke();
        }

    }
}