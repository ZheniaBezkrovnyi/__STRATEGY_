using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum InfoImprove
{
    Info,
    Improve
}
public class PanelCanvasHouse : MonoBehaviour
{
    public void GiveBackData(InfoImprove infoImprove)
    {
        switch (infoImprove)
        {
            case InfoImprove.Info: Debug.Log("Info");
                break;
            case InfoImprove.Improve:
                Debug.Log("Improve");
                break;
        }
    }
}
