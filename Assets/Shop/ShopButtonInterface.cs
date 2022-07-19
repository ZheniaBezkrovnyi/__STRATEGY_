using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TypeTextOnButton
{
    CountHouse,
    TimeBuild,

}
public class ShopButtonInterface : MonoBehaviour
{
    private Text text;
    private Vector2[] listPosText;
    private TypeTextOnButton typeText;
    public ShopButtonInterface(Text _text) // тепер можу просто вписувать значення і правильно добавиться, значення ставити, від правого нижного куту кнопок,однакова послідовність з енамом
    {
        listPosText = new Vector2[2] {
            new Vector2(5,5),
            new Vector2(145,5),
        };
        text = _text;

    }
    public void InitialiseTexts(Button button,House house)
    {
        RectTransform rectButton = button.GetComponent<RectTransform>();
        float diffX = rectButton.rect.width / 2;
        float diffY = -rectButton.rect.height / 2;
        for (int i = 0; i < listPosText.Length; i++)
        {
            typeText = (TypeTextOnButton)i;
            Text _text = Instantiate(text);
            _text.gameObject.SetActive(true);

            RectTransform rectText = _text.rectTransform;
            rectText.SetParent(button.transform);

            house.InitData();
            SwithEnumBetweenText(typeText, house.dataTextOnButton, _text);
            rectText.position = new Vector3(rectButton.rect.width - rectText.rect.width / 2 - diffX - listPosText[i].x, -rectButton.rect.height + rectText.rect.height / 2 - diffY + listPosText[i].y, 0);
        }

        void SwithEnumBetweenText(TypeTextOnButton type, DataHouseTextOnButton data,Text Text)
        {
            switch (type)
            {
                case TypeTextOnButton.CountHouse:
                    //Debug.Log(data.MaxCountBuild);
                    Text.text = "0 / " + data.MaxCountBuild.ToString();
                        break;
                case TypeTextOnButton.TimeBuild:
                    string day = data.TimeBuild.days != 0 ? data.TimeBuild.days.ToString() + "d" : null;
                    string hour = data.TimeBuild.hours != 0 ? data.TimeBuild.hours.ToString() + "h" : null;
                    string minute = data.TimeBuild.minutes != 0 ? data.TimeBuild.minutes.ToString() + "m" : null;
                    string second = data.TimeBuild.seconds != 0 ? data.TimeBuild.seconds.ToString() + "s" : null;
                    Text.text = day + hour + minute + second;
                    break;
            }
        }
    }
}
public class TimeBuild
{
    public int days,hours,minutes,seconds;
    public TimeBuild(int _days, int _hours, int _minutes, int _seconds)
    {
        days = _days;

        if (_hours > 23) _hours = 23;
        if (_hours < 0) _hours = 0;
        hours = _hours;

        if (_minutes > 59) _minutes = 59;
        if (_minutes < 0) _minutes = 0;
        minutes = _minutes;

        if (_seconds > 59) _seconds = 59;
        if (_seconds < 0) _seconds = 0;
        seconds = _seconds;
    }
}