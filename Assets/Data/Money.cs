using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public enum TypeMoney
{
    Yellow,
    Green,
    Blue
}
public enum TypeOperation
{
    True,
    False,
    More
}
public class Money : MonoBehaviour
{
    [SerializeField] private DataMoney dataMoneyDollar;
    [SerializeField] private DataMoney dataMoneySilver;
    [SerializeField] private DataMoney dataMoneyGold;
    [SerializeField] private Sliders sliders;
    private void Start() // для инициализации
    {
        InitColor();
        for(int i = 0; i < 3; i++) {
            Init((TypeMoney)i);
        }
        void Init(TypeMoney typeMoney)
        {
            GetDataMoney(typeMoney).OnStart(typeMoney);
            StartCoroutine(sliders.ValueSliderAnim(0, (float)GetDataMoney(typeMoney).CountMoney/(float)GetDataMoney(typeMoney).MaxMoney, GetDataMoney(typeMoney)));
        }
        void InitColor()
        {
            dataMoneyDollar.InitColor(new Color32(0, 255, 50, 255));
            dataMoneySilver.InitColor(new Color32(195, 195, 195, 255));
            dataMoneyGold.InitColor(new Color32(255, 242, 0, 255));
        }
    }
    private int time;
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ChangeMoney(1000,(TypeMoney)UnityEngine.Random.Range(0,3));
        }
#if UNITY_ANDROID && !UNITY_EDITOR
        if (Time.time >= time + 1)
        {
            time++;
            ChangeMoney(100, (TypeMoney)UnityEngine.Random.Range(0, 3));
        }
#endif
    }
    public void ChangeMoney(int sum,TypeMoney typeMoney,bool save = true) 
    {
        if (GetDataMoney(typeMoney).CanDoingOperation(sum) == TypeOperation.False) { return; }
        float beginValueSlider = GetDataMoney(typeMoney).slider.value;
        float endValueSlider = GetDataMoney(typeMoney).ChangeMoneyAndGetValue(sum);
        GetDataMoney(typeMoney).SaveJSONData(typeMoney);
        StartCoroutine(sliders.ValueSliderAnim(beginValueSlider, endValueSlider, GetDataMoney(typeMoney)));
    }

    public TypeOperation CanDoingOperation(int sum, TypeMoney typeMoney, bool notIgnore = false)
    {
        return GetDataMoney(typeMoney).CanDoingOperation(sum,notIgnore);
    }
    public DataMoney GetDataMoney(TypeMoney typeMoney)
    {
        switch (typeMoney)
        {
            case TypeMoney.Yellow:
                return dataMoneyDollar;
            case TypeMoney.Green:
                return dataMoneySilver;
            case TypeMoney.Blue:
                return dataMoneyGold;
        }
        return null;
    }
}

[Serializable]
public class DataMoney
{
    [SerializeField] private int countMoney;
    public int CountMoney { get { return countMoney; } }
    [SerializeField] private int maxMoney;
    public int MaxMoney { get { return maxMoney; } }
    public Slider slider;
    public Text textCount;
    private Color color;
    public Color Color { get { return color; } }
    public void OnStart(TypeMoney typeMoney)
    {
        if (ReturnAllOnStart.startProject == StartProject.Continue) 
        {
            GetJSONData(typeMoney);
        }
        else
        {
            SaveJSONData(typeMoney);
        }
    }

    public float ChangeMoneyAndGetValue(int sum, bool notIgnore = false)
    {
        float startValueSlider = (float)countMoney / (float)maxMoney;
        if (CanDoingOperation(sum, notIgnore) == TypeOperation.True)
        {
            countMoney += sum;
            return (float)countMoney / (float)maxMoney;
        }else if(CanDoingOperation(sum, notIgnore) == TypeOperation.More)
        {
            countMoney = maxMoney;
            return 1f;
        }
        else
        {
            return startValueSlider;
        }
    }

    public TypeOperation CanDoingOperation(int sum, bool notIgnore = false)
    {
        bool plusMoney = sum > 0 ? true : false;
        if (plusMoney)
        {
            if (countMoney + sum <= maxMoney)
            {
                return TypeOperation.True;
            }
            else if (!notIgnore)
            {
                return TypeOperation.More;
            }
            Debug.Log("перебор сумми");
            return TypeOperation.False;
        }
        else
        {
            if (countMoney + sum >= 0)
            {
                return TypeOperation.True;
            }
            else
            {
                Debug.Log("не хватает мані");
                return TypeOperation.False;
            }
        }
    }


    public void SaveJSONData(TypeMoney typeMoney) // потім оптимізую
    {
        GetDataMoney(typeMoney).countMoney = countMoney;
        GetDataMoney(typeMoney).maxMoney = maxMoney;

        ReturnAllOnStart.json.Save(ReturnAllOnStart.allData);
    }
    private void GetJSONData(TypeMoney typeMoney) // потім оптимізую
    {
        maxMoney = GetDataMoney(typeMoney).maxMoney;
        countMoney = GetDataMoney(typeMoney).countMoney;
        if (countMoney > maxMoney)
        {
            countMoney = 0;
        }
    }

    public void InitColor(Color _color)
    {
        color = _color;
    }
    public DataMoneyJSON GetDataMoney(TypeMoney typeMoney)
    {
        switch (typeMoney)
        {
            case TypeMoney.Yellow: return ReturnAllOnStart.allData.allTypeMoney.dataMoneyYellow;
            case TypeMoney.Green: return ReturnAllOnStart.allData.allTypeMoney.dataMoneyGreen;
            case TypeMoney.Blue: return ReturnAllOnStart.allData.allTypeMoney.dataMoneyBlue;
        }
        return null;
    }
}
