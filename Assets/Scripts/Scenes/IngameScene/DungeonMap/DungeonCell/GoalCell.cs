using UnityEngine;

namespace Scenes.IngameScene.DungeonMap
{
    public class GoalCell : BaseCell
    {
        public override Color CellColor => Color.white;
        public override bool CanMove => true;
        public override bool IsOpen => isOpen;

        public GoalCell(CellType type) : base(type)
        {
        }
    }
}