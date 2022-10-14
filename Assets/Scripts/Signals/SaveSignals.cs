using System;
using System.Collections.Generic;
using Enums;
using Extentions;
using Keys;
using UnityEngine.Events;

namespace Signals
{
    public class SaveSignals : MonoSingleton<SaveSignals>
    {
        //save
        public UnityAction onLevelSave = delegate {  };
        public Func<int> onSaveLevelData = delegate { return 0;};
        
        public UnityAction onScoreSave = delegate {  };
        public Func<ScoreDataParams> onSaveScoreData = delegate { return default;};
        
        public UnityAction onAreaDataSave = delegate {  };
        public Func<AreaDataParams> onSaveAreaData = delegate { return default;};
        
        public UnityAction onOutSideDataSave = delegate {  };
        public Func<Dictionary<OutSideStateLevels,int>> onSaveOutsideData = delegate { return default;};

        //load
        public Func<int> onLoadCurrentLevel = delegate { return 0;};
        public Func<ScoreDataParams> onLoadScoreData = delegate { return default;};
        public Func<AreaDataParams> onLoadAreaData = delegate { return default;};
        public Func<Dictionary<OutSideStateLevels,int>> onLoadOutsideData = delegate { return default;};
        //public Func<Dictionary<RoomNameEnum,AreaDataParams>> onLoadAreaData = delegate { return default;};
    }
}