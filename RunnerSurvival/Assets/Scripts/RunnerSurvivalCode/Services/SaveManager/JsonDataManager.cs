using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
namespace RunnerSurvivalCode.Services.SaveManager {
    public class JsonDataManager {
        public Action<string> OnDataSaveSuccess;
        public Action<string> OnDataSaveFail;
    
        public Action<string> OnDataLoadSuccess;
        public Action<string> OnDataLoadFail;

        public void SaveToJson<T>(string fileName, T data) {
            try {
                string filePath = Path.Combine(Application.persistentDataPath, fileName);
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(filePath, json);
                OnDataSaveSuccess?.Invoke(fileName);
            }
            catch (Exception ex) {
                Debug.LogError(ex.Message);
                OnDataSaveFail?.Invoke(fileName);
            }
        }

        public T LoadFromJson<T>(string fileName) {
            try {
                string filePath = Path.Combine(Application.persistentDataPath, fileName);

                if (!File.Exists(filePath)) {
                    OnDataLoadFail?.Invoke(fileName);
                    return default;
                }

                string json = File.ReadAllText(filePath);
                T data = JsonConvert.DeserializeObject<T>(json);
                OnDataLoadSuccess?.Invoke(fileName);
                return data;
            }
            catch (Exception) {
                OnDataLoadFail?.Invoke(fileName);
                return default;
            }
        }
    }
}
