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
        if (!house.existOrNot) // щоб не відпрацьовував тач при взятті хауса, далі можна з exist зробити enum, якшо треба буде відслідковувати перше взяття з переносами
        {
            SaveInJSON saveInJSON = new SaveInJSON();
            saveInJSON.SaveThisHouseInList(house);
            house.existOrNot = true;
            return;
        }
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
                Debug.Log("актіон");
                TakeObjects.End(
                    Posit.DesWithPosit(house.transform.position.x, house.transform.position.z,house).x, 
                    Posit.DesWithPosit(house.transform.position.x, house.transform.position.z, house).y,
                    house,
                    true
                );
                TakeObjects._house = null;
            }
            else
            {
                house.onDown = false;
            }
        }
    }
}
