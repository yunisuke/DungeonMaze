using UnityEngine;

namespace Scenes.IngameScene
{
    public class MiniMap : MonoBehaviour
    {
        [SerializeField] private RectTransform markerTr;
        [SerializeField] private MiniMapCell[] miniMapCells;

        public void Update(Map map)
        {
            OpenCell(map);
            SetMarkerDirection(map.Position.d);
            DrawMiniMapCell(map, map.Cells, map.Position);
        }

        private void OpenCell(Map map)
        {
            map.Cells[map.Position.y, map.Position.x].IsOpen = true; // 現在位置をオープン

            switch(map.Position.d)
            {
            case Direction.North:
                Open(map.Position.x,     map.Position.y - 1, map); // 正面
                Open(map.Position.x + 1, map.Position.y - 1, map); // 右正面
                Open(map.Position.x - 1, map.Position.y - 1, map); // 左正面
                Open(map.Position.x + 1, map.Position.y,     map); // 右
                Open(map.Position.x - 1, map.Position.y,     map); // 左
                break;
            case Direction.South:
                Open(map.Position.x,     map.Position.y + 1, map); // 正面
                Open(map.Position.x - 1, map.Position.y + 1, map); // 右正面
                Open(map.Position.x + 1, map.Position.y + 1, map); // 左正面
                Open(map.Position.x - 1, map.Position.y,     map); // 右
                Open(map.Position.x + 1, map.Position.y,     map); // 左
                break;
            case Direction.West:
                Open(map.Position.x - 1, map.Position.y,     map); // 正面
                Open(map.Position.x - 1, map.Position.y - 1, map); // 右正面
                Open(map.Position.x - 1, map.Position.y + 1, map); // 左正面
                Open(map.Position.x,     map.Position.y - 1, map); // 右
                Open(map.Position.x,     map.Position.y + 1, map); // 左
                break;
            case Direction.East:
                Open(map.Position.x + 1, map.Position.y,    map); // 正面
                Open(map.Position.x + 1, map.Position.y + 1, map); // 右正面
                Open(map.Position.x + 1, map.Position.y - 1, map); // 左正面
                Open(map.Position.x,     map.Position.y + 1, map); // 右
                Open(map.Position.x,     map.Position.y - 1, map); // 左
                break;
            }
        }

        private void Open(int x, int y, Map map)
        {
            if (x >= map.Max_X || y >= map.Max_Y || x < 0 || y < 0) return;
            map.Cells[y, x].IsOpen = true;
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

        private void DrawMiniMapCell(Map m, Cell[,] cells, PlayerPosition p)
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
                    mc.DrawCell(c.CellType);
                }
            }
        }
    }
}
