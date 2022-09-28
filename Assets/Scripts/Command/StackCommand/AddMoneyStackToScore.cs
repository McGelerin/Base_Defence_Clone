using System.Collections.Generic;
using DG.Tweening;
using Enums;
using Signals;
using UnityEngine;

namespace Command.StackCommand
{
    public class AddMoneyStackToScore
    {
        #region Self Variables

        #region Private Variables

        private List<GameObject> _stackList;
        private GameObject _stackHolder;

        #endregion

        #endregion

        public AddMoneyStackToScore(ref List<GameObject> stackList,ref GameObject stackHolder)
        {
            _stackList = stackList;
            _stackHolder = stackHolder;
        }

        public void Execute()
        {
            for (int i = 0; i < _stackList.Count; i++)
            {
                var money = _stackList[i];
                money.transform.SetParent(_stackHolder.transform.parent);
                money.transform.DOLocalMove(
                    money.transform.localPosition + new Vector3(Random.Range(-3f, 3f), 0.5f, Random.Range(-3f, 3f)), 0.5f);
                money.transform.DOLocalMove(Vector3.zero, 0.5f).SetDelay(0.5f).OnComplete(() =>
                {
                    PoolSignals.Instance.onReleasePoolObject(PoolType.Money.ToString(), money);
                });
            }

            ScoreSignals.Instance.onSetScore(PayTypeEnum.Money, _stackList.Count*5);
            SaveSignals.Instance.onScoreSave?.Invoke();
            _stackList.Clear();
        }
    }
}