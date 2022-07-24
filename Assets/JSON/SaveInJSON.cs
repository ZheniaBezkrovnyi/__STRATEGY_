﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveInJSON : MonoBehaviour
{
    public void SaveThisHouseInList(House house)
    {
        Debug.Log("AddInList");
        house.houseTextOnShop.buttonChange.AddCurrentBuildThisHouse(); // визветься тільки при ставленні;по ссилкам все норм, при взятті, даю сюди ссилку на buttonChange кнопки на якій це пишеться і маю тут її дані, і у неї змінюю і свої і її                                                     
        AllDataHouse allDataHouse = new AllDataHouse()
        {
            dataHouse = new DataHouse()
            {
                NameThisHouse = house.dataHouse.NameThisHouse,
                posit = new Posit(house.dataHouse.posit.x, house.dataHouse.posit.z),
                myIndexOnSave = ReturnAllOnStart.allData.allDataHouses.Count
            },
            dataHouseChangeOnText = new DataHouseChangeOnText()
            {
                currentBuildThisHouse = house.houseTextOnShop.buttonChange.houseTextOnShop.dataHouseChangeOnText.currentBuildThisHouse,
            }
        }; // тут початок запису для цього обьекту бо я його тут ставлю, тому норм що зразу новий allDataHouse
        house.dataHouse = allDataHouse.dataHouse;
        ReturnAllOnStart.allData.allDataHouses.Add(allDataHouse);
        ReturnAllOnStart.json.Save(ReturnAllOnStart.allData);
    }
}
