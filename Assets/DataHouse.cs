using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class HouseTextOnShop //дание для кнопок в магазине
{
    [HideInInspector] public DataHouseChangeOnText dataHouseChangeOnText;
    public int MaxCountBuild;
    public TimeBuild TimeBuildStart;
    [HideInInspector] public ButtonChange buttonChange;
}
[Serializable]
public class DataTextOnHouse
{
    public int priceImprove;
    public TimeBuild timeImprove;
    public Info info;
    [HideInInspector] public bool openCanvas;
}

public class Info
{
    public Image imageHouse;
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
    [HideInInspector] public Posit posit; //*** при End
    [HideInInspector] public int myIndexOnSave;  //**** коли ставлю
}

[Serializable]
public class DataHouseChangeOnText
{
    public int currentBuildThisHouse;  //**** коли ставлю
}
[Serializable]
public class Posit
{
    public int x, z;
    public Posit(int _x, int _z)
    {
        x = _x;
        z = _z;
    }
}