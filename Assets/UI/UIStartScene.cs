using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStartScene : MonoBehaviour
{
    [SerializeField] private Button buttonShop, buttonBackShop;
    [SerializeField] private Canvas shopCanvas;
    [SerializeField] private Shop shop;
    [SerializeField] private Canvas panelCanvas;        //панель з усім своїм повність у свій клас який створю
    [SerializeField] private Button buttonBackPanel;
    [SerializeField] private CanvasHouse canvasHouse;
    [SerializeField] private GetTouch0 getTouch0;
    [SerializeField] private Button buttonCanvasStartYes, buttonCanvasStartNo;
    [SerializeField] private AnimTimeBuild animTimeBuild;
    [SerializeField] private Money money;
    private void Start()
    {
        ActiveOrNotShop(buttonShop,true, shopCanvas);
        ActiveOrNotShop(buttonBackShop, false, shopCanvas);
        ActiveOrNotShop(canvasHouse.buttonImprove, true, panelCanvas);
        ActiveOrNotShop(canvasHouse.buttonInfo, true, panelCanvas);
        ActiveOrNotShop(buttonBackPanel, false, panelCanvas);
        ActiveOrNotShop(canvasHouse.buttonImprovePrice, false, panelCanvas);
        RealizeCanvasHouseStart(buttonCanvasStartYes);
        RealizeCanvasHouseStart(buttonCanvasStartNo);
    }
    private float widthCanvas;
    private float heightCanvas;
    private void ActiveOrNotShop(Button button,bool _bool,Canvas canvas)
    {
        button.onClick.AddListener(() => {
            if (canvas == panelCanvas) //дуже хуйово виглядає, колись перепишу, тут не має бути стільки іфів, треба окремо робити для цієї панелі
            {
                if (_bool)
                {
                    getTouch0.STOP = _bool;
                    if (button != canvasHouse.buttonImprovePrice)
                    {
                        GetDtaForPanel(button);
                    }
                }
                else
                {
                    if (button == canvasHouse.buttonImprovePrice) // проблеми із ссилкою у самій панелі, тому буде тут
                    {
                        if (button.image.color != ColorsStatic.colorDefoltInShop)
                        {
                            money.ChangeMoney(-TakeObjects._house.dataTextOnHouse.priceImprove, TakeObjects._house.houseTextOnShop.typeMoney);
                            if (TakeObjects._house.GetComponent<MainHouse>())
                            {
                                StartCoroutine(animTimeBuild.BeginBuildHouse((MainHouse)TakeObjects._house, true));
                            }
                            StartCoroutine(StopTouch());
                        }
                        else
                        {
                            Debug.Log("не хватает денег для улучшения");  //  потом notification
                            return;
                        }
                    }
                    else
                    {
                        StartCoroutine(StopTouch()); // не могу красиво, потому что гонка времени не дает
                    }
                }
            }
            canvas.gameObject.SetActive(_bool);
            CameraMove.possibleMove = !_bool;
            if(button == buttonShop)
            {
                shop.OpenShop();
            }
        });
        IEnumerator StopTouch()
        {
            yield return new WaitForSeconds(1f / 20);
            getTouch0.STOP = false;
        }
    }

    private void GetDtaForPanel(Button _button)
    {
        switch (_button.name)
        {
            case "Info": panelCanvas.GetComponent<PanelCanvasHouse>().GiveBackData(InfoImprove.Info);
                break;
            case "Improve":
                panelCanvas.GetComponent<PanelCanvasHouse>().GiveBackData(InfoImprove.Improve);
                break;
            case "End":
                panelCanvas.GetComponent<PanelCanvasHouse>().GiveBackData(InfoImprove.End);
                break;
        }
    }



    private void RealizeCanvasHouseStart(Button _button) //оптимізувати
    {
        _button.onClick.AddListener(() => {
            if(_button.name == "Yes")   // закинуть сюда же загрузку временем сразу
            {
                GeneralHouse thisHouse = TakeObjects._house;
                SaveInJSON saveInJSON = new SaveInJSON();
                saveInJSON.AddThisHouseInList(thisHouse);
                thisHouse.canvasHouse.CloseCanvasHouseOnlyStart();
                TakeObjects.End(
                    Posit.DesWithPosit(thisHouse.transform.position.x, thisHouse.transform.position.z, thisHouse).x,
                    Posit.DesWithPosit(thisHouse.transform.position.x, thisHouse.transform.position.z, thisHouse).y,
                    thisHouse,
                    true
                );
                if (thisHouse.GetComponent<MainHouse>())
                {
                    StartCoroutine(animTimeBuild.BeginBuildHouse((MainHouse)thisHouse, true));
                    thisHouse.existOrNot = ExistOrNot.Almost;
                }
                else
                {
                    thisHouse.existOrNot = ExistOrNot.Yes;
                }
                money.ChangeMoney(-thisHouse.houseTextOnShop.priceForBuild, thisHouse.houseTextOnShop.typeMoney);
            }
            else if(_button.name == "No")
            {
                TakeObjects._house.canvasHouse.CloseCanvasHouseOnlyStart();
                Destroy(TakeObjects._house.gameObject);
                TakeObjects._house = null;
            }
            else
            {
                Debug.LogError("щось з назвами у канвас хаус старт");
            }
        });
    }
}
