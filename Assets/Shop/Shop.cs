using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
    [SerializeField] private List<House> listHouse;
    [SerializeField] private MyTerrain terrain;
    private ShopButtonInterface shopInterface;
    private void Awake()
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
        for (int i = 0; i < listShop.Count; i++)
        {
            RectTransform button = Instantiate(buttonPrefab);
            Button _button = button.GetComponent<Button>();

            House _house = listHouse[i];
            shopInterface.InitialiseTexts(_button, _house);

            void OnButton()
            {
                terrain.TakeHouse(_house);
                scrollRect.gameObject.SetActive(false);
                cameraMove.possibleMove = true;
            }
            _button.onClick.AddListener(OnButton);
            Text text = button?.GetChild(0).GetComponent<Text>();
            text.text = listShop[i].ToString();

            button.gameObject.SetActive(true);
            button.SetParent(content);
            button.position = new Vector3(diffX + 50 + i * (button.rect.width + 30), diffY - button.rect.height/2 - heihtPadding(),0);


            float heihtPadding()
            {
                float heightContent = content.rect.height;
                //Debug.Log(heightContent);
                if (heightContent - button.rect.height > 0)
                {
                    float dif = (heightContent - button.rect.height)/2 * 0.8f;
                    return dif;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
