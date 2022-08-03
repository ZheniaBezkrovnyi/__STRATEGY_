using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    public Canvas panelCanvas;
    public Button buttonBackPanel;
    [SerializeField] private GetTouch0 getTouch0;
    [SerializeField] private AnimTimeBuild animTimeBuild;
    [SerializeField] private CanvasHouse canvasHouse;
    [SerializeField] private Money money;
    public void ActionsWithPanel(Button button)
    {
        button.onClick.AddListener(() =>
        {
            if (button == canvasHouse.buttonInfo || button == canvasHouse.buttonImprove)
            {
                getTouch0.STOP = true;
                GetDtaForPanel(button);
            }

            if (button == canvasHouse.buttonImprovePrice)
            {
                if (button.image.color != ColorsStatic.colorDefoltInShop)
                {
                    money.ChangeMoney(-TakeObjects._house.dataTextOnHouse.priceImprove, TakeObjects._house.houseTextOnShop.typeMoney);
                    StartCoroutine(animTimeBuild.BeginBuildHouse((MainHouse)TakeObjects._house, true));
                    StartCoroutine(StopTouch());
                }
                else
                {
                    Debug.Log("не хватает денег для улучшения");  //  потом notification
                    return;
                }
            }

            if (button == canvasHouse.buttonBackPanel)
            {
                StartCoroutine(StopTouch());
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
            case "Info":
                panelCanvas.GetComponent<PanelCanvasHouse>().GiveBackData(InfoImprove.Info);
                break;
            case "Improve":
                panelCanvas.GetComponent<PanelCanvasHouse>().GiveBackData(InfoImprove.Improve);
                break;
            case "End":
                panelCanvas.GetComponent<PanelCanvasHouse>().GiveBackData(InfoImprove.End);
                break;
        }
    }
}
