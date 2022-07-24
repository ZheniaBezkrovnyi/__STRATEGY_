using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
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
            new Vector2(20,14),
            new Vector2(330,14),
        };
        text = _text;

    }
    public void InitialiseTexts(Button button,House house,ButtonChange buttonChange)
    {
        RectTransform rectButton = button.GetComponent<RectTransform>();
        float diffX = rectButton.rect.width / 2;
        float diffY = -rectButton.rect.height / 2;

        buttonChange.houseTextOnShop = house.houseTextOnShop;

        for (int i = 0; i < listPosText.Length; i++)
        {
            CreateText(i);
        }


        void CreateText(int I)
        {
            typeText = (TypeTextOnButton)I;
            Text _text = Instantiate(text);
            _text.gameObject.SetActive(true);

            RectTransform rectText = _text.rectTransform;
            rectText.SetParent(button.transform);
            rectText.position = new Vector3(rectButton.rect.width - rectText.rect.width / 2 - diffX - listPosText[I].x, -rectButton.rect.height + rectText.rect.height / 2 - diffY + listPosText[I].y, 0);

            SwithEnumBetweenText(typeText, buttonChange, _text);
        }

        void SwithEnumBetweenText(TypeTextOnButton type, ButtonChange _buttonChange, Text Text)
        {
            switch (type)
            {
                case TypeTextOnButton.CountHouse:
                    Text.alignment = TextAnchor.MiddleRight;
                    _buttonChange.InitTextCurrent(Text);
                        break;
                case TypeTextOnButton.TimeBuild:
                    Text.alignment = TextAnchor.MiddleLeft;
                    _buttonChange.InitTextTimeBuild(Text);
                    break;
            }
        }
    }
}
[Serializable]
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
    public static string GoStringTimeBuild(TimeBuild timeBuild)
    {
        string day = timeBuild.days != 0 ? timeBuild.days.ToString() + "d" : null;
        string hour = timeBuild.hours != 0 ? timeBuild.hours.ToString() + "h" : null;
        string minute = timeBuild.minutes != 0 ? timeBuild.minutes.ToString() + "m" : null;
        string second = timeBuild.seconds != 0 ? timeBuild.seconds.ToString() + "s" : null;
        return day + hour + minute + second;
    }

}