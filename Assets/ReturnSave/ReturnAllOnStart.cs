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
    public List<House> listHouse;
    public static JSON json;
    public StartProject startProject;
    public static AllData allData;
    public List<AllDataHouse> checkAllData;
    public int posX;
    public int posZ;
    void Awake()
    {
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
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            checkAllData = allData.allDataHouses;
            if (allData.allDataHouses.Count > 0)
            {
                posX = allData.allDataHouses[allData.allDataHouses.Count - 1].dataHouse.posit.x;
                posZ = allData.allDataHouses[allData.allDataHouses.Count - 1].dataHouse.posit.z;
            }
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
            for(int i = 0; i < listHouse.Count; i++)
            {
                if(allData.allDataHouses[I].dataHouse.NameThisHouse == listHouse[i].dataHouse.NameThisHouse)
                {
                    //Debug.Log(allData.allDataHouses[I].dataHouse.NameThisHouse);
                    House house = Instantiate(listHouse[i]);
                    house.dataHouse.myIndexOnSave = I;
                    int x = allData.allDataHouses[I].dataHouse.posit.x;
                    int z = allData.allDataHouses[I].dataHouse.posit.z;
                    house.transform.position = new Vector3(x * MyTerrain.sizeOneCell + MyTerrain.xMin * MyTerrain.sizeOneCell + (float)house.NeParniX / 2f * MyTerrain.sizeOneCell, house.transform.localScale.y / 2, z * MyTerrain.sizeOneCell + MyTerrain.zMin * MyTerrain.sizeOneCell + (float)house.NeParniZ / 2f * MyTerrain.sizeOneCell);
                    TakeObjects.End(x,z,house);
                    house.existOrNot = true;
                    house.dataHouse.posit = new Posit(x,z);
                    house.stateHouse = StateHouse.NotActive;
                    return;
                }
            }
        }
    }
    public void ReturnChangeTextOnButton(House _house, List<AllDataHouse> _allDataHouse)
    {
        for (int i = _allDataHouse.Count - 1; i >= 0; i--)
        {
            if (_house.dataHouse.NameThisHouse == _allDataHouse[i].dataHouse.NameThisHouse) //по імені бо треба вибрати для самого типу будівлі, і беру останній такий, бо зберігав додаючи до нових будів
            {
                _house.houseTextOnButton.dataHouseChangeOnText = _allDataHouse[i].dataHouseChangeOnText; // далі дається ссилка на хаус і він передає ссилку свою на кнопку)))
                break;
            }
            if (i == 0)
            {
                Debug.Log("не знайшов по імені хаус, тому дані для кнопки не дав");
            }
        }
    }

}
