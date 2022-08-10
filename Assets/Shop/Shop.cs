using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum NameHouse
{
    A,
    PVO,
    Meria,
    D,
    Wall,
}
public class Shop : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private Canvas canvasShop; // процент влияет на pos
    [SerializeField] private RectTransform canvas;
    [SerializeField] private CameraMove cameraMove;
    [SerializeField] private RectTransform panelPrefab;
    [SerializeField] private List<NameHouse> listShop;
    [SerializeField] private ReturnAllOnStart returnAllStart;
    [SerializeField] private MyTerrain terrain;
    private ShopButtonInterface shopInterface;
    [SerializeField] private Notification notification;
    [SerializeField] private CanvasHouse canvasHouse;
    private Vector3 localScaleCanvas;
    [SerializeField] private Money money;

    private bool IsCreate;

    public void OpenShop()
    {
        if (!IsCreate)
        {
            InitializeShop();
        }
        else
        {
            CheckAllUpdate();
        }
    }


    private void InitializeShop()
    {
        IsCreate = true;
        localScaleCanvas = canvas.localScale;
        float heightCanvas = canvas.rect.height * canvas.localScale.y;
        float widthCanvas = canvas.rect.width * canvas.localScale.x;
        float heightContent = heightCanvas * 0.9f;
        float widthContent = widthCanvas * 0.9f;
        float diffX = widthCanvas * 0.05f;
        float diffY = heightCanvas * 0.95f;
        float DiffContent = ((panelPrefab.rect.width * canvas.localScale.x / 5) * 2 + panelPrefab.rect.width * canvas.localScale.x + (listShop.Count - 1) * (panelPrefab.rect.width * canvas.localScale.x + (panelPrefab.rect.width * canvas.localScale.x / 8)))/ widthContent;
        content.anchorMax = new Vector2(DiffContent,1);

        for (int i = 0; i < listShop.Count; i++)
        {
            CreateButton(i);
        }



        void CreateButton(int I)
        {
            GeneralHouse _house = null;
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

            RectTransform panel = Instantiate(panelPrefab);
            Button _button = panel.GetChild(0).GetComponent<Button>();

            Image image = panel.GetChild(1).GetComponent<Image>();
            image.sprite= _house.dataTextOnHouse.info.spriteHouse; // картинка пряма, но потом под кутом скрин и обрезать все кроме здания и клетки

            ButtonChange buttonChange = _button.GetComponent<ButtonChange>();

            buttonChange.money = money;
            listButtonChange.Add(buttonChange);

            shopInterface = new ShopButtonInterface(panel.GetChild(2).GetComponent<Text>(), panel.GetChild(3).GetComponent<Text>(), panel.GetChild(0).GetChild(0).GetComponent<Text>());
            shopInterface.InitialiseTexts( _house, buttonChange);
            Transform imageMoney = _button.transform.GetChild(1 + (int)buttonChange.houseTextOnShop.typeMoney);
            imageMoney.gameObject.SetActive(true);



            buttonChange.CheckAllUpdate(); // для interactable

            void OnButton()
            {
                if(_button.image.color != ColorsStatic.colorDefoltInShop)
                {
                    _house.canvasHouse = canvasHouse; // в Take уже откриваю канвас, поетому перед ним присваиваю
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

            Text text = panel.GetChild(4).GetComponent<Text>(); // по хорошому внести в текст инит, но пускай тут
            text.text = listShop[I].ToString();

            panel.gameObject.SetActive(true);
            panel.SetParent(content);
            panel.position = new Vector3((panel.rect.width * canvas.localScale.x/5) + diffX + panel.rect.width / 2 * canvas.localScale.x + I*(panel.rect.width * canvas.localScale.x + (panel.rect.width * canvas.localScale.x / 8)),
                diffY - panel.rect.height / 2*canvas.localScale.y - heihtPadding(panel),
                0
            );
            panel.localScale = new Vector3(1, 1, 1);
        }

        float heihtPadding(RectTransform button)
        {
            //Debug.Log(heightContent);
            if (heightContent - button.rect.height * canvas.localScale.y / 2 > 0)
            {
                float dif = (heightContent - button.rect.height * canvas.localScale.y) / 2 * 1.25f;
                return dif; 
            }
            else
            {
                return 0;
            }
        }
    }
    private List<ButtonChange> listButtonChange = new List<ButtonChange>();
    private void CheckAllUpdate()
    {
        for(int i=0; i < listButtonChange.Count; i++)
        {
            listButtonChange[i].CheckAllUpdate();
        }
    }
}
