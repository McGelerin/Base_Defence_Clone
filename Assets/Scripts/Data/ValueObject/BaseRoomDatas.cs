using System;
using System.Collections.Generic;

namespace Data.ValueObject
{
    [Serializable]
    public class BaseRoomDatas
    {
        public List<TurretData> TurretDatas = new List<TurretData>(2);
        public List<RoomData> Rooms = new List<RoomData>();
    }
}