using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    [SerializeField] private RectTransform markerTr;
    [SerializeField] private MiniMapCell[] miniMapCells;

    public void UpdateMap(Map map)
    {
        SetMarkerDirection(map.Position.d);
        DrawMiniMapCell(map, map.Cells, map.Position);
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
                Debug.Log("target x is " + targetX);
                Debug.Log("target y is " + targetY);
                if (targetX >= m.Max_X || targetY >= m.Max_Y || targetX < 0 || targetY < 0) 
                {
                    mc.ClearCell();
                    continue;
                }

                var c = cells[targetY, targetX];
                mc.DrawCell(c.CellType);
            }
        }
    }
}
