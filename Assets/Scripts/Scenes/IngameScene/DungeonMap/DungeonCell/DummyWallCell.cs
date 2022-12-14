using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

namespace Scenes.IngameScene.DungeonMap
{
    public class DummyWallCell : BaseCell
    {
        private bool isOnCell = false;

        public override Color CellColor {
            get {
                Color tmp;

                if (isOnCell)
                {
                    ColorUtility.TryParseHtmlString("#ffffff", out tmp);
                }
                else
                {
                    ColorUtility.TryParseHtmlString("#919191", out tmp);
                }
                
                return tmp;
            }
        }
        public override bool CanMove => true;
        public override bool IsOpen => isOpen;

        public override Func<IEnumerator> ExecOnCellEvent(DungeonScene ds)
        {
            isOnCell = true;
            return null;
        }

        public DummyWallCell(CellType type) : base(type)
        {
        }
    }
}