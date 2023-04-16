using UnityEngine;
using Scenes.IngameScene.DungeonMap;

namespace Scenes.IngameScene
{
    /// <summary>
    /// ダンジョン生成クラス
    /// </summary>
    public class DungeonMaker : MonoBehaviour
    {
        [Header("Dungoen Prefab")]
        [SerializeField] private GameObject Ground;
        [SerializeField] private GameObject Wall;
        [SerializeField] private GameObject DummyWall;
        [SerializeField] private GameObject darkZone;
        [SerializeField] private GameObject Goal;
        [SerializeField] private GameObject[] Warps;

        [Header("Dungeon Container")]
        [SerializeField] private GameObject DungeonContainer;
        [SerializeField] private GameObject GoalContainer;

        [Header("Dungeon MapId")]
        [SerializeField] private Data.MapId MapId;

        /// <summary>
        /// inspectorで設定されたMapIdからダンジョンを作成する
        /// </summary>
        public void Awake()
        {
            var map = MapReader.ReadFile(MapId);
            MakeDungeon(map);
        }

        /// <summary>
        /// ダンジョンを作成する
        /// </summary>
        /// <param name="map"></param>
        public void MakeDungeon(MapData map)
        {
            PutPrefabs(map);
        }

        /// <summary>
        /// MapDataからダンジョンprefabを生成する
        /// </summary>
        /// <param name="map"></param>
        private void PutPrefabs(MapData map)
        {
            for (int y = 0; y < map.Max_Y; y++)
            {
                for (int x = 0; x < map.Max_X; x++)
                {
                    var c = map.Cells[y, x];
                    GameObject obj;
                    switch(c.CellType)
                    {
                        case CellType.Empty:
                            break;
                        case CellType.Ground:
                            obj = GameObject.Instantiate(Ground);
                            obj.transform.position = new Vector3(x, 0, -y);
                            PutInDungeonContainer(obj);
                            break;
                        case CellType.Wall:
                            obj = GameObject.Instantiate(Wall);
                            obj.transform.position = new Vector3(x, 1, -y);
                            PutInDungeonContainer(obj);
                            break;
                        case CellType.DummyWall:
                            obj = GameObject.Instantiate(DummyWall);
                            obj.transform.position = new Vector3(x, 1, -y);
                            PutInDungeonContainer(obj);

                            var obj2 = GameObject.Instantiate(Ground);
                            obj2.transform.position = new Vector3(x, 0, -y);
                            PutInDungeonContainer(obj2);
                            break;
                        case CellType.DarkZone:
                            obj = GameObject.Instantiate(darkZone);
                            obj.transform.position = new Vector3(x, 1, -y);
                            PutInDungeonContainer(obj);
                            break;
                        case CellType.Goal:
                            var g = GameObject.Instantiate(Goal);
                            g.transform.position = new Vector3(x, 1, -y);
                            PutInGoalContainer(g);
                            
                            obj = GameObject.Instantiate(Ground);
                            obj.transform.position = new Vector3(x, 0, -y);
                            PutInDungeonContainer(obj);
                            break;  
                        case CellType.Warp:
                            var warpCell = (WarpCell)c;

                            if (warpCell.ExistEffect && warpCell.Type != WarpCell.WarpType.Goal)
                            {
                                var warpObj = Warps[warpCell.WarpNum];

                                obj = GameObject.Instantiate(warpObj);
                                obj.transform.position = new Vector3(x, 1, -y);
                                PutInDungeonContainer(obj);
                            }

                            var obj3 = GameObject.Instantiate(Ground);
                            obj3.transform.position = new Vector3(x, 0, -y);
                            PutInDungeonContainer(obj3);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 作成したobjectをcontainerに格納する
        /// </summary>
        /// <param name="obj"></param>
        private void PutInDungeonContainer(GameObject obj)
        {
            obj.transform.SetParent(DungeonContainer.transform);
        }

        /// <summary>
        /// 作成したgoalオブジェクトをcontainerに格納する
        /// </summary>
        /// <param name="obj"></param>
        private void PutInGoalContainer(GameObject obj)
        {
            obj.transform.SetParent(GoalContainer.transform);
        }
    }
}
