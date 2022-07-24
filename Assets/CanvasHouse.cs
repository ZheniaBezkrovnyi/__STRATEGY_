using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasHouse : MonoBehaviour
{
    [SerializeField] private Canvas canvasHouse;
    [SerializeField] private PanelCanvasHouse panelCanvasHouse;
    public void OpenCanvasHouse(House _house)
    {
        panelCanvasHouse.house = _house;
        canvasHouse.gameObject.SetActive(true);
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
    public void CloseCanvasHouse()
    {
        panelCanvasHouse.house = null;
        canvasHouse.gameObject.SetActive(false);
    }
}
