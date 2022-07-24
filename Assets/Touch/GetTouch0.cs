﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExistOrNot
{
    Not,
    Almost,
    Yes
}
public class GetTouch0 : MonoBehaviour
{
    [SerializeField] private UIStartScene uiStartScene;
    [SerializeField] private Canvas panel;
    House house;
    private bool drag = false;
    private float timeDrag;
    public bool STOP;
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
        if (house.existOrNot != ExistOrNot.Yes) // щоб не відпрацьовував тач при взятті хауса, далі можна з exist зробити enum, якшо треба буде відслідковувати перше взяття з переносами
        {
            house.existOrNot = ExistOrNot.Almost;
            return;
        }/*else if (house.existOrNot == ExistOrNot.Yes)
        {
            SaveInJSON saveInJSON = new SaveInJSON();
            saveInJSON.SaveThisHouseInList(house);
        }*/
        if (Input.touchCount > 0)
        {


            if(Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                if(timeDrag <= 0.3f  && !STOP && !uiStartScene.boolPosButton(Input.GetTouch(0).position))
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
                //Debug.Log("актіон");
                TakeObjects.End(
                    Posit.DesWithPosit(house.transform.position.x, house.transform.position.z,house).x, 
                    Posit.DesWithPosit(house.transform.position.x, house.transform.position.z, house).y,
                    house,
                    true
                );
            }
            else
            {
                house.onDown = false;
            }
        }
    }
}
