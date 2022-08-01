using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class AllHousePrefab : ScriptableObject
{
    [HideInInspector] public List<List<GeneralHouse>> listHouse;
    [SerializeField] private List<GeneralHouse> list0;
    [SerializeField] private List<GeneralHouse> list1;
    [SerializeField] private List<GeneralHouse> list2;
    [SerializeField] private List<GeneralHouse> list3;
    [SerializeField] private List<GeneralHouse> list4;

    public void FillInList()
    {
        listHouse = new List<List<GeneralHouse>>();
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
