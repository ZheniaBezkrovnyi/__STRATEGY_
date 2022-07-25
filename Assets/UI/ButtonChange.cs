using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChange : MonoBehaviour
{
    public Text textCurrent;
    public Text textTimeBuild;
    public HouseTextOnShop houseTextOnShop;
    public void InitTextCurrent(Text _textCurrentBuild)
    {
        textCurrent = _textCurrentBuild;
        GoStringCountBuild();
    }
    public void GoStringCountBuild()
    {
        textCurrent.text = houseTextOnShop.dataHouseChangeOnText.currentBuildThisHouse.ToString() + " / " + houseTextOnShop.MaxCountBuild.ToString();
    }
    public void InitTextTimeBuild(Text _textTimeBuild)
    {
        textTimeBuild = _textTimeBuild;
        GoStringTimeBuild();
    }
    private void GoStringTimeBuild()
    {
        string day = houseTextOnShop.TimeNeedBuildStart.days != 0 ? houseTextOnShop.TimeNeedBuildStart.days.ToString() + "d" : null;
        string hour = houseTextOnShop.TimeNeedBuildStart.hours != 0 ? houseTextOnShop.TimeNeedBuildStart.hours.ToString() + "h" : null;
        string minute = houseTextOnShop.TimeNeedBuildStart.minutes != 0 ? houseTextOnShop.TimeNeedBuildStart.minutes.ToString() + "m" : null;
        string second = houseTextOnShop.TimeNeedBuildStart.seconds != 0 ? houseTextOnShop.TimeNeedBuildStart.seconds.ToString() + "s" : null;
        textTimeBuild.text = day + hour + minute + second;
    }
    public void AddCurrentBuildThisHouse()
    {
        if (houseTextOnShop.dataHouseChangeOnText.currentBuildThisHouse + 1 <= houseTextOnShop.MaxCountBuild)
        {
            ++houseTextOnShop.dataHouseChangeOnText.currentBuildThisHouse;
            GoStringCountBuild();
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
