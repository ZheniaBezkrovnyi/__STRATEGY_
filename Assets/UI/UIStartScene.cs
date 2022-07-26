using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStartScene : MonoBehaviour
{
    [SerializeField] private Button buttonShop, buttonBackShop;
    [SerializeField] private Canvas shopCanvas;
    [SerializeField] private Canvas panelCanvas;
    [SerializeField] private Button buttonBackPanel;
    [SerializeField] private CanvasHouse canvasHouse;
    [SerializeField] private GetTouch0 getTouch0;
    [SerializeField] private Button buttonCanvasStartYes, buttonCanvasStartNo;
    [SerializeField] private AnimTimeBuild animTimeBuild;
    private void Awake()
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
            Debug.Log(button.name);
            if (canvas == panelCanvas)
            {
                if (_bool)
                {
                    getTouch0.STOP = _bool;
                    GetDtaForPanel(button);
                }
                else
                {
                    StartCoroutine(StopTouch()); // не могу красиво, потому что гонка времени не дает
                    if (button == canvasHouse.buttonImprovePrice) // проблеми із ссилкою у самій панелі, тому буде тут
                    {
                        StartCoroutine(animTimeBuild.BeginBuildHouse(TakeObjects._house, true));
                    }
                }
            }
            canvas.gameObject.SetActive(_bool);
            CameraMove.possibleMove = !_bool;
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
                House thidHouse = TakeObjects._house;
                SaveInJSON saveInJSON = new SaveInJSON();
                saveInJSON.AddThisHouseInList(thidHouse);
                thidHouse.canvasHouse.CloseCanvasHouseOnlyStart();
                TakeObjects.End(
                    Posit.DesWithPosit(thidHouse.transform.position.x, thidHouse.transform.position.z, thidHouse).x,
                    Posit.DesWithPosit(thidHouse.transform.position.x, thidHouse.transform.position.z, thidHouse).y,
                    thidHouse,
                    true
                );
                StartCoroutine(animTimeBuild.BeginBuildHouse(thidHouse,true));
                thidHouse.existOrNot = ExistOrNot.Almost;
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
