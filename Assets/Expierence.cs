using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class Expierence : MonoBehaviour
{
    [SerializeField] private Sliders sliders;
    [SerializeField] private DataExp dataExp;
    private void Start()
    {
        dataExp.OnStart();
        StartCoroutine(sliders.ValueSliderAnimExp(0, (float)dataExp.CountExp/ (float)dataExp.GetMaxExpInLevel, dataExp));
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ChangeExp(20);
        }
    }
    public void ChangeExp(int sum)
    {
        float beginValueSlider = dataExp.slider.value;
        float endValueSlider = dataExp.ChangeExpAndGetValue(sum);

        dataExp.SaveJSONData();

        StartCoroutine(sliders.ValueSliderAnimExp(beginValueSlider, endValueSlider, dataExp));
    }

}
[Serializable]
public class DataExp
{
    [SerializeField] private int countExp;
    public int CountExp { get { return countExp; } }

    [SerializeField] private int countLevel;
    public int CountLevel { get { return countLevel; } }

    public int GetMaxExpInLevel { get { return 100 + 50 * CountLevel; } }

    public Slider slider;
    public Text textCount;
    public Text textLevel;
    public void OnStart()
    {
        if (ReturnAllOnStart.startProject == StartProject.Continue)
        {
            GetJSONData();
        }
        textCount.text = CountExp + "/" + GetMaxExpInLevel;
        textLevel.text = CountLevel.ToString();
    }
    
    public float ChangeExpAndGetValue(int sum)
    {
        countExp += sum;

        ChangeLevel();

        return (float)countExp / (float)GetMaxExpInLevel;

        void ChangeLevel()
        {
            if (countExp > GetMaxExpInLevel)
            {
                countExp -= GetMaxExpInLevel;
                countLevel++;
                textLevel.text = CountLevel.ToString();
            }
            else
            {
                return;
            }
            ChangeLevel();
        }
    }




    private void GetJSONData() 
    {
        countExp = ReturnAllOnStart.allData.dataExp.countExp;
        countLevel = ReturnAllOnStart.allData.dataExp.countLevel;
    }
    public void SaveJSONData()
    {
        ReturnAllOnStart.allData.dataExp.countExp = countExp;
        ReturnAllOnStart.allData.dataExp.countLevel = countLevel;

        ReturnAllOnStart.json.Save(ReturnAllOnStart.allData);
    }

}
