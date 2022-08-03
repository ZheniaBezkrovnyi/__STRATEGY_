﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public enum StateHouse
{
    IsActive,
    NotActive,
    Neytral,
    InBlue
}
public enum StateColor
{
    Default,
    Normal,
    Green,
    Red,
    Blue
}
public class GeneralHouse : Touch, IPointerClickHandler, IPointerDownHandler
{
    public DataTextOnHouse dataTextOnHouse;
    public HouseTextOnShop houseTextOnShop;
    public DataHouse dataHouse;

    [HideInInspector] public CanvasHouse canvasHouse;
    [SerializeField] private Vector2Int sides;
    public Vector2Int Sides { get { return sides; } }

    private ColorsObjects colorsObjects;
    [SerializeField] private Color clickColor;
    [SerializeField] private Color redColor;

    [HideInInspector] public ExistOrNot existOrNot;
    private int neParniX;
    public int NeParniX { get { return neParniX; } }
    private int neParniZ;
    public int NeParniZ { get { return neParniZ; } }

    [HideInInspector] public StateColor currentColor;
    private StateColor _currentColor = StateColor.Default;
    private bool ChangeColor
    {
        get
        {
            if (_currentColor != currentColor)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    private void OnEnable()
    {
        colorsObjects = new ColorsObjects();
        __house = this;
        InitColor(StateColor.Normal);
        stateHouse = StateHouse.NotActive;
        neParniX = sides.x % 2 == 1 ? 1 : 0;
        neParniZ = sides.y % 2 == 1 ? 1 : 0;
    }
    private void Update()
    {
        if (ChangeColor)
        {
            _currentColor = currentColor;
            InitColor(_currentColor);
        }
        Upd();
    }
    public void ReturnCell(GeneralHouse _house) //позиція у масиві точки це її початок зліва
    {
        transform.position = new Vector3(
            Posit.InitInPosit(_house.dataHouse.posit.x, _house.dataHouse.posit.z, _house).x,
            _house.transform.position.y,
            Posit.InitInPosit(_house.dataHouse.posit.x, _house.dataHouse.posit.z, _house).y
        );
    }
    #region Colors
    private void InitColor(StateColor stateColor)
    {
        if (stateColor == StateColor.Green)
        {
            colorsObjects.InitColor(clickColor, this);
        }
        else if (stateColor == StateColor.Red)
        {
            colorsObjects.InitColor(redColor, this);
        }
        else if (stateColor == StateColor.Blue)
        {
            colorsObjects.InitColor(new Color(0, 0, 1), this);
        }
        else if (stateColor == StateColor.Normal)
        {
            colorsObjects.ReturnOrInitColor(this.transform);
        }
    }
    #endregion
    #region Pointers
    public void OnPointerDown(PointerEventData eventData)
    {
        onDown = true;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.delta.magnitude == 0 && !Drag)
        {
            if (TakeObjects._house != null)
            {
                if (TakeObjects._house.existOrNot == ExistOrNot.Not) // чтоб не переключался когда еще здание не поставил, из Attack по ситуации поменяю
                {
                    return;
                }
                TakeObjects.End(TakeObjects._house.dataHouse.posit.x, TakeObjects._house.dataHouse.posit.z, TakeObjects._house, true);
            }
            if (stateHouse == StateHouse.NotActive)
            {
                IfClick(this, canvasHouse);
            }
        }
    }
    public static void IfClick(GeneralHouse _house, CanvasHouse _canvasHouse)
    {
        TakeObjects._house = _house;
        _canvasHouse.OpenCanvasHouse(_house);
        _house.stateHouse = StateHouse.InBlue;
        _house.currentColor = StateColor.Blue;
    }
    #endregion
    ~GeneralHouse()
    {
        //Debug.Log("ending");
    }
}



public class ColorsObjects
{
    private List<Color> listStartColor;
    public ColorsObjects()
    {
        listStartColor = new List<Color>();
    }

    public void ReturnOrInitColor(Transform trnsf)
    {
        InitializeOrReturnColor(trnsf, Color.white);
    }
    public void InitColor(Color _clickColor, GeneralHouse house)
    {
        InitializeOrReturnColor(house.transform, _clickColor);
    }
    private void InitializeOrReturnColor(Transform trnsf, Color _color)
    {
        bool emptyList = false;
        int countChild = 0;
        if (listStartColor.Count == 0)
        {
            emptyList = true;
        }
        Init(trnsf, _color);
        void Init(Transform _trnsf, Color _color_)
        {
            if (_trnsf.childCount != 0)
            {
                #region Comment
                /*for (int i = 0; i < _trnsf.childCount; i++)
                {
                    if (emptyList)
                    {
                        //Debug.Log("AddList");
                        listStartColor.Add(_trnsf.GetChild(i).GetComponent<Renderer>().material.color);
                    }
                    else
                    {
                        if (_color_ == Color.white)
                        {
                            if (listStartColor[countChild] != null)
                            {
                                _trnsf.GetChild(i).GetComponent<Renderer>().material.color = listStartColor[countChild];
                            }
                        }
                        else
                        {
                            _trnsf.GetChild(i).GetComponent<Renderer>().material.color = _color_;
                        }
                    }
                    countChild++;
                    Init(_trnsf.GetChild(i), _color_);
                }*/
                #endregion
                for (int i = 0; i < 1; i++)
                {
                    if (emptyList)
                    {
                        listStartColor.Add(_trnsf.GetChild(i).GetComponent<Renderer>().material.color);
                    }
                    else
                    {
                        if (_color_ == Color.white)
                        {
                            if (listStartColor[countChild] != null)
                            {
                                _trnsf.GetChild(i).GetComponent<Renderer>().material.color = listStartColor[countChild];
                            }
                        }
                        else
                        {
                            _trnsf.GetChild(i).GetComponent<Renderer>().material.color = _color_;
                        }
                    }
                }
            }
        }
    }
}