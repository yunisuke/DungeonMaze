using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace Manager
{
    public static class DataManager
    {
        private static string filePath;
        public static Dictionary<int, ClearData> clearData = new Dictionary<int, ClearData>();
        private static SaveData s = new SaveData();

        public static void Initialize()
        {
            filePath = Application.persistentDataPath + "/" + "savedata.json";
            Load();
        }

        public static void Save(int mapNo, int getStar)
        {
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

        public static void Load()
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

        public static int GetStageInfo(int level)
        {
            if (clearData.ContainsKey(level) == false) return 0;
            Debug.Log("level is " + clearData[level]);
            return clearData[level].GetStar;
        }

        [Serializable]
        public class SaveData{
            public List<ClearData> clearDataList = new List<ClearData>();
        }
    }
}
