using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum TypeSave
{
    House,

}
public class JSON
{
    private string fileJSON;
    public JSON()
    {
        fileJSON = Application.persistentDataPath + "/save.json";
    }/*
    public void Save(Data data)
    {
        var json = JsonUtility.ToJson(data);
        using (var writer = new StreamWriter(fileJSON))
        {
            writer.WriteLine(json);
        }
    }
    public Data Load()
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
        return JsonUtility.FromJson<Data>(json);
    }*/
}
