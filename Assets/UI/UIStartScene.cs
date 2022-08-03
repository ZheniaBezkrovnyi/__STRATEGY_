using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStartScene : MonoBehaviour
{
    [SerializeField] private Button buttonShop, buttonBackShop;
    [SerializeField] private Canvas shopCanvas;
    [SerializeField] private Shop shop;
    [SerializeField] private UIPanel uIPanel;
    [SerializeField] private CanvasHouseStart canvasHouseStart;
    private void Start()
    {
        ActiveOrNotShop(buttonShop,true, shopCanvas);
        ActiveOrNotShop(buttonBackShop, false, shopCanvas);
        ActiveOrNotShop(uIPanel.canvasHouse.buttonImprove, true, uIPanel.panelCanvas);
        ActiveOrNotShop(uIPanel.canvasHouse.buttonInfo, true, uIPanel.panelCanvas);
        ActiveOrNotShop(uIPanel.buttonBackPanel, false, uIPanel.panelCanvas);
        ActiveOrNotShop(uIPanel.canvasHouse.buttonImprovePrice, false, uIPanel.panelCanvas);
        ActiveOrNotShop(canvasHouseStart.buttonCanvasStartYes, false, canvasHouseStart.canvasHouseStart);
        ActiveOrNotShop(canvasHouseStart.buttonCanvasStartNo, false, canvasHouseStart.canvasHouseStart);
    }
    private void ActiveOrNotShop(Button button,bool _bool,Canvas canvas)
    {
        button.onClick.AddListener(() => {
            uIPanel.ActionsWithPanel(button);
            canvasHouseStart.RealizeCanvasHouseStart(button);

            canvas.gameObject.SetActive(_bool);
            CameraMove.possibleMove = !_bool;
            if (button == buttonShop)
            {
                shop.OpenShop();
            }
        });
    }
}
