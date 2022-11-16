using System;

namespace Data
{
    [Serializable]
    public class ClearData
    {
        public MapId MapId;
        public int GetStar;

        public ClearData(MapId mapId, int getStar)
        {
            this.MapId = mapId;
            this.GetStar = getStar;
        }
    }
}
