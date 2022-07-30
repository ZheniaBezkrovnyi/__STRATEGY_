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
    [SerializeField] private DataMoney dataMoneyDollar;
    [SerializeField] private DataMoney dataMoneySilver;
    [SerializeField] private DataMoney dataMoneyGold;

    private void Start() // для инициализации
    {
        Init();
        void Init()
        {
            dataMoneyDollar.OnStart(TypeMoney.Yellow);
            StartCoroutine(ValueSliderAnim(0, (float)dataMoneyDollar.CountMoney/(float)dataMoneyDollar.MaxMoney, dataMoneyDollar));
            dataMoneySilver.OnStart(TypeMoney.Green);
            StartCoroutine(ValueSliderAnim(0, (float)dataMoneySilver.CountMoney / (float)dataMoneySilver.MaxMoney, dataMoneySilver));
            dataMoneyGold.OnStart(TypeMoney.Blue);
            StartCoroutine(ValueSliderAnim(0, (float)dataMoneyGold.CountMoney / (float)dataMoneyGold.MaxMoney, dataMoneyGold));
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
                StartCoroutine(ValueSliderAnim(beginValueSlider, endValueSlider, dataMoneyDollar));
                break;
            case TypeMoney.Green:
                if (dataMoneySilver.CanDoingOperation(sum) == TypeOperation.False) { return; }

                beginValueSlider = dataMoneySilver.slider.value;
                endValueSlider = dataMoneySilver.ChangeMoneyAndGetValue(sum);
                dataMoneySilver.SaveJSONData(TypeMoney.Green);
                StartCoroutine(ValueSliderAnim(beginValueSlider, endValueSlider, dataMoneySilver));
                break;
            case TypeMoney.Blue:
                if (dataMoneyGold.CanDoingOperation(sum) == TypeOperation.False) { return; }

                beginValueSlider = dataMoneyGold.slider.value;
                endValueSlider = dataMoneyGold.ChangeMoneyAndGetValue(sum);
                dataMoneyGold.SaveJSONData(TypeMoney.Blue);
                StartCoroutine(ValueSliderAnim(beginValueSlider, endValueSlider, dataMoneyGold));
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


    private IEnumerator ValueSliderAnim(float beginValue, float endValue, DataMoney _dataMoney) // для текста меньше кадров сделать(50)
    {
        int numberCadr = 100;

        int startCount = Mathf.RoundToInt(beginValue * _dataMoney.MaxMoney);
        int endCount = _dataMoney.CountMoney;
        int diffCount = Mathf.RoundToInt(((float)endCount - (float)startCount) / numberCadr);

        float diffValue = ((float)endValue - (float)beginValue) / numberCadr;


        bool direction = diffValue > 0 ? true : false;

        int currentCountText = startCount;
        for (int i = 0; i < numberCadr; i++)
        {
            _dataMoney.slider.value += diffValue;

            currentCountText += diffCount;
            _dataMoney.textCount.text = (currentCountText).ToString();

            yield return new WaitForSeconds(1f/ numberCadr * curve.Evaluate((float)i/ (float)numberCadr));

            if (i == numberCadr - 1 && currentCountText != endCount)
            {
                _dataMoney.textCount.text = endCount.ToString();
            }

            if(direction && _dataMoney.slider.value >= endValue || !direction && _dataMoney.slider.value <= endValue)
            {
                _dataMoney.textCount.text = endCount.ToString();
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
    public Text textCount;

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
}
