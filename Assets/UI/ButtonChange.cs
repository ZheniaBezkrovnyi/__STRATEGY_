using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChange : MonoBehaviour
{
    public Text textCurrent;
    public Text textTimeBuild;
    public HouseTextOnButton houseTextOnButton;
    public void InitTextCurrent(Text _textCurrentBuild)
    {
        textCurrent = _textCurrentBuild;
        GoStringCountBuild();
    }
    public void GoStringCountBuild()
    {
        textCurrent.text = houseTextOnButton.dataHouseChangeOnText.currentBuildThisHouse.ToString() + " / " + houseTextOnButton.MaxCountBuild.ToString();
    }
    public void InitTextTimeBuild(Text _textTimeBuild)
    {
        textTimeBuild = _textTimeBuild;
        GoStringTimeBuild();
    }
    private void GoStringTimeBuild()
    {
        string day = houseTextOnButton.TimeBuild.days != 0 ? houseTextOnButton.TimeBuild.days.ToString() + "d" : null;
        string hour = houseTextOnButton.TimeBuild.hours != 0 ? houseTextOnButton.TimeBuild.hours.ToString() + "h" : null;
        string minute = houseTextOnButton.TimeBuild.minutes != 0 ? houseTextOnButton.TimeBuild.minutes.ToString() + "m" : null;
        string second = houseTextOnButton.TimeBuild.seconds != 0 ? houseTextOnButton.TimeBuild.seconds.ToString() + "s" : null;
        textTimeBuild.text = day + hour + minute + second;
    }
    public void AddCurrentBuildThisHouse()
    {
        if (houseTextOnButton.dataHouseChangeOnText.currentBuildThisHouse + 1 <= houseTextOnButton.MaxCountBuild)
        {
            ++houseTextOnButton.dataHouseChangeOnText.currentBuildThisHouse;
            GoStringCountBuild();
            if(houseTextOnButton.dataHouseChangeOnText.currentBuildThisHouse == houseTextOnButton.MaxCountBuild)
            {
                GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            Debug.LogError("карент больше максимум зданий");
        }
    }

    public void CheckUpdate() //визвать при обновлении MaxCount и входе в игру 
    {
        if (houseTextOnButton.dataHouseChangeOnText.currentBuildThisHouse != houseTextOnButton.MaxCountBuild)
        {
            GetComponent<Button>().interactable = true;
        }
        else
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
