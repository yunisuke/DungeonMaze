using UnityEngine;

namespace Scenes.IngameScene.DungeonMap
{
    public class WallCell : BaseCell
    {
        public override Color CellColor {
            get {
                Color tmp;
                ColorUtility.TryParseHtmlString("#919191", out tmp);
                return tmp;
            }
        }
        public override bool CanMove {get {return false;}}
        public override bool IsOpen {get {return isOpen;}}

        public WallCell(CellType type) : base(type)
        {
        }
    }
}