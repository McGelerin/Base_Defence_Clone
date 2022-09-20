using System.Collections.Generic;
using Enums;

namespace Keys
{
    public struct AreaDataParams
    {
        public Dictionary<RoomNameEnum,int> RoomPayedAmound;
        public Dictionary<TurretNameEnum,int> RoomTurretPayedAmound;
    }
}