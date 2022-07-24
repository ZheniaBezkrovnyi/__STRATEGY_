﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum NameHouse
{
    A,
    B,
    C,
    D,
    Wall,
    F,
    G
}
public class Shop : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private Canvas canvasShop; // процент влияет на pos
    [SerializeField] private RectTransform canvas;
    [SerializeField] private CameraMove cameraMove;
    [SerializeField] private RectTransform buttonPrefab;
    [SerializeField] private Text textOnButton;
    [SerializeField] private List<NameHouse> listShop;
    [SerializeField] private ReturnAllOnStart returnAllStart;
    [SerializeField] private MyTerrain terrain;
    private ShopButtonInterface shopInterface;
    [SerializeField] private Notification notification;
    private void Start()
    {
        localScaleCanvas = canvas.localScale;
        shopInterface = new ShopButtonInterface(textOnButton);
        InitializeShop();
    }
    private Vector3 localScaleCanvas;
    private void InitializeShop()
    {
        float heightCanvas = canvas.rect.height * canvas.localScale.y;
        float widthCanvas = canvas.rect.width * canvas.localScale.x;
        float heightContent = heightCanvas * 0.9f;
        float widthContent = widthCanvas * 0.9f;
        float diffX = widthCanvas * 0.05f;
        float diffY = heightCanvas * 0.95f;
        float DiffContent = (50*2 + buttonPrefab.rect.width * canvas.localScale.x + (listShop.Count - 1) * (buttonPrefab.rect.width * canvas.localScale.x + 30))/ widthContent;
        content.anchorMax = new Vector2(DiffContent,1);

        for (int i = 0; i < listShop.Count; i++)
        {
            CreateButton(i);
        }



        void CreateButton(int I)
        {
            House _house = null;
            for (int i = 0; i < returnAllStart.allHousePrefab.listHouse.Count; i++) //з енама витягується, все норм тут, але в майбутньому оптимізувати
            {
                if (listShop[I] == returnAllStart.allHousePrefab.listHouse[i][0].dataHouse.NameThisHouse)
                {
                    _house = returnAllStart.allHousePrefab.listHouse[i][0]; 
                    break;
                }
                if( i == returnAllStart.allHousePrefab.listHouse.Count - 1)
                {
                    Debug.Log("not exist " + listShop[I]);
                    return;
                }
            }

            returnAllStart.ReturnChangeTextOnButton(_house);

            RectTransform button = Instantiate(buttonPrefab);
            Button _button = button.GetComponent<Button>();
            ButtonChange buttonChange = _button.GetComponent<ButtonChange>();


            shopInterface.InitialiseTexts(_button, _house, buttonChange);
            buttonChange.CheckUpdate(); // для interactable

            void OnButton()
            {
                if(_button.image.color == new Color(1, 1, 1, 1))
                {
                    terrain.TakeHouse(_house, buttonChange);
                    canvasShop.gameObject.SetActive(false);
                    CameraMove.possibleMove = true;
                }
                else
                {
                    notification.CallNotification("максимальное количество зданий уже построено");
                }
            }
            _button.onClick.AddListener(OnButton);
            Text text = button?.GetChild(0).GetComponent<Text>();
            text.text = listShop[I].ToString();

            button.gameObject.SetActive(true);
            button.SetParent(content);
            button.position = new Vector3(50 + diffX + button.rect.width / 2 * canvas.localScale.x + I*(button.rect.width * canvas.localScale.x + 30), diffY - button.rect.height / 2*canvas.localScale.y - heihtPadding(button), 0);
            button.localScale = new Vector3(1, 1, 1);
        }

        float heihtPadding(RectTransform button)
        {
            //Debug.Log(heightContent);
            if (heightContent - button.rect.height * canvas.localScale.y / 2 > 0)
            {
                float dif = (heightContent - button.rect.height * canvas.localScale.y) / 2 * 0.85f;
                return dif;
            }
            else
            {
                return 0;
            }
        }
    }
}
