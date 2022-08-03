using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasHouse : MonoBehaviour
{
    [SerializeField] private Canvas canvasHouse;
    [SerializeField] private Canvas canvasHouseOnlyStart;
    public Button buttonImprove, buttonInfo, buttonEnd, buttonImprovePrice,buttonBackPanel;
    [SerializeField] private PanelCanvasHouse panelCanvasHouse;
    [SerializeField] private RectTransform canvas;
    public void OpenCanvasHouse(GeneralHouse _house)
    {
        FalseAllActiveButton();

        panelCanvasHouse.house = _house;
        canvasHouse.gameObject.SetActive(true);
        BuildPosButton(_house);
        for (int i = 0; i < canvasHouse.gameObject.transform.childCount; i++)
        {
            if (canvasHouse.gameObject.transform.GetChild(i).name == "Improve")
            {
                WriteText(canvasHouse.gameObject.transform.GetChild(i), "Price", _house.dataTextOnHouse.priceImprove.ToString());
            }
        }


        void WriteText(Transform childCanvas, string name, string data)
        {
            for (int j = 0; j < childCanvas.childCount; j++)
            {
                if (childCanvas.GetChild(j).name == name)
                {
                    Text text = childCanvas.GetChild(j).GetComponent<Text>();
                    text.text = data;
                    break;
                }
            }
        }
    }

    private List<Button> activeButton = new List<Button>();
    private void FalseAllActiveButton()
    {
        for(int i=0; i < activeButton.Count; i++)
        {
            activeButton[i].gameObject.SetActive(false);
        }
        activeButton = new List<Button>();
    }
    private void BuildPosButton(GeneralHouse _house)
    {
        RectTransform rectButtonInfo = buttonInfo.GetComponent<RectTransform>();
        float widthOne = rectButtonInfo.rect.width * canvas.localScale.x;


        bool loading = TimeDateTime.InSeconds(_house.dataHouse.dataAnimBuildHouse.timeEndBuild) != 0;
        TypeHouse typeHouse = _house.dataTextOnHouse.typeHouse;
        if (!loading) // тут в свичах просто указать переменние и все
        {
            switch (typeHouse)
            {
                case TypeHouse.JustHouse: Build(InfoImprove.Info, InfoImprove.Improve);
                    break;
                case TypeHouse.Defence: Build(InfoImprove.Info, InfoImprove.Improve);
                    break;
                case TypeHouse.Resources: Build(InfoImprove.Info, InfoImprove.Improve);
                    break;
            }
        }
        else
        {
            switch (typeHouse)
            {
                case TypeHouse.JustHouse: Build(InfoImprove.Info, InfoImprove.End);
                    break;
                case TypeHouse.Defence: Build(InfoImprove.Info, InfoImprove.End);
                    break;
                case TypeHouse.Resources: Build(InfoImprove.Info, InfoImprove.End);
                    break;
            }
        }

        float heightCanvas;
        float widthCanvas;
        void Build(params InfoImprove[] infoImprove)
        {
            heightCanvas = canvas.rect.height * canvas.localScale.y;
            widthCanvas = canvas.rect.width * canvas.localScale.x;
            int count = infoImprove.Length;
            for(int i = 0; i < count; i++)
            {
                switch (infoImprove[i])
                {
                    case InfoImprove.Info: CreateRect(i,buttonInfo);
                        break;
                    case InfoImprove.Improve: CreateRect(i, buttonImprove);
                        break;
                    case InfoImprove.End: CreateRect(i, buttonEnd);
                        break;
                }
            }

            void CreateRect(int I, Button _button)
            {
                activeButton.Add(_button);
                RectTransform rectButton = _button.GetComponent<RectTransform>();
                float sumWidth = (widthOne + 10)*count - 10;
                float startWidthLeft = (widthCanvas - sumWidth) / 2;
                rectButton.gameObject.SetActive(true);
                rectButton.position = new Vector3(startWidthLeft + widthOne/2 + I * (widthOne + 10), heightCanvas * ((0.07f + 0.23f) / 2), 0);
                rectButton.localScale = new Vector3(1, 1, 1);
            }
        }
    }
    public void CloseCanvasHouse()
    {
        panelCanvasHouse.house = null;
        canvasHouse.gameObject.SetActive(false);
    }

    public void OpenCanvasHouseOnlyStart()
    {
        canvasHouseOnlyStart.gameObject.SetActive(true);
    }
    public void CloseCanvasHouseOnlyStart()
    {
        canvasHouseOnlyStart.gameObject.SetActive(false);
    }
}
