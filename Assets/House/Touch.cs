using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Touch : MonoBehaviour,IDragHandler
{
    public bool Drag { get { return drag; } }

    [SerializeField] private bool drag; //сховать
    [HideInInspector] public bool startMove;
    [HideInInspector] public bool endMove;
    [HideInInspector] public StateHouse stateHouse;
    protected House __house;
    public void OnDrag(PointerEventData eventData)
    {
        drag = true;
    }
    protected void Upd()
    {
        if (drag)
        {
            if(Input.touchCount == 0)
            {
                drag = false;
                //Debug.Log("qwerty");
                if (__house.currentColor == StateColor.Green)
                {
                    Put();
                }
                else if (__house.currentColor == StateColor.Red)
                {
                    Put();
                    __house.ReturnCell();
                }
                void Put()
                {
                    stateHouse = StateHouse.InBlue;
                    __house.currentColor = StateColor.Blue;
                }
            } 
        }
    }
}
