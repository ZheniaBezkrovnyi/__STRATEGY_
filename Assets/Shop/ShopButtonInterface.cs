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
    private Text text;
    private Vector2[] listPosText;
    private TypeTextOnButton typeText;
    public ShopButtonInterface(Text _text) // тепер можу просто вписувать значення і правильно добавиться, значення ставити, від правого нижного куту кнопок,однакова послідовність з енамом
    {
        listPosText = new Vector2[3] {
            new Vector2(20,54),
            new Vector2(330,54),
            new Vector2(175,24)
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
            _buttonChange.InitText(Text,type);
        }
    }
}
