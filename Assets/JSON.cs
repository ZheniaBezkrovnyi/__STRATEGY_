using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public enum TypeSave
{
    House,

}
[CreateAssetMenu]
public class JSON : ScriptableObject
{
    public string fileJSON;
    public JSON()
    {
        //fileJSON = Application.persistentDataPath + "/save.json";
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

[Serializable]
public class AllData
{
    public List<AllDataHouse> allDataHouses;
}

[Serializable]
public class AllDataHouse
{
    public DataHouse dataHouse;
    public DataHouseChangeOnText dataHouseChangeOnText;
}

[Serializable]
public class DataHouse
{
    public NameHouse NameThisHouse; //**** коли ставлю
    public Posit posit; //*** при End
    public int myIndexOnSave;  //**** коли ставлю


}

[Serializable]
public class DataHouseChangeOnText
{
    public int currentBuildThisHouse;  //**** коли ставлю
}

public class Posit
{
    public int x, z;
    public Posit(int _x,int _z)
    {
        x = _x;
        z = _z;
    }
}