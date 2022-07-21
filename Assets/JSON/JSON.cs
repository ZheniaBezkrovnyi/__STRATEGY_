using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public enum TypeSave
{
    House,

}
public class JSON
{
    public string fileJSON;
    public JSON()
    {
        string application;

#if UNITY_ANDROID
        application = Application.persistentDataPath;
#endif
#if UNITY_EDITOR
        application = Application.dataPath;
#endif
        fileJSON = Path.Combine(application + "/JSON/JSON.json.txt");
    }
    public void Save(AllData allData)
    {
        var json = JsonUtility.ToJson(allData);
        using (var writer = new StreamWriter(fileJSON))
        {
            writer.WriteLine(json);
        }
    }
    public AllData Load()
    {
        string json = "";
        using (var reader = new StreamReader(fileJSON))
        {
            for (string line; (line = reader.ReadLine()) != null;)
            {
                json += line;
            }
        }
        if (string.IsNullOrEmpty(json))
        {
            return null;
        }
        return JsonUtility.FromJson<AllData>(json);
    }
}

