using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTouch0 : MonoBehaviour
{
    void Update()
    {
        House house = TakeObjects._house;
        if(house == null) { return; }

        if (Input.touchCount != 0 && house.stateHouse == StateHouse.IsActive && Input.GetTouch(0).phase == TouchPhase.Began/* && !house.dataTextOnHouse.openCanvas*/)
        {
            Debug.Log(Input.GetTouch(0).position);


            Action();
            void Action()
            {
                Debug.Log("h");
                if (house.currentColor == StateColor.Norm)
                {
                    if (!house.existOrNot)
                    {
                        Debug.Log("Add");

                        house.houseTextOnShop.buttonChange.AddCurrentBuildThisHouse(); // по ссилкам все норм, при взятті, даю сюди ссилку на buttonChange кнопки на якій це пишеться і маю тут її дані, і у неї змінюю і свої і її
                                                                                 //Debug.Log(houseTextOnButton.buttonChange.houseTextOnButton.dataHouseChangeOnText.currentBuildThisHouse + " check");
                        AllDataHouse allDataHouse = new AllDataHouse()
                        {
                            dataHouse = new DataHouse()
                            {
                                NameThisHouse = house.dataHouse.NameThisHouse,
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
                    house.endMove = true; //тепер перейде виконувати End
                    house.stateHouse = StateHouse.NotActive;
                    house.ReturnOrInitColor();
                    house.CloseCanvasHouse(house.canvasWindows);
                }
                else if (house.currentColor == StateColor.Red)
                {
                    if (house.existOrNot)
                    {
                        house.ReturnOrInitColor();
                        house.CloseCanvasHouse(house.canvasWindows);
                        house.ReturnCell();
                        house.stateHouse = StateHouse.NotActive;
                    }
                    else
                    {
                        Destroy(house.gameObject);
                    }
                }
                TakeObjects._house = null;
            }
        }
    }
}
