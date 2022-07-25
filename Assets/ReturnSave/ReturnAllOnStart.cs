using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StartProject
{
    Start,
    Continue
}
public class ReturnAllOnStart : MonoBehaviour
{
    public AllHousePrefab allHousePrefab;
    public static JSON json;
    [HideInInspector] public StartProject startProject;
    public static AllData allData;
    [SerializeField] private CanvasHouse canvasHouse;
    void Awake()
    {
        allHousePrefab.FillInList();
        json = new JSON();
        //NullJSON();
        if (json.Load() != null)
        {
            startProject = StartProject.Continue;
            //Debug.Log("считал вродеби");
            allData = json.Load();
            CreateReturnHouse();
        }
        else
        {
            startProject = StartProject.Start;
            allData = new AllData()
            {
                allDataHouses = new List<AllDataHouse>(),
            };
        }
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
                    //Debug.Log(allData.allDataHouses[I].dataHouse.NameThisHouse);
                    House house = Instantiate(allHousePrefab.listHouse[i][allData.allDataHouses[I].dataHouse.levelHouse]);
                    house.canvasHouse = canvasHouse;
                    house.dataHouse.myIndexOnSave = I;
                    int x = allData.allDataHouses[I].dataHouse.posit.x;
                    int z = allData.allDataHouses[I].dataHouse.posit.z;
                    house.transform.position = new Vector3(
                        Posit.InitInPosit(x, z, house).x, 
                        house.transform.localScale.y / 2,
                        Posit.InitInPosit(x, z, house).y
                    );
                    TakeObjects.End(x,z,house, false,false);
                    house.existOrNot = ExistOrNot.Yes;
                    house.stateHouse = StateHouse.NotActive;
                    return;
                }
            }
        }
    }
    public void ReturnChangeTextOnButton(House _house)
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
