using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public enum TypeTextOnButton
{
    CountHouse,
    TimeBuild,
    Price
}
public class ShopButtonInterface : MonoBehaviour
{
    private Text[] text;
    private TypeTextOnButton typeText;
    public ShopButtonInterface(params Text[] _text) // тепер можу просто вписувать значення і правильно добавиться, значення ставити, від правого нижного куту кнопок,однакова послідовність з енамом
    {
        text = _text;
    }
    public void InitialiseTexts(GeneralHouse house,ButtonChange buttonChange)
    {
        buttonChange.houseTextOnShop = house.houseTextOnShop;

        for (int i = 0; i < text.Length; i++)
        {
            CreateText(i);
        }


        void CreateText(int I)
        {
            typeText = (TypeTextOnButton)I;
            Text _text = text[I];

            SwithEnumBetweenText(typeText, buttonChange, _text);
        }

        void SwithEnumBetweenText(TypeTextOnButton type, ButtonChange _buttonChange, Text Text)
        {
            _buttonChange.InitText(Text,type);
        }
    }
}
