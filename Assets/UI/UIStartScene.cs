using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStartScene : MonoBehaviour
{
    [SerializeField] private CameraMove cameraMove;
    [SerializeField] private Button buttonShop, buttonBackShop;
    [SerializeField] private RectTransform rectCanvas;
    [SerializeField] private Canvas shopCanvas;
    [SerializeField] private Canvas panelCanvas;
    [SerializeField] private Button buttonImprove, buttonInfo, buttonBackPanel;
    private void Awake()
    {
        widthCanvas = rectCanvas.rect.width * rectCanvas.localScale.x;
        heightCanvas = rectCanvas.rect.height * rectCanvas.localScale.y;
        ActiveOrNotShop(buttonShop,true, shopCanvas);
        ActiveOrNotShop(buttonBackShop, false, shopCanvas);
        ActiveOrNotShop(buttonImprove, true, panelCanvas);
        ActiveOrNotShop(buttonInfo, true, panelCanvas);
        ActiveOrNotShop(buttonBackPanel, false, panelCanvas);
    }
    private float widthCanvas;
    private float heightCanvas;
    private void ActiveOrNotShop(Button button,bool _bool,Canvas canvas)
    {
        button.onClick.AddListener(() => {
            canvas.gameObject.SetActive(_bool);
            cameraMove.possibleMove = !_bool;
        });
    }


    public bool posButton(Vector3 pos)
    {
        RectTransform rectButtonImprove = buttonImprove.GetComponent<RectTransform>();
        if (!ForAllNeedButton(rectButtonImprove))
        {
            return false;
        }
        RectTransform rectButtonInfo = buttonInfo.GetComponent<RectTransform>();
        if (!ForAllNeedButton(rectButtonInfo))
        {
            return false;
        }
        return true;



        bool ForAllNeedButton(RectTransform rect) {
            if (pos.x < widthCanvas * rect.anchorMax.x && pos.x > widthCanvas * rect.anchorMin.x &&
                pos.y < heightCanvas * rect.anchorMax.x && pos.y > heightCanvas * rect.anchorMin.x)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
