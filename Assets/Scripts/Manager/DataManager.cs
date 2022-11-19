using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Manager
{
    public class DataManager
    {
        private static DataManager _instance;
        private IDataRepository repos;
        
        // メモリ内ユーザーデータ
        public Dictionary<MapId, ClearData> clearDataDic = new Dictionary<MapId, ClearData>();

        // メモリ内ステージデータ
        public List<MapId> mapIdList = new List<MapId>();

        private DataManager () {
        }

        public static DataManager Instance {get {
            if (_instance == null) _instance = new DataManager ();
            return _instance;
        }}

        public void Initialize()
        {
            repos = new FileRepository();
            
            LoadUserData();
            LoadStageData();
        }

        public void SaveUserData(MapId mapId, int getStar)
        {
            var nowCd = new ClearData(mapId, getStar);
            var befCd = GetClearData(mapId);
            if (nowCd.GetStar <= befCd.GetStar) return;
            
            // メモリに保存
            clearDataDic[mapId] = nowCd;

            // リポジトリに保存
            var tmp = new List<ClearData>();
            foreach(var c in clearDataDic.Values)
            {
                tmp.Add(c);
            }
            repos.Save(tmp);
        }

        public void LoadUserData()
        {
            List<ClearData> clearDataList = repos.Load();
            foreach(var cd in clearDataList)
            {
                clearDataDic[cd.MapId] = cd;
            }
        }

        public void DeleteData()
        {
            // メモリから削除
            clearDataDic = new Dictionary<MapId, ClearData>();

            // リポジトリから削除
            repos.Delete();
        }

        private void LoadStageData()
        {
            mapIdList = new List<MapId>();

            // ステージデータ読み込み
            TextAsset[] files = Resources.LoadAll<TextAsset>("MapFile");
            foreach(var f in files)
            {
                mapIdList.Add(new MapId(f.name));
            }
        }

        public ClearData GetClearData(MapId mapId)
        {
            if (clearDataDic.ContainsKey(mapId) == false) return new ClearData(mapId, 0);
            return clearDataDic[mapId];
        }

        public MapId GetNextStage(MapId mapId)
        {
            var ind = mapIdList.IndexOf(mapId) + 1;
            if (ind >= mapIdList.Count) return null;
            return mapIdList[ind];
        }

        public bool ExistNextGame(MapId mapId)
        {
            MapId id = GetNextStage(mapId);
            return id != null;
        }
    }
}
