using System.IO;
using System.Text;
using UnityEngine;

public class MapReader
{
    public static Map ReadFile(int level)
    {
        if (level == 0) level = 1;

        var fileName = level;
        TextAsset txt = Resources.Load("MapFile/" + fileName) as TextAsset;

        var map = CreateMap(level, txt);
        return map;
    }

    private static Map CreateMap(int level, TextAsset txt)
    {
        int maxX = GetMaxX(txt);
        int maxY = GetMaxY(txt);
        PlayerPosition p = null;

        Cell[,] cells = new Cell[maxY, maxX];
        for (int _y = 0; _y < maxY; _y++)
        {
            for (int x = 0; x < maxX; x++)
            {
                cells[_y, x] = new Cell(CellType.Wall);
            }
        }

        StringReader reader = new StringReader(txt.text);
        int y = 0;

        // ヘッダー取得
        string header = reader.ReadLine();
        var mh = JsonUtility.FromJson<MapHeader>(header);

        // マップ読み込み
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            int x = 0;
            foreach (var s in line)
            {
                cells[y, x] = ChangeCell(s);
                if (s == 's') p = SetPlayerPosition(x, y, mh.pd);
                x++;
            }
            y++;
        }

        return new Map(level, maxX, maxY, cells, p, mh.star2, mh.star3);
    }

    private static PlayerPosition SetPlayerPosition(int x, int y, string pd)
    {
        Direction d;
        switch(pd)
        {
            case "n":
                d = Direction.North;
                break;
            case "s":
                d = Direction.South;
                break;
            case "w":
                d = Direction.West;
                break;
            case "e":
                d = Direction.East;
                break;
            default:
                d = Direction.North;
                break;
        }

        return new PlayerPosition(x, y, d);
    }

    private static int GetMaxX(TextAsset txt)
    {
        int maxX = 0;

        StringReader reader = new StringReader(txt.text);

        // ヘッダー抜き出し
        reader.ReadLine();

        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            int x = 0;
            foreach (var s in line)
            {
                x++;
            }
            if (x > maxX) maxX = x;
        }
        return maxX;
    }

    private static int GetMaxY(TextAsset txt)
    {
        int maxY = 0;

        StringReader reader = new StringReader(txt.text);
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            maxY++;
        }
        return maxY;
    }

    private static Cell ChangeCell(char c)
    {
        switch(c)
        {
            case '-':
                return new Cell(CellType.None);
            case 's':
                return new Cell(CellType.Ground);
            case '#':
                return new Cell(CellType.Wall);
            case ' ':
                return new Cell(CellType.Ground);
            case 'g':
                return new Cell(CellType.Goal);
            default:
                return null;
        }
    }

    public class MapHeader
    {
        public string pd;
        public float star2;
        public float star3;
    }
}
