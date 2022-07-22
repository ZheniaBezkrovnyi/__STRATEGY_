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
    public bool onDown;
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
                onDown = false;
                //TakeObjects.End(); //налаштувати тут це і буде зберігатись шоб не верталось до того як синім стало,взять формулу з TakeHouse
            } 
        }
    }
}
