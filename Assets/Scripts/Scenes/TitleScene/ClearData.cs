using System;

namespace Scenes.TitleScene
{
    [Serializable]
    public class ClearData
    {
        public int StageNumber;
        public int GetStar;

        public ClearData(int mapNo, int getStar)
        {
            StageNumber = mapNo;
            GetStar = getStar;
        }
    }
}
