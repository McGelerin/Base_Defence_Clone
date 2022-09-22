using System;
using Data.ValueObject;
using Enums;
using Keys;
using Signals;
using UnityEngine;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables

        private ScoreDataParams _loadedScoreDatas;
        private ScoreData _scoreData = new ScoreData();

        #endregion

        #endregion
        
        #region Event Subscriptions

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            SaveSignals.Instance.onSaveScoreData += OnGetSaveScoreData;
            ScoreSignals.Instance.onScoreData += OnGetScoreData;
            ScoreSignals.Instance.onSetScore += OnSetScore;
        }

        private void UnsubscribeEvents()
        {
            SaveSignals.Instance.onSaveScoreData -= OnGetSaveScoreData;
            ScoreSignals.Instance.onScoreData -= OnGetScoreData;
            ScoreSignals.Instance.onSetScore -= OnSetScore;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void Start()
        {
            _loadedScoreDatas = SaveSignals.Instance.onLoadScoreData();
            _scoreData.MoneyScore = _loadedScoreDatas.MoneyScore;
            _scoreData.GemScore = _loadedScoreDatas.GemScore;
            ScoreSignals.Instance.onSetScoreToUI?.Invoke();
        }

        private void OnSetScore(PayTypeEnum scoreType, int score)
        {
            switch (scoreType)
            {
                case PayTypeEnum.Money:
                    _scoreData.MoneyScore += score;
                    break;
                case PayTypeEnum.Gem:
                    _scoreData.GemScore += score;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(scoreType), scoreType, null);
            }
            ScoreSignals.Instance.onSetScoreToUI?.Invoke();
        }
        
        private ScoreDataParams OnGetSaveScoreData()
        {
            return new ScoreDataParams
            {
                MoneyScore = _scoreData.MoneyScore,
                GemScore = _scoreData.GemScore
            };
        }

        private ScoreDataParams OnGetScoreData()
        {
            return new ScoreDataParams
            {
                MoneyScore = _scoreData.MoneyScore,
                GemScore = _scoreData.GemScore
            };
        }
    }
}