using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public enum StartProject
{
    Start,
    Continue
}
public class ReturnAllOnStart : MonoBehaviour
{
    public AllHousePrefab allHousePrefab;
    public static JSON json;
    [HideInInspector] public static StartProject startProject; //поки так
    public static AllData allData;
    [SerializeField] private CanvasHouse canvasHouse;
    [SerializeField] private AnimTimeBuild animTimeBuild;
    [SerializeField] private Notification notification;
    void Awake()
    {
        allHousePrefab.FillInList();
        json = new JSON(notification);
        //NullJSON();
        if (json.Load() != null)
        {
            startProject = StartProject.Continue;
            allData = json.Load();
            CreateReturnHouse();
        }
        else
        {
            startProject = StartProject.Start;
            allData = new AllData()
            {
                allDataHouses = new List<AllDataHouse>(),
                allTypeMoney = new AllTypeMoney()
                {
                    dataMoneyYellow = new DataMoneyJSON(),
                    dataMoneyGreen = new DataMoneyJSON(),
                    dataMoneyBlue = new DataMoneyJSON(),
                },
                dataExp = new DataExpierence(),
            };
        }
        notification.CallNotification("Create");
        void NullJSON()
        {
            allData = null;
            json.Save(allData);
        }
    }
    private void CreateReturnHouse()
    {
        for(int i = 0; i < allData.allDataHouses.Count; i++)
        {
            ReturnHouse(i);
        }
        void ReturnHouse(int I)
        {
            for(int i = 0; i < allHousePrefab.listHouse.Count; i++)
            {
                if (allData.allDataHouses[I].dataHouse.NameThisHouse == allHousePrefab.listHouse[i][0].dataHouse.NameThisHouse)
                {
                    ExistOrNot existOrNot = ExistOrNot.Yes;
                    int diffLevel = 0; // треба для цього початку, може колись щось краще зроблю
                    if(allData.allDataHouses[I].dataHouse.levelHouse == 0){

                        existOrNot = ExistOrNot.Almost; // бо якщо є, то будується
                        diffLevel = 1;
                    }
                    GeneralHouse house = Instantiate(allHousePrefab.listHouse[i][allData.allDataHouses[I].dataHouse.levelHouse - 1 + diffLevel]);
                    house.canvasHouse = canvasHouse;
                    house.dataHouse.myIndexOnSave = I;
                    int x = allData.allDataHouses[I].dataHouse.posit.x;
                    int z = allData.allDataHouses[I].dataHouse.posit.z;
                    house.transform.position = new Vector3(
                        Posit.InitInPosit(x, z, house).x, 
                        house.transform.localScale.y / 2,
                        Posit.InitInPosit(x, z, house).y
                    );
                    house.dataHouse.dataAnimBuildHouse.timeEndBuild = allData.allDataHouses[I].dataHouse.dataAnimBuildHouse.timeEndBuild;
                    TakeObjects.End(x,z,house, false,false);
                    house.existOrNot = existOrNot; // не раніше корутіни, бо там це треба

                    if (house.GetComponent<MainHouse>())
                    {
                        TimeDateTime timeEnd = house.dataHouse.dataAnimBuildHouse.timeEndBuild;
                        int timeEndseconds = TimeDateTime.InSeconds(timeEnd);
                        if (timeEndseconds != 0)
                        {
                            StartCoroutine(animTimeBuild.BeginBuildHouse((MainHouse)house, false));
                        }
                    }
                    house.stateHouse = StateHouse.NotActive;
                    return;
                }
            }
        }
    }
    public void ReturnChangeTextOnButton(GeneralHouse _house)
    {
        _house.houseTextOnShop.dataHouseChangeOnText = new DataHouseChangeOnText(); //щоб те що залишалось в префабах  стерти і заново дати
        for (int i = allData.allDataHouses.Count - 1; i >= 0; i--)
        {
            if (_house.dataHouse.NameThisHouse == allData.allDataHouses[i].dataHouse.NameThisHouse)  //беру підряд по іменам що є в скролі і беру останні дані
            {
                _house.houseTextOnShop.dataHouseChangeOnText = allData.allDataHouses[i].dataHouseChangeOnText;
                break;
            }
            if (i == 0)
            {
                Debug.Log(i + " не знайшов по імені хаус, тому дані для кнопки не дав");
            }
        }
    }

}
