using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using Scenes.TitleScene;

namespace Manager
{
    public class DataManager
    {
        private static DataManager _instance;

        private string filePath;
        public Dictionary<int, ClearData> clearData = new Dictionary<int, ClearData>();
        private SaveData s = new SaveData();

        private DataManager () {
        }

        public static DataManager Instance {get {
            if (_instance == null) _instance = new DataManager ();
            return _instance;
        }}

        public void Initialize()
        {
            filePath = Application.persistentDataPath + "/" + "savedata.json";
            Load();
        }

        public void Save(int mapNo, int getStar)
        {
            if (GetStageInfo(mapNo) >= getStar) return;

            clearData[mapNo] = new ClearData(mapNo, getStar);
            s.clearDataList = new List<ClearData>();
            foreach(var v in clearData.Values)
            {
                s.clearDataList.Add(v);
            }

            string json = JsonUtility.ToJson(s);
            Debug.Log(json);
            StreamWriter streamWriter = new StreamWriter(filePath);
            streamWriter.Write(json);
            streamWriter.Flush();
            streamWriter.Close();
        }

        public void Load()
        {
            if (File.Exists(filePath))
            {
                StreamReader streamReader;
                streamReader = new StreamReader(filePath);
                string data = streamReader.ReadToEnd();
                streamReader.Close();
                var tmp = JsonUtility.FromJson<SaveData>(data);
                s = tmp;
                foreach(var v in s.clearDataList)
                {
                    clearData[v.StageNumber] = v;
                }
            }
        }

        public void DeleteData()
        {
            File.Delete(filePath);
            clearData = new Dictionary<int, ClearData>();
            s = new SaveData();
        }

        public int GetClearStageMax()
        {
            if (clearData.Keys.Count == 0) return 0;
            return clearData.Keys.Max();
        }

        public int GetStageInfo(int level)
        {
            if (clearData.ContainsKey(level) == false) return 0;
            return clearData[level].GetStar;
        }

        [Serializable]
        public class SaveData{
            public List<ClearData> clearDataList = new List<ClearData>();
        }
    }
}
