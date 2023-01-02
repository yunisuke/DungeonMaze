using UnityEngine;

namespace Scenes.IngameScene.DungeonMap
{
    public abstract class BaseCell : ICell
    {
        protected CellType type;
        protected bool isOpen;

        public BaseCell(CellType type)
        {
            this.type = type;
        }

        public CellType CellType => type;
        public abstract Color CellColor {get;}
        public abstract bool CanMove {get;}
        public abstract bool IsOpen {get;}
        public virtual void ExecOnCellEvent(DungeonScene ds) {}
        public virtual void ExecIntoSightCellEvent()
        {
            isOpen = true;
        }
    }
}