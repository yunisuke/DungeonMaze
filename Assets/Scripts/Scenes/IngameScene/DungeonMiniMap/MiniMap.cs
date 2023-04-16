using UnityEngine;
using Scenes.IngameScene.DungeonMap;

namespace Scenes.IngameScene.DungeonMiniMap
{
    public class MiniMap : MonoBehaviour
    {
        [SerializeField] private RectTransform markerTr;
        [SerializeField] private MiniMapCell[] miniMapCells;

        public void UpdateMinimap(MapData map)
        {
            OpenCell(map);
            SetMarkerDirection(map.Position.d);
            DrawMiniMapCell(map, map.Cells, map.Position);
        }

        private void OpenCell(MapData map)
        {
            map.Cells[map.Position.y, map.Position.x].ExecIntoSightCellEvent(); // 現在位置をオープン

            // 現在のセルがダークゾーンの場合は開放されない
            var nowCell = map.GetNowCell();
            if (nowCell.CellType == CellType.DarkZone) return;

            (int x, int y) front = (0, 0);
            (int x, int y) right = (0, 0);
            (int x, int y) left = (0, 0);
            (int x, int y) fRight = (0, 0);
            (int x, int y) fLeft = (0, 0);

            switch(map.Position.d)
            {
            case Direction.North:
                front  = (map.Position.x,     map.Position.y - 1); // 正面
                right  = (map.Position.x + 1, map.Position.y);     // 右
                left   = (map.Position.x - 1, map.Position.y);     // 左
                fRight = (map.Position.x + 1, map.Position.y - 1); // 右正面
                fLeft  = (map.Position.x - 1, map.Position.y - 1); // 左正面
                break;
            case Direction.South:
                front  = (map.Position.x,     map.Position.y + 1); // 正面
                right  = (map.Position.x - 1, map.Position.y);     // 右
                left   = (map.Position.x + 1, map.Position.y);     // 左
                fRight = (map.Position.x - 1, map.Position.y + 1); // 右正面
                fLeft  = (map.Position.x + 1, map.Position.y + 1); // 左正面
                break;
            case Direction.West:
                front  = (map.Position.x - 1, map.Position.y);     // 正面
                right  = (map.Position.x,     map.Position.y - 1); // 右
                left   = (map.Position.x,     map.Position.y + 1); // 左
                fRight = (map.Position.x - 1, map.Position.y - 1); // 右正面
                fLeft  = (map.Position.x - 1, map.Position.y + 1); // 左正面
                break;
            case Direction.East:
                front  = (map.Position.x + 1, map.Position.y);     // 正面
                right  = (map.Position.x,     map.Position.y + 1); // 右
                left   = (map.Position.x,     map.Position.y - 1); // 左
                fRight = (map.Position.x + 1, map.Position.y + 1); // 右正面
                fLeft  = (map.Position.x + 1, map.Position.y - 1); // 左正面
                break;
            }

            Open(front.x, front.y, map); // 正面
            Open(right.x, right.y, map); // 右
            Open(left.x,  left.y,  map); // 左

            // 正面セルが壁やダークゾーンの場合は右正面、左正面は開放されない
            var frontCell = map.Cells[front.y, front.x];
            var rightCell = map.Cells[right.y, right.x];
            var leftCell =  map.Cells[left.y,  left.x];

            if (frontCell.IsBlockCell() == false || rightCell.IsBlockCell() == false) Open(fRight.x, fRight.y, map); // 右正面
            if (frontCell.IsBlockCell() == false || leftCell.IsBlockCell() == false)  Open(fLeft.x,  fLeft.y,  map); // 左正面
        }

        private void Open(int x, int y, MapData map)
        {
            if (x >= map.Max_X || y >= map.Max_Y || x < 0 || y < 0) return;
            map.Cells[y, x].ExecIntoSightCellEvent();
        }

        private void SetMarkerDirection(Direction d)
        {
            switch(d)
            {
            case Direction.North:
                markerTr.localEulerAngles = new Vector3(0, 0, 0);
                break;
            case Direction.South:
                markerTr.localEulerAngles = new Vector3(0, 0, 180);
                break;
            case Direction.West:
                markerTr.localEulerAngles = new Vector3(0, 0, 90);
                break;
            case Direction.East:
                markerTr.localEulerAngles = new Vector3(0, 0, 270);
                break;
            }
        }

        private void DrawMiniMapCell(MapData m, BaseCell[,] cells, PlayerPosition p)
        {
            int beginX = p.x - 4;
            int endX = p.x + 4;
            int beginY = p.y - 4;
            int endY = p.y + 4;

            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    var mc = miniMapCells[y * 9 + x];
                    var targetX = beginX + x;
                    var targetY = beginY + y;

                    // ターゲットがマップ範囲外
                    if (targetX >= m.Max_X || targetY >= m.Max_Y || targetX < 0 || targetY < 0)
                    {
                        mc.ClearCell();
                        continue;
                    }

                    // ターゲットがまだ開拓されていない
                    var c = cells[targetY, targetX];
                    if (c.IsOpen == false)
                    {
                        mc.ClearCell();
                        continue;
                    }

                    // マップに地形記載
                    mc.DrawCell(c.CellColor);
                }
            }
        }
    }
}
