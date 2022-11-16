using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Scenes.TitleScene;

namespace Data
{
    public class FileRepository : IDataRepository
    {
        private string filePath;

        public FileRepository()
        {
            filePath = Application.persistentDataPath + "/" + "savedata.json";
        }

        public void Save(List<ClearData> clearDataLIst)
        {
            var saveData = new SaveData();
            saveData.clearDataList = clearDataLIst;

            string json = JsonUtility.ToJson(saveData);
            StreamWriter streamWriter = new StreamWriter(filePath);
            streamWriter.Write(json);
            streamWriter.Flush();
            streamWriter.Close();
        }

        public List<ClearData> Load()
        {
            if (File.Exists(filePath) == false) return new List<ClearData>();

            StreamReader streamReader;
            streamReader = new StreamReader(filePath);
            string data = streamReader.ReadToEnd();
            streamReader.Close();
            var tmp = JsonUtility.FromJson<SaveData>(data);
            return tmp.clearDataList;
        }

        public void Delete()
        {
            File.Delete(filePath);
        }
    }

    [Serializable]
    public class SaveData{
        public List<ClearData> clearDataList = new List<ClearData>();
    }
}