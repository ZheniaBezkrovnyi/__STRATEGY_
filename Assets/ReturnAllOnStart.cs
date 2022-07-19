using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnAllOnStart : MonoBehaviour
{
    [SerializeField] private Shop shop;
    [SerializeField] private JSON json;
    public static AllData allData;
    void Awake()
    {
        if (json.Load() != null)
        {
            allData = json.Load();
            CreateReturnHouse();
        }
        else
        {
            allData = new AllData();
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
            for(int i = 0; i < shop.listHouse.Count; i++)
            {
                if(allData.allDataHouses[I].dataHouse.NameThisHouse == shop.listHouse[i].dataHouse.NameThisHouse)
                {
                    House house = Instantiate(shop.listHouse[i]);
                    int x = allData.allDataHouses[I].dataHouse.posit.x;
                    int z = allData.allDataHouses[I].dataHouse.posit.z;
                    house.transform.position = new Vector3(x * MyTerrain.sizeOneCell + MyTerrain.xMin * MyTerrain.sizeOneCell + (float)house.NeParniX / 2f * MyTerrain.sizeOneCell, house.transform.localScale.y / 2, z * MyTerrain.sizeOneCell + MyTerrain.zMin * MyTerrain.sizeOneCell + (float)house.NeParniZ / 2f * MyTerrain.sizeOneCell);
                    TakeObjects.End(x,z,house);
                    house.existOrNot = true;
                    house.dataHouse.myIndexOnSave = I;
                    return;
                }
            }
        }
    }
}
