using Extentions;
using UnityEngine.Events;

namespace Signals
{
    public class IdleSignals : MonoSingleton<IdleSignals>
    {
        public UnityAction onBaseAreaBuyedItem = delegate {  };
    }
}