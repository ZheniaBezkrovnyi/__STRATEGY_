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
        Init();
        void Init()
        {
            dataMoneyDollar.InitColor(new Color32(0,255,50,255));
            dataMoneyDollar.OnStart(TypeMoney.Yellow);
            StartCoroutine(sliders.ValueSliderAnim(0, (float)dataMoneyDollar.CountMoney/(float)dataMoneyDollar.MaxMoney, dataMoneyDollar));
            dataMoneySilver.OnStart(TypeMoney.Green);
            StartCoroutine(sliders.ValueSliderAnim(0, (float)dataMoneySilver.CountMoney / (float)dataMoneySilver.MaxMoney, dataMoneySilver));
            dataMoneySilver.InitColor(new Color32(195, 195, 195, 255));
            dataMoneyGold.OnStart(TypeMoney.Blue);
            StartCoroutine(sliders.ValueSliderAnim(0, (float)dataMoneyGold.CountMoney / (float)dataMoneyGold.MaxMoney, dataMoneyGold));
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
        float beginValueSlider;
        float endValueSlider;
        switch (typeMoney)
        {
            case TypeMoney.Yellow:
                if (dataMoneyDollar.CanDoingOperation(sum) == TypeOperation.False) { return; }

                beginValueSlider = dataMoneyDollar.slider.value;
                endValueSlider = dataMoneyDollar.ChangeMoneyAndGetValue(sum);
                dataMoneyDollar.SaveJSONData(TypeMoney.Yellow);
                StartCoroutine(sliders.ValueSliderAnim(beginValueSlider, endValueSlider, dataMoneyDollar));
                break;
            case TypeMoney.Green:
                if (dataMoneySilver.CanDoingOperation(sum) == TypeOperation.False) { return; }

                beginValueSlider = dataMoneySilver.slider.value;
                endValueSlider = dataMoneySilver.ChangeMoneyAndGetValue(sum);
                dataMoneySilver.SaveJSONData(TypeMoney.Green);
                StartCoroutine(sliders.ValueSliderAnim(beginValueSlider, endValueSlider, dataMoneySilver));
                break;
            case TypeMoney.Blue:
                if (dataMoneyGold.CanDoingOperation(sum) == TypeOperation.False) { return; }

                beginValueSlider = dataMoneyGold.slider.value;
                endValueSlider = dataMoneyGold.ChangeMoneyAndGetValue(sum);
                dataMoneyGold.SaveJSONData(TypeMoney.Blue);
                StartCoroutine(sliders.ValueSliderAnim(beginValueSlider, endValueSlider, dataMoneyGold));
                break;
        }
    }

    public TypeOperation CanDoingOperation(int sum, TypeMoney typeMoney, bool notIgnore = false)
    {
        switch (typeMoney)
        {
            case TypeMoney.Yellow:
                return dataMoneyDollar.CanDoingOperation(sum);
            case TypeMoney.Green:
                return dataMoneySilver.CanDoingOperation(sum);
            case TypeMoney.Blue:
                return dataMoneyGold.CanDoingOperation(sum);
        }
        return TypeOperation.False;
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
        switch (typeMoney)
        {
            case TypeMoney.Yellow:
                ReturnAllOnStart.allData.allTypeMoney.dataMoneyYellow.countMoney = countMoney;
                ReturnAllOnStart.allData.allTypeMoney.dataMoneyYellow.maxMoney = maxMoney;
                break;
            case TypeMoney.Green:
                ReturnAllOnStart.allData.allTypeMoney.dataMoneyGreen.countMoney = countMoney;
                ReturnAllOnStart.allData.allTypeMoney.dataMoneyGreen.maxMoney = maxMoney;
                break;
            case TypeMoney.Blue:
                ReturnAllOnStart.allData.allTypeMoney.dataMoneyBlue.countMoney = countMoney;
                ReturnAllOnStart.allData.allTypeMoney.dataMoneyBlue.maxMoney = maxMoney;
                break;
        }
        ReturnAllOnStart.json.Save(ReturnAllOnStart.allData);
    }
    private void GetJSONData(TypeMoney typeMoney) // потім оптимізую
    {
        switch (typeMoney)
        {
            case TypeMoney.Yellow:
                maxMoney = ReturnAllOnStart.allData.allTypeMoney.dataMoneyYellow.maxMoney;
                countMoney = ReturnAllOnStart.allData.allTypeMoney.dataMoneyYellow.countMoney; 
                break;
            case TypeMoney.Green:
                maxMoney = ReturnAllOnStart.allData.allTypeMoney.dataMoneyGreen.maxMoney;
                countMoney = ReturnAllOnStart.allData.allTypeMoney.dataMoneyGreen.countMoney;
                break;
            case TypeMoney.Blue:
                maxMoney = ReturnAllOnStart.allData.allTypeMoney.dataMoneyBlue.maxMoney;
                countMoney = ReturnAllOnStart.allData.allTypeMoney.dataMoneyBlue.countMoney;
                break;
        }
        if (countMoney > maxMoney)
        {
            countMoney = 0;
        }
    }

    public void InitColor(Color _color)
    {
        color = _color;
    }
}
