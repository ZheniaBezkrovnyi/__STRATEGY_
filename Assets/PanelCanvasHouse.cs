using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum InfoImprove
{
    Info,
    Improve
}
public class PanelCanvasHouse : MonoBehaviour // у Improve треба зробить те що плюсується, і картинки до полосок, але то другим
{
    [HideInInspector] public House house;
    [SerializeField] private RectTransform canvas;
    [SerializeField] private Image image;
    [SerializeField] private RectTransform panel, sliderButton, buttonPrice;
    [SerializeField] private Text message, timeImprove;
    private InitPanelHouse initPanelHouse;
    private List<GameObject> allObj = new List<GameObject>(); // чтоб удалять при переключении
    public void GiveBackData(InfoImprove infoImprove)
    {
        initPanelHouse = new InitPanelHouse(panel, image, sliderButton, buttonPrice, canvas, message, timeImprove,allObj); // не могу в Awake, он не успевает
        switch (infoImprove)
        {
            case InfoImprove.Info:
                initPanelHouse.InitPanel(house,InfoImprove.Info);
                break;
            case InfoImprove.Improve:
                initPanelHouse.InitPanel(house, InfoImprove.Improve);
                break;
        }
    }
}
public class InitPanelHouse : MonoBehaviour  // панель не буду стирать, потому что и так другой клик заменит содержимое
{
    private RectTransform panel,sliderButton, buttonPrice,canvas;
    private Image image;
    private Text message,timeImprove;
    private List<GameObject> allObj;
    public InitPanelHouse(RectTransform _panel, Image _image, RectTransform _sliderButton, RectTransform _buttonPrice, 
        RectTransform _canvas, Text _message, Text _timeImprove, List<GameObject> _allObj)
    {
        allObj = _allObj;
        if(allObj.Count != 0)
        {
            for(int i = allObj.Count - 1; i >= 0; i--)
            {
                Destroy(allObj[i]);
                allObj.RemoveAt(i);
            }
        }
        panel = _panel;
        image = _image;
        sliderButton = _sliderButton;
        buttonPrice = _buttonPrice;
        canvas = _canvas;
        message = _message;
        timeImprove = _timeImprove;
        CloseAllForStart();
        widthCanvas = canvas.rect.width * canvas.localScale.x;
        heightCanvas = canvas.rect.height * canvas.localScale.y;
        widthPanel = panel.rect.width * canvas.localScale.x;
        heightPanel = panel.rect.height * canvas.localScale.y;
    }
    public void InitPanel(House _house,InfoImprove infoImprove)
    {
        InitImage(_house);
        switch (infoImprove)
        {
            case InfoImprove.Info:
                switch (_house.dataTextOnHouse.typeHouse)
                {
                    case TypeHouse.JustHouse:
                        InitSliderButton(sliderButton, 0);
                        break;
                    case TypeHouse.Defence:
                        InitSliderButton(sliderButton, 0);
                        InitSliderButton(sliderButton, 1);
                        break;
                    case TypeHouse.Resources:
                        InitSliderButton(sliderButton, 0);
                        InitSliderButton(sliderButton, 1);
                        InitSliderButton(sliderButton, 2);
                        break;
                }
                break;
            case InfoImprove.Improve:
                InitPriceAndTimeButton(_house);
                switch (_house.dataTextOnHouse.typeHouse)
                {
                    case TypeHouse.JustHouse:
                        InitMessage("JustHouse");
                        break;
                    case TypeHouse.Defence:
                        InitMessage("Defence");
                        break;
                    case TypeHouse.Resources:
                        InitMessage("Resources");
                        break;
                }
                break;
        }



    }
    private float widthCanvas;
    private float heightCanvas;
    private float widthPanel;
    private float heightPanel;
    private Vector2 ZeroPositPanel()
    {
        float x = widthCanvas * panel.anchorMin.x;
        float y = heightCanvas * panel.anchorMax.y;
        return new Vector2(x,y);
    }

    private void InitSliderButton(RectTransform _sliderButton, int number)
    {
        RectTransform[] sliders = new RectTransform[2];
        for(int i = 0; i < sliders.Length; i++)
        {
            sliders[i] = Instantiate(_sliderButton);
            allObj.Add(sliders[i].gameObject);
            sliders[i].gameObject.SetActive(true);
            sliders[i].SetParent(panel);
            sliders[i].localScale = new Vector3(1,1,1);
            sliders[i].position = new Vector3(
                ZeroPositPanel().x + widthPanel - sliders[i].rect.width * canvas.localScale.x * 0.7f,
                ZeroPositPanel().y - (sliders[i].rect.height*2 + number * (sliders[i].rect.height*2)) * canvas.localScale.y, 
                0
            );
        }


    }
    private void InitPriceAndTimeButton(House _house)
    {
        buttonPrice.gameObject.SetActive(true); //баттон инит в UIStartScene
        Text text = buttonPrice.GetChild(0).GetComponent<Text>();
        text.text = _house.dataTextOnHouse.priceImprove.ToString();

        InitTimeImprove();
        void InitTimeImprove()
        {
            timeImprove.gameObject.SetActive(true);
            timeImprove.text = TimeBuild.ToString(_house.dataTextOnHouse.timeImprove);
        }
    }
    private void InitImage(House _house)
    {
        image.gameObject.SetActive(true);
        image.sprite = _house.dataTextOnHouse.info.spriteHouse;
    }
    private void InitMessage(string _message)
    {
        message.gameObject.SetActive(true);
        message.text = _message;
    }
    private void CloseAllForStart()
    {
        message.gameObject.SetActive(false);
        buttonPrice.gameObject.SetActive(false);
        timeImprove.gameObject.SetActive(false);
    }
}
