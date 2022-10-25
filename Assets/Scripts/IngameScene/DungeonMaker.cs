using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMaker : MonoBehaviour
{
    [SerializeField] private Player pl;
    [SerializeField] private GameObject Ground;
    [SerializeField] private GameObject Wall;
    [SerializeField] private GameObject Goal;

    public void MakeDungeon(Map map)
    {
        PutPrefabs(map);
        SetPlayerPosition(map.Position);
    }

    private void SetPlayerPosition(PlayerPosition p)
    {
        pl.transform.position = new Vector3(p.x, 1, -p.y);
        pl.transform.eulerAngles = GetPlayerRotation(p.d);
    }

    private Vector3 GetPlayerRotation(Direction d)
    {
        int dic;
        switch(d)
        {
            case Direction.North:
                dic = 0;
                break;
            case Direction.South:
                dic = 180;
                break;
            case Direction.West:
                dic = 270;
                break;
            case Direction.East:
                dic = 90;
                break;
            default:
                dic = 0;
                break;
        }
        return new Vector3(0, dic, 0);
    }

    private void PutPrefabs(Map map)
    {
        for (int y = 0; y < map.Max_Y; y++)
        {
            for (int x = 0; x < map.Max_X; x++)
            {
                var c = map.Cells[y, x];
                GameObject obj;
                switch(c.CellType)
                {
                    case CellType.Ground:
                        obj = GameObject.Instantiate(Ground);
                        obj.transform.position = new Vector3(x, 0, -y);
                        break;
                    case CellType.Wall:
                        obj = GameObject.Instantiate(Wall);
                        obj.transform.position = new Vector3(x, 1, -y);
                        break;
                    case CellType.Goal:
                        var g = GameObject.Instantiate(Goal);
                        g.transform.position = new Vector3(x, 1, -y);
                        
                        obj = GameObject.Instantiate(Ground);
                        obj.transform.position = new Vector3(x, 0, -y);
                        break;  
                }
            }
        }
    }
}