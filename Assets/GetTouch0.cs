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
        if (TakeObjects._house == null) { return; }
        house = TakeObjects._house;
        Debug.Log(timeDrag);
        if (Input.touchCount > 0)
        {


            if(Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                if(timeDrag <= 0.3f)
                {
                    ActionIfOneTap();
                }
            }
            timeDrag += Input.GetTouch(0).deltaTime;
        }
        else
        {
            timeDrag = 0;
        }









        void ActionIfDrag()
        {
            
        }
        void ActionIfOneTap()
        {
            if (!house.onDown) {
                if (house.currentColor == StateColor.Blue)
                {
                    house.endMove = true;
                    TakeObjects._house = null;
                }
                else if (house.currentColor == StateColor.Red)
                {
                    house.ReturnCell();
                    house.endMove = true;
                    TakeObjects._house = null;
                }
            }
            else
            {
                house.onDown = false;
            }
        }
    }
}
