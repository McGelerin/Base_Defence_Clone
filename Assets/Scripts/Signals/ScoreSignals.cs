using System;
using Enums;
using Extentions;
using Keys;
using UnityEngine.Events;

namespace Signals
{
    public class ScoreSignals : MonoSingleton<ScoreSignals>
    {
        public UnityAction onSetScoreToUI = delegate { };
        public UnityAction<PayTypeEnum,int> onSetScore =delegate {  };
        public Func<ScoreDataParams> onScoreData = delegate { return default;};
    }
}