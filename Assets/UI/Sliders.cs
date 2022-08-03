using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliders : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve; // від 1 до 0 в середині і до 5 наверх
    public IEnumerator ValueSliderAnim(float beginValue, float endValue, DataMoney _dataMoney) // для текста меньше кадров сделать(50)
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
            _dataMoney.textCount.text = (currentCountText).ToString() + "/" + _dataMoney.MaxMoney;

            yield return new WaitForSeconds(1f / numberCadr * curve.Evaluate((float)i / (float)numberCadr));

            if (i == numberCadr - 1 && currentCountText != endCount)
            {
                _dataMoney.textCount.text = endCount.ToString() + "/" + _dataMoney.MaxMoney;
            }

            if (direction && _dataMoney.slider.value >= endValue || !direction && _dataMoney.slider.value <= endValue)
            {
                _dataMoney.textCount.text = endCount.ToString() + "/" + _dataMoney.MaxMoney;
                yield break;
            }
        }
    }


    public IEnumerator ValueSliderAnimExp(float beginValue, float endValue, DataExp _dataExp) // для текста меньше кадров сделать(50)
    {
        int numberCadr = 100;

        int startCount = Mathf.RoundToInt(beginValue * _dataExp.GetMaxExpInLevel);
        int endCount = _dataExp.CountExp;
        float diffCount = ((float)endCount - (float)startCount) / numberCadr;

        float diffValue = ((float)endValue - (float)beginValue) / numberCadr;

        bool direction = diffValue > 0 ? true : false;

        float currentCountText = startCount;
        for (int i = 0; i < numberCadr; i++)
        {
            _dataExp.slider.value += diffValue;

            currentCountText += diffCount;
            _dataExp.textCount.text = ((int)currentCountText).ToString() + "/" + _dataExp.GetMaxExpInLevel;

            yield return new WaitForSeconds(1f / numberCadr * curve.Evaluate((float)i / (float)numberCadr));

            if (i == numberCadr - 1 && (int)currentCountText != endCount)
            {
                _dataExp.textCount.text = endCount.ToString() + "/" + _dataExp.GetMaxExpInLevel;
            }

            if (direction && _dataExp.slider.value >= endValue || !direction && _dataExp.slider.value <= endValue)
            {
                _dataExp.textCount.text = endCount.ToString() + "/" + _dataExp.GetMaxExpInLevel;
                yield break;
            }
        }
    }
}
