using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPr : MonoBehaviour
{
    public static void InitInt(string key,int value)
    {
        PlayerPrefs.SetInt(key,value);
        PlayerPrefs.Save();
    }
    public static void GetInt(string key)
    {
        PlayerPrefs.GetInt(key);
    }
}
