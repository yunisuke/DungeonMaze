using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

namespace Scenes.IngameScene.DungeonMap
{
    public interface ICell
    {
        CellType CellType {get;}
        Color CellColor {get;}
        bool CanMove {get;} // 移動可能か？
        bool IsOpen {get;} // 解放済みのセルか？

        Func<IEnumerator> ExecOnCellEvent(DungeonScene ds); // セルに乗った際のイベントを実行
        void ExecIntoSightCellEvent(); // 視界に入った際のセルのイベントを実行
    }
}