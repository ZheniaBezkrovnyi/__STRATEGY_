using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public enum TypeSave
{
    House,

}
public class JSON
{
    private string fileJSON;
    private Notification notification;
    public JSON(Notification _notification)
    {
        notification = _notification;
        string application;
#if UNITY_ANDROID && !UNITY_EDITOR
        application = Application.persistentDataPath;
#else
        application = Application.dataPath;
#endif
        fileJSON = Path.Combine(application, "JSON.json.txt");
        if (ReturnAllOnStart.startProject == StartProject.Start && !File.Exists(fileJSON))
        {
            File.Create(fileJSON);
            SceneManager.LoadScene("SampleScene");
        }
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

