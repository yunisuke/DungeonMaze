using UnityEngine;

namespace Scenes.IngameScene.DungeonMap
{
    public class EmptyCell : BaseCell
    {
        public override Color CellColor => Color.black;
        public override bool CanMove {get {return true;}}
        public override bool IsOpen {get {return isOpen;}}

        public EmptyCell(CellType type) : base(type)
        {
        }
    }
}