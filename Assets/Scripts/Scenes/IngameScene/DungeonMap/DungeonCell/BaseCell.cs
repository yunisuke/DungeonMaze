using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

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
        public virtual Func<IEnumerator> ExecOnCellEvent(DungeonScene ds) {return null;}
        public virtual void ExecIntoSightCellEvent()
        {
            isOpen = true;
        }
    }
}