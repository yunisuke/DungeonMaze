using UnityEngine;
using Data;

namespace Scenes.IngameScene.DungeonMap
{
    public class MapData
    {
        public MapId MapId {get; private set;}

        public Cell[,] Cells {get; private set;}
        public int Max_X {get; private set;}
        public int Max_Y {get; private set;}
        public PlayerPosition Position{get; private set;}
        public float Star3 {get; private set;}
        public float Star2 {get; private set;}

        public MapData(MapId mapId, int maxX, int maxY, Cell[,] c, PlayerPosition p, float star2, float star3)
        {
            MapId = mapId;

            Max_X = maxX;
            Max_Y = maxY;
            Cells = c;
            Position = p;

            Star2 = star2;
            Star3 = star3;
        }

        public Cell TurnRightPlayer()
        {
            Position.TurnRight();
            return Cells[Position.y, Position.x];
        }

        public Cell TurnLeftPlayer()
        {
            Position.TurnLeft();
            return Cells[Position.y, Position.x];
        }

        public bool EnableMove(bool isAhead)
        {
            // 移動先
            var targetX = Position.x + GetTargetX(Position.d, isAhead);
            var targetY = Position.y + GetTargetY(Position.d, isAhead);

            if (targetX > Max_X || targetX < 0) return false;
            if (targetY > Max_Y || targetY < 0) return false;

            if (Cells[targetY, targetX].CellType == CellType.Wall) return false;

            return true;
        }

        public Cell MovePlayer(bool isAhead)
        {
            var targetX = Position.x + GetTargetX(Position.d, isAhead);
            var targetY = Position.y + GetTargetY(Position.d, isAhead);

            Position.x = targetX;
            Position.y = targetY;
            Debug.Log("after x is " + Position.x);
            Debug.Log("after y is " + Position.y);
            return Cells[Position.y, Position.x];
        }

        private int GetTargetX(Direction d, bool isAhead)
        {
            int diff;
            switch(d)
            {
                case Direction.East:
                    diff = 1;
                    break;
                case Direction.West:
                    diff = -1;
                    break;
                default:
                    diff = 0;
                    break;
            }

            return isAhead ? diff : -diff;
        }

        private int GetTargetY(Direction d, bool isAhead)
        {
            int diff;
            switch(d)
            {
                case Direction.North:
                    diff = -1;
                    break;
                case Direction.South:
                    diff = 1;
                    break;
                default:
                    diff = 0;
                    break;
            }

            return isAhead ? diff : -diff;
        }
    }

    public class Cell
    {
        public CellType CellType {get; private set;}
        private EventType eventType;
        public bool IsOpen {get; set;}

        public Cell(CellType ct)
        {
            CellType = ct;
        }
    }

    public enum CellType
    {
        None, // 何もない
        Ground, // 地面
        Wall, // 壁。移動不可能
        DummyWall, // 偽物の壁。移動可能
        Goal, // ゴール
    }

    public enum EventType
    {
        None, // 何もない
        Item, // アイテムが配置されている
        Warp, // ワープゾーン
        // Door = 1 << 2, // 扉
    }

    public class PlayerPosition
    {
        public int x; // プレイヤーX座標
        public int y; // プレイヤーY座標
        public Direction d; // プレイヤー向き

        public PlayerPosition(int x, int y, Direction d)
        {
            this.x = x;
            this.y = y;
            this.d = d;
        }

        public void TurnRight()
        {
            switch(d)
            {
                case Direction.North:
                    d = Direction.East;
                    break;
                case Direction.East:
                    d = Direction.South;
                    break;
                case Direction.South:
                    d = Direction.West;
                    break;
                case Direction.West:
                    d = Direction.North;
                    break;
            }
        }

        public void TurnLeft()
        {
            switch(d)
            {
                case Direction.North:
                    d = Direction.West;
                    break;
                case Direction.East:
                    d = Direction.North;
                    break;
                case Direction.South:
                    d = Direction.East;
                    break;
                case Direction.West:
                    d = Direction.South;
                    break;
            }
        }
    }

    public enum Direction
    {
        North,
        South,
        West,
        East,
    }
}
