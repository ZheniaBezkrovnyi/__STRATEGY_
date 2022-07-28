using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class AllHousePrefab : ScriptableObject
{
    [HideInInspector] public List<List<House>> listHouse;
    [SerializeField] private List<House> list0;
    [SerializeField] private List<House> list1;
    [SerializeField] private List<House> list2;
    [SerializeField] private List<House> list3;
    [SerializeField] private List<House> list4;

    public void FillInList()
    {
        listHouse = new List<List<House>>();
        listHouse.Add(list0);
        listHouse.Add(list1);
        listHouse.Add(list2);
        listHouse.Add(list3);
        listHouse.Add(list4);
        if(listHouse.Count > CountEnum())
        {
            Debug.LogError("не верное количество листов префабов,добавь лист");
        }
    }
    private int CountEnum()
    {
        int count = 0;
        for(int i = 0; i <= count; i++)
        {
            if (Enum.IsDefined(typeof(NameHouse), i))
            {
                count++;
            }
        }
        return count;
    }
}
