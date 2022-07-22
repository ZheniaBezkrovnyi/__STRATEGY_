using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTouch0 : MonoBehaviour
{
    House house;
    private bool drag = false;
    private float timeDrag;
    private bool ChangeDragNow
    {
        get
        {
            if (house != null) {
                if(house.Drag != drag){
                    drag = house.Drag;
                    return true;
                }
            }
            return false;
        }
    }
    void Update()
    {
        house = TakeObjects._house;
        if (house == null) { return; }
        
        if (Input.touchCount == 1)
        {
            if (true)
            {

            }
            timeDrag += Input.GetTouch(0).deltaTime;
            if (timeDrag > 0.1f)
            {
                ActionIfDrag();
            }
            if(house.currentColor == StateColor.Blue || house.currentColor == StateColor.Red)
            {
                //ActionIfOneTap();
            }
        }

        void ActionIfDrag()
        {
            
        }
        void ActionIfOneTap()
        {
            if (house.stateHouse != StateHouse.Neytral) {
                Debug.Log("oneTap");
                if (house.currentColor == StateColor.Blue)
                {
                    house.stateHouse = StateHouse.NotActive;
                    house.currentColor = StateColor.Normal;
                    house.CloseCanvasHouse(house.canvasWindows);
                    house.endMove = true;
                    TakeObjects._house = null;
                }
                else if (house.currentColor == StateColor.Red)
                {
                    house.stateHouse = StateHouse.NotActive;
                    house.currentColor = StateColor.Normal;
                    house.CloseCanvasHouse(house.canvasWindows);
                    house.ReturnCell();
                    house.endMove = true;
                    TakeObjects._house = null;
                }
            }
        }
    }
}
