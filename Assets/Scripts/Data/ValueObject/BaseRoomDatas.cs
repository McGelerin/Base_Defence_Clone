using System;
using System.Collections.Generic;

namespace Data.ValueObject
{
    [Serializable]
    public class BaseRoomDatas
    {
        public List<RoomData> Rooms = new List<RoomData>();
    }
}