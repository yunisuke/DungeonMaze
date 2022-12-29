using UnityEngine;

namespace Scenes.IngameScene.DungeonMap
{
    public class GroundCell : BaseCell
    {
        public override Color CellColor => Color.white;
        public override bool CanMove {get {return true;}}
        public override bool IsOpen {get {return isOpen;}}

        public GroundCell(CellType type) : base(type)
        {
        }
    }
}