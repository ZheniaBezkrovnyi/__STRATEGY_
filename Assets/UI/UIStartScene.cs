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
    [SerializeField] private GetTouch0 getTouch0;
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
            if (canvas == panelCanvas)
            {
                if (_bool)
                {
                    getTouch0.STOP = _bool;
                }
                else
                {
                    StartCoroutine(StopTouch()); // не могу красиво, потому что гонка времени не дает
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


    public bool boolPosButton(Vector2 pos)
    {
        RectTransform[] rectButtons = new RectTransform[2]
        {
            buttonImprove.GetComponent<RectTransform>(),
            buttonInfo.GetComponent<RectTransform>()
        };
        if (!ForAllNeedButton(rectButtons))
        {
            return false;
        }
        return true;



        bool ForAllNeedButton(RectTransform[] rect) {
            float MinWidth = 0;
            float MaxWidth = 0;
            float MinHeight = 0;
            float MaxHeight = 0;
            for (int i = 0; i < rect.Length; i++)
            {
                if(rect[i].anchorMin.x < MinWidth)
                {
                    MinWidth = rect[i].anchorMin.x;
                }
                if (rect[i].anchorMax.x > MaxWidth)
                {
                    MaxWidth = rect[i].anchorMax.x;
                }
                if (rect[i].anchorMin.y < MinHeight)
                {
                    MinHeight = rect[i].anchorMin.y;
                }
                if (rect[i].anchorMax.y > MaxHeight)
                {
                    MaxHeight = rect[i].anchorMax.y;
                }
            }

            if (pos.x < widthCanvas * MaxWidth && pos.x > widthCanvas * MinWidth &&
                pos.y < heightCanvas * MaxHeight && pos.y > heightCanvas * MinHeight)
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
