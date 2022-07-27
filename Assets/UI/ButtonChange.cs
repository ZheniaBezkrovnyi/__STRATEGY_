using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChange : MonoBehaviour
{
    public Text textCurrent;
    public Text textTimeBuild;
    public Text textPrice;
    public HouseTextOnShop houseTextOnShop;
    public void InitText(Text _text,TypeTextOnButton typeText) // alignment можна потом при ініциалізації вказувать
    {
        switch (typeText)
        {
            case TypeTextOnButton.CountHouse:
                _text.alignment = TextAnchor.MiddleRight;
                GoStringCountBuild(_text);
                break;
            case TypeTextOnButton.TimeBuild:
                _text.alignment = TextAnchor.MiddleLeft;
                GoStringTimeBuild(_text);
                break;
            case TypeTextOnButton.Price:
                GoStringPrice(_text);
                break;
        }
    }
    private void GoStringCountBuild(Text _text)
    {
        textCurrent = _text;
        textCurrent.text = houseTextOnShop.dataHouseChangeOnText.currentBuildThisHouse.ToString() + " / " + houseTextOnShop.MaxCountBuild.ToString();
    }
    private void GoStringTimeBuild(Text _text)
    {
        textTimeBuild = _text;
        string day = houseTextOnShop.TimeNeedBuildStart.days != 0 ? houseTextOnShop.TimeNeedBuildStart.days.ToString() + "d" : null;
        string hour = houseTextOnShop.TimeNeedBuildStart.hours != 0 ? houseTextOnShop.TimeNeedBuildStart.hours.ToString() + "h" : null;
        string minute = houseTextOnShop.TimeNeedBuildStart.minutes != 0 ? houseTextOnShop.TimeNeedBuildStart.minutes.ToString() + "m" : null;
        string second = houseTextOnShop.TimeNeedBuildStart.seconds != 0 ? houseTextOnShop.TimeNeedBuildStart.seconds.ToString() + "s" : null;
        textTimeBuild.text = day + hour + minute + second;
    }
    private void GoStringPrice(Text _text)
    {
        textPrice = _text;
        textPrice.GetComponent<RectTransform>().localScale += new Vector3(0.2f,0.2f,0);
        textPrice.text = houseTextOnShop.priceForBuild.ToString();
    }
    public void AddCurrentBuildThisHouse()
    {
        if (houseTextOnShop.dataHouseChangeOnText.currentBuildThisHouse + 1 <= houseTextOnShop.MaxCountBuild)
        {
            ++houseTextOnShop.dataHouseChangeOnText.currentBuildThisHouse;
            GoStringCountBuild(textCurrent);
            if(houseTextOnShop.dataHouseChangeOnText.currentBuildThisHouse == houseTextOnShop.MaxCountBuild)
            {
                GetComponent<Button>().image.color = new Color(0.6f, 0.6f, 0.6f, 1f);
            }
        }
        else
        {
            Debug.LogError("карент больше максимум зданий");
        }
    }

    public void CheckUpdate() //визвать при обновлении MaxCount и входе в игру 
    {
        if (houseTextOnShop.dataHouseChangeOnText.currentBuildThisHouse != houseTextOnShop.MaxCountBuild)
        {
            GetComponent<Button>().image.color = new Color(1, 1, 1, 1);
        }
        else
        {
            GetComponent<Button>().image.color = new Color(0.6f, 0.6f, 0.6f, 1f);
        }
    }
}
