using UnityEngine;

namespace Scenes.IngameScene.DungeonMap
{
    public class DarkZoneCell : BaseCell
    {
        public override Color CellColor => Color.black;
        public override bool CanMove {get {return true;}}
        public override bool IsOpen {get {return isOpen;}}

        public DarkZoneCell(CellType type) : base(type)
        {
        }
    }
}