using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChange : MonoBehaviour
{
    public Text textCurrent;
    public Text textTimeBuild;
    public DataHouseTextOnButton data;
    public void InitTextCurrent(Text _textCurrentBuild)
    {
        textCurrent = _textCurrentBuild;
        GoStringCountBuild();
    }
    public void GoStringCountBuild()
    {
        textCurrent.text = data.currentBuildThisHouse.ToString() + " / " + data.MaxCountBuild.ToString();
    }
    public void InitTextTimeBuild(Text _textTimeBuild)
    {
        textTimeBuild = _textTimeBuild;
        GoStringTimeBuild();
    }
    private void GoStringTimeBuild()
    {
        string day = data.TimeBuild.days != 0 ? data.TimeBuild.days.ToString() + "d" : null;
        string hour = data.TimeBuild.hours != 0 ? data.TimeBuild.hours.ToString() + "h" : null;
        string minute = data.TimeBuild.minutes != 0 ? data.TimeBuild.minutes.ToString() + "m" : null;
        string second = data.TimeBuild.seconds != 0 ? data.TimeBuild.seconds.ToString() + "s" : null;
        textTimeBuild.text = day + hour + minute + second;
    }
    public void AddCurrentBuildThisHouse(NameHouse name)
    {
        if (data.currentBuildThisHouse + 1 <= data.MaxCountBuild)
        {
            ++data.currentBuildThisHouse;
            //PlayerPr.InitInt("count" + name.ToString(), ++currentBuildThisHouse);
            GoStringCountBuild();
        }
    }
}
