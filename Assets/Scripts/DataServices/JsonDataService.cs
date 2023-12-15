using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonDataService : IDataService
{
    string basePath = Application.persistentDataPath;
    public T loadData<T>(string relativePath, bool encrypt = true)
    {
        string savePath = basePath + relativePath;

        if (!File.Exists(savePath))
        {
            Debug.LogError($"Cannot load file at {savePath}");
            return default;
            throw new FileNotFoundException($"File is missing at {savePath}");
        }
        else
        {
            try
            {
                T data = JsonConvert.DeserializeObject<T>(File.ReadAllText(savePath));
                return data;
            }
            catch (Exception e)
            {
                Debug.LogError($"Load failed because of {e.Message} , exception stackTrace: {e.StackTrace}");
                return default;
                throw e;
            }
            
        }
        
    }

    public void saveData<T>(string relativePath, T data, bool encrypt = true)
    {
        string savePath = basePath + relativePath;
        try
        {
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
                Debug.Log("Saved New Data, deleted old one.");
            }
            else Debug.Log("Saved Data, First time");

            using FileStream stream = File.Create(savePath);
            stream.Close();
            File.WriteAllText(savePath, JsonConvert.SerializeObject(data, Formatting.Indented));
        }
        catch (Exception e)
        {
            Debug.LogError($"Save failed because of {e.Message} , exception stackTrace: {e.StackTrace}");
        }
    }

}
