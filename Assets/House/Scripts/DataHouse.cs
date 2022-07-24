﻿using System.Collections;
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
    public TypeHouse typeHouse;
    public Info info;
}

[Serializable]
public class Info
{
    public Sprite spriteHouse;
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
    [HideInInspector] public Posit posit;
    [HideInInspector] public int myIndexOnSave;  //**** коли ставлю
    public int levelHouse;
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
    public static Vector2 InitInPosit(int x,int z,House house)
    {
        float X = (x + (float)house.NeParniX / 2f + MyTerrain.xMin) * MyTerrain.sizeOneCell;
        float Z = (z + (float)house.NeParniZ / 2f + MyTerrain.zMin) * MyTerrain.sizeOneCell;
        return new Vector2(X,Z);
    }
    public static Vector2Int DesWithPosit(float x, float z, House house)
    {
        int X = Mathf.RoundToInt( x / MyTerrain.sizeOneCell - MyTerrain.xMin - (float)house.NeParniX / 2f);
        int Z = Mathf.RoundToInt(z / MyTerrain.sizeOneCell - MyTerrain.xMin - (float)house.NeParniZ / 2f);
        return new Vector2Int(X, Z);
    }
}