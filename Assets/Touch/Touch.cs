using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Touch : MonoBehaviour,IDragHandler
{
    public bool Drag { get { return drag; } }

    [SerializeField] private bool drag; //сховать
    [HideInInspector] public bool startMove;
    //[HideInInspector] public bool endMove;
    [HideInInspector] public StateHouse stateHouse;
    public bool onDown;
    protected House __house;
    public void OnDrag(PointerEventData eventData) // відповідає за те коли переходить з синього на активний
    {
        drag = true;
        if (!zeroCell)
        {
            zeroCell = true;
            TakeObjects.ZeroCell(__house);
        }
    }
    private bool zeroCell;  // чтоб только раз виполнить
    protected void Upd()
    {
        if (drag)
        {
            if(Input.touchCount == 0) // з активного на синій
            {
                drag = false;
                onDown = false;

                if (__house.existOrNot == ExistOrNot.Not) return;

                zeroCell = false;
                //Debug.Log("0 тачей");
                TakeObjects.End(
                    Posit.DesWithPosit(__house.transform.position.x, __house.transform.position.z, __house).x,
                    Posit.DesWithPosit(__house.transform.position.x, __house.transform.position.z, __house).y,
                    __house, 
                    false
                );
            } 
        }
    }
}
