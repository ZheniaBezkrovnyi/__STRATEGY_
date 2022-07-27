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
    [SerializeField] private AnimationCurve curve; // від 1 до 0 в середині і до 5 наверх
    [SerializeField] private DataMoney dataMoneyYellow;
    [SerializeField] private DataMoney dataMoneyGreen;
    [SerializeField] private DataMoney dataMoneyBlue;

    private void Start() // для инициализации
    {
        Init();
        void Init()
        {
            dataMoneyYellow.OnStart(TypeMoney.Yellow);
            StartCoroutine(ValueSliderAnim(0, (float)dataMoneyYellow.CountMoney/(float)dataMoneyYellow.MaxMoney, dataMoneyYellow.slider));
            dataMoneyGreen.OnStart(TypeMoney.Green);
            StartCoroutine(ValueSliderAnim(0, (float)dataMoneyGreen.CountMoney / (float)dataMoneyGreen.MaxMoney, dataMoneyGreen.slider));
            dataMoneyBlue.OnStart(TypeMoney.Blue);
            StartCoroutine(ValueSliderAnim(0, (float)dataMoneyBlue.CountMoney / (float)dataMoneyBlue.MaxMoney, dataMoneyBlue.slider));
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ChangeMoney(1000,(TypeMoney)UnityEngine.Random.Range(0,3));
        }
    }
    public void ChangeMoney(int sum,TypeMoney typeMoney,bool save = true)
    {
        float beginValueSlider;
        float endValueSlider;
        switch (typeMoney)
        {
            case TypeMoney.Yellow:
                if (dataMoneyYellow.CanDoingOperation(sum) == TypeOperation.False) { return; }

                beginValueSlider = dataMoneyYellow.slider.value;
                endValueSlider = dataMoneyYellow.ChangeMoneyAndGetValue(sum);
                dataMoneyYellow.SaveJSONData(TypeMoney.Yellow);
                StartCoroutine(ValueSliderAnim(beginValueSlider, endValueSlider, dataMoneyYellow.slider));
                break;
            case TypeMoney.Green:
                if (dataMoneyGreen.CanDoingOperation(sum) == TypeOperation.False) { return; }

                beginValueSlider = dataMoneyGreen.slider.value;
                endValueSlider = dataMoneyGreen.ChangeMoneyAndGetValue(sum);
                dataMoneyGreen.SaveJSONData(TypeMoney.Green);
                StartCoroutine(ValueSliderAnim(beginValueSlider, endValueSlider, dataMoneyGreen.slider));
                break;
            case TypeMoney.Blue:
                if (dataMoneyBlue.CanDoingOperation(sum) == TypeOperation.False) { return; }

                beginValueSlider = dataMoneyBlue.slider.value;
                endValueSlider = dataMoneyBlue.ChangeMoneyAndGetValue(sum);
                dataMoneyBlue.SaveJSONData(TypeMoney.Blue);
                StartCoroutine(ValueSliderAnim(beginValueSlider, endValueSlider, dataMoneyBlue.slider));
                break;
        }
    }

    public TypeOperation CanDoingOperation(int sum, TypeMoney typeMoney, bool notIgnore = false)
    {
        switch (typeMoney)
        {
            case TypeMoney.Yellow:
                return dataMoneyYellow.CanDoingOperation(sum);
            case TypeMoney.Green:
                return dataMoneyGreen.CanDoingOperation(sum);
            case TypeMoney.Blue:
                return dataMoneyBlue.CanDoingOperation(sum);
        }
        return TypeOperation.False;
    }


    private IEnumerator ValueSliderAnim(float beginValue, float endValue, Slider slider)
    {
        float diffValue = (endValue - beginValue) / 100f;
        bool direction = diffValue > 0 ? true : false;

        for (int i = 0; i < 100; i++)
        {
            slider.value += diffValue;
            yield return new WaitForSeconds(1f/100*curve.Evaluate((float)i/100f));

            if(direction && slider.value >= endValue || !direction && slider.value <= endValue)
            {
                yield break;
            }
        }
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

    public void OnStart(TypeMoney typeMoney)
    {
        if (ReturnAllOnStart.startProject == StartProject.Continue) // потомучто если тут 0, то еще не сохранял
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
}
