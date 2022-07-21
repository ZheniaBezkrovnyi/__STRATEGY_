using System.Collections;
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
    E,
    F,
    G
}
public class Shop : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private RectTransform scrollRect; // процент влияет на pos
    [SerializeField] private RectTransform canvas;
    [SerializeField] private CameraMove cameraMove;
    [SerializeField] private RectTransform buttonPrefab;
    [SerializeField] private Text textOnButton;
    [SerializeField] private List<NameHouse> listShop;
    [SerializeField] private ReturnAllOnStart returnAllStart;
    [SerializeField] private MyTerrain terrain;
    private ShopButtonInterface shopInterface;
    private void Start()
    {
        shopInterface = new ShopButtonInterface(textOnButton);
        InitializeShop();
    }
    private void InitializeShop()
    {
        float diffX = canvas.rect.width * scrollRect.anchorMin.y + buttonPrefab.rect.width / 2; ;
        float diffY = canvas.rect.height * scrollRect.anchorMax.y;

        float DiffContent = (diffX + 50 + (listShop.Count - 1) * (buttonPrefab.rect.width + 30) + buttonPrefab.rect.width/2) / (canvas.rect.width * (scrollRect.anchorMax.y - scrollRect.anchorMin.y));
        content.anchorMax = new Vector2(DiffContent,1);

        List<AllDataHouse> allDataHouse = ReturnAllOnStart.allData.allDataHouses;

        for (int i = 0; i < listShop.Count; i++)
        {
            CreateButton(i);
        }



        void CreateButton(int I)
        {
            House _house = null;
            for (int i = 0; i < returnAllStart.listHouse.Count; i++) //з енама витягується, все норм тут, але в майбутньому оптимізувати
            {
                if (listShop[I] == returnAllStart.listHouse[i].dataHouse.NameThisHouse)
                {
                    _house = returnAllStart.listHouse[i];
                    _house.InitData(); // зразу створюю те що в наступному циклі змінюватиму
                    break;
                }
                if( i == returnAllStart.listHouse.Count - 1)
                {
                    Debug.Log("not exist " + listShop[I]);
                    return;
                }
            }

            returnAllStart.ReturnChangeTextOnButton(_house, allDataHouse);

            RectTransform button = Instantiate(buttonPrefab);
            Button _button = button.GetComponent<Button>();
            ButtonChange buttonChange = _button.GetComponent<ButtonChange>();


            shopInterface.InitialiseTexts(_button, _house, buttonChange);
            buttonChange.CheckUpdate(); // для interactable

            void OnButton()
            {
                terrain.TakeHouse(_house, buttonChange);
                scrollRect.gameObject.SetActive(false);
                cameraMove.possibleMove = true;
            }
            _button.onClick.AddListener(OnButton);
            Text text = button?.GetChild(0).GetComponent<Text>();
            text.text = listShop[I].ToString();

            button.gameObject.SetActive(true);
            button.SetParent(content);
            button.position = new Vector3(diffX + 50 + I * (button.rect.width + 30), diffY - button.rect.height / 2 - heihtPadding(button), 0);

        }

        float heihtPadding(RectTransform button)
        {
            float heightContent = content.rect.height;
            //Debug.Log(heightContent);
            if (heightContent - button.rect.height > 0)
            {
                float dif = (heightContent - button.rect.height) / 2 * 0.8f;
                return dif;
            }
            else
            {
                return 0;
            }
        }
    }
}
