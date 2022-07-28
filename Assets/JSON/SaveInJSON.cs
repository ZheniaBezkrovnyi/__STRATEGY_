using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveInJSON : MonoBehaviour
{
    public void AddThisHouseInList(House house)
    {
        house.houseTextOnShop.buttonChange.AddCurrentBuildThisHouse(); // визветься тільки при ставленні;по ссилкам все норм, при взятті, даю сюди ссилку на buttonChange кнопки на якій це пишеться і маю тут її дані, і у неї змінюю і свої і ї
        AllDataHouse allDataHouse = new AllDataHouse()
        {
            dataHouse = new DataHouse()
            {
                NameThisHouse = house.dataHouse.NameThisHouse,
                posit = new Posit(house.dataHouse.posit.x, house.dataHouse.posit.z),
                dataAnimBuildHouse = new DataAnimBuildHouse(),
                myIndexOnSave = ReturnAllOnStart.allData.allDataHouses.Count,
                levelHouse = 0
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

    public void SaveInsteadThisTwoHouseInList(House house,House _house)
    {
        int indexLast = _house.dataHouse.myIndexOnSave;
        house.dataHouse = _house.dataHouse;
        house.dataHouse.levelHouse = _house.dataHouse.levelHouse + 1;
        house.canvasHouse = _house.canvasHouse;
        house.houseTextOnShop = _house.houseTextOnShop;
        house.houseTextOnShop.dataHouseChangeOnText.currentBuildThisHouse = ReturnAllOnStart.allData.allDataHouses[indexLast].dataHouseChangeOnText.currentBuildThisHouse;
        house.existOrNot = ExistOrNot.Yes;
        AllDataHouse allDataHouse = new AllDataHouse()
        {
            dataHouse = new DataHouse()
            {
                NameThisHouse = house.dataHouse.NameThisHouse,
                posit = new Posit(house.dataHouse.posit.x, house.dataHouse.posit.z),
                dataAnimBuildHouse = new DataAnimBuildHouse(),
                myIndexOnSave = indexLast,
                levelHouse = house.dataHouse.levelHouse
            },
            dataHouseChangeOnText = new DataHouseChangeOnText()
            {
                currentBuildThisHouse = _house.houseTextOnShop.dataHouseChangeOnText.currentBuildThisHouse
            }
        };
        ReturnAllOnStart.allData.allDataHouses[indexLast] = allDataHouse;
        ReturnAllOnStart.json.Save(ReturnAllOnStart.allData);
    }
}
