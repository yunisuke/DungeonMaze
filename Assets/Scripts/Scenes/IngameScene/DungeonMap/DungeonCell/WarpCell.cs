using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Scenes.IngameScene.DungeonMap
{
    public class WarpCell : BaseCell
    {
        public override Color CellColor => Color.white;
        public override bool CanMove {get {return true;}}
        public override bool IsOpen {get {return isOpen;}}

        private int warpNum;
        private WarpType warpType;

        public WarpCell(string s, CellType type) : base(type)
        {
            var cellType = s.Substring(0, 1);
            if (cellType != "w") {
                Debug.LogError("セルのタイプがワープではありません");
            }

            var warpNum = s.Substring(1, 1);
            if (System.Text.RegularExpressions.Regex.IsMatch(warpNum, "[0-9]") == false) {
                Debug.LogError("ワープ先の指定が数値ではありません。※[0-9]のみ有効");
            }

            var warpType = s.Substring(2, 1);
            if (System.Text.RegularExpressions.Regex.IsMatch(warpType, "[nsg]") == false) {
                Debug.LogError("ワープタイプの指定が間違っています。※[nsg]のみ有効");
            }

            this.warpNum = Int32.Parse(warpNum);
            this.warpType = GetWarpType(warpType);
        }

        public override Func<IEnumerator> ExecOnCellEvent(DungeonScene ds)
        {
            if (warpType == WarpType.Goal) return null;
            return () => ds.Warp(warpNum, this);
        }

        private WarpType GetWarpType(string s)
        {
            switch(s)
            {
                case "n":
                    return WarpType.Normal;
                case "s":
                    return WarpType.Start;
                case "g":
                    return WarpType.Goal;
                default:
                    return WarpType.Normal;
            }
        }

        private enum WarpType
        {
            Normal, // 双方向ワープ
            Start, // ワープスタート地点(一方向)
            Goal, // ワープゴール地点(一方向)
        }
    }
}