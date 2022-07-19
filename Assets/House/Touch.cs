using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Touch : MonoBehaviour, IEndDragHandler,IDragHandler
{
    public bool Drag { get { return drag; } }

    [SerializeField] private bool drag; //сховать
    [HideInInspector] public bool startMove;
    [HideInInspector] public bool endMove;
    [HideInInspector] public StateHouse stateHouse;
    public void OnDrag(PointerEventData eventData)
    {
        drag = true;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        stateHouse = StateHouse.IsActive;
        drag =  false;
    }
}
