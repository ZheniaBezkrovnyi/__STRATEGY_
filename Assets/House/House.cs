using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public enum StateHouse
{
    IsActive,
    NotActive,
    Neytral
}
public enum StateColor
{
    Norm,
    Red
}
public class House : Touch, IPointerClickHandler, IPointerDownHandler
{
    public NameHouse NameThisHouse { get { return nameThisHouse; } }
    [SerializeField] private NameHouse nameThisHouse;
    [SerializeField] private Vector2Int sides;
    public Vector2Int Sides { get { return sides; } }
    public int[,,] posOnMap;//save

    private ColorsObjects colorsObjects;
    [SerializeField] private Color clickColor;
    [SerializeField] private Color redColor;
    [HideInInspector] public StateColor currentColor;
    private StateColor _currentColor;

    [HideInInspector]  public bool existOrNot;

    private int neParniX;
    public int NeParniX { get { return neParniX; } }
    private int neParniZ;
    public int NeParniZ { get { return neParniZ; } }
    [SerializeField] private int MaxCountBuild;
    [SerializeField] private Vector4 timeBuild;

    public DataHouseTextOnButton dataTextOnButton;
    private bool ChangeColor
    {
        get
        {
            if(_currentColor != currentColor)
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
        InitData();
        neParniX = sides.x % 2 == 1 ? 1 : 0;
        neParniZ = sides.y % 2 == 1 ? 1 : 0;
        colorsObjects = new ColorsObjects();
        _currentColor = currentColor = StateColor.Norm;
        ReturnOrInitColor();
    }
    private void Update()
    {
        if (ChangeColor)
        {
            _currentColor = currentColor;
            InitColor(_currentColor);
        }
        if(stateHouse == StateHouse.IsActive && Input.touchCount != 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (currentColor == StateColor.Norm)
            {
                if (!existOrNot)
                {
                    Debug.Log("Add");
                    dataTextOnButton.buttonChange.AddCurrentBuildThisHouse(NameThisHouse);
                }
                endMove = true; //тепер перейде виконувати End
                stateHouse = StateHouse.NotActive;
                TakeObjects._house = null;
                ReturnOrInitColor();
            }
            else if (currentColor == StateColor.Red)
            {
                if (existOrNot)
                {
                    ReturnOrInitColor();
                    ReturnCell();
                    stateHouse = StateHouse.NotActive;
                }
                else
                {
                    Destroy(gameObject);
                }
                TakeObjects._house = null;
            }
            Debug.Log(dataTextOnButton.buttonChange);
        }
    }
    public void InitData()
    {
        dataTextOnButton = new DataHouseTextOnButton(
            MaxCountBuild,
            new TimeBuild((int)timeBuild.x, (int)timeBuild.y, (int)timeBuild.z, (int)timeBuild.w)
        );
    }
    private void ReturnCell() //позиція у масиві точки це її початок зліва
    {
        float _x_ = posOnMap[0, 0, 0] + MyTerrain.xMin + sides.x / 2 + (float)(neParniX) / 2f;
        float _z_ = posOnMap[1, 0, 0] + MyTerrain.zMin + sides.y / 2 + (float)(neParniZ) / 2f;
        transform.position = new Vector3(_x_* MyTerrain.sizeOneCell, transform.position.y,_z_* MyTerrain.sizeOneCell);


        Debug.Log(posOnMap[0, 0, 0] + (int)(sides.x / 2) + " : " + posOnMap[1, 0, 0] + (int)sides.y / 2);
        TakeObjects.End(posOnMap[0, 0, 0] + (int)(sides.x / 2), posOnMap[1, 0, 0] + (int)(sides.y / 2), this);
    }
    #region Colors
    private void ReturnOrInitColor()
    {

        colorsObjects.ReturnOrInitColor(this.transform);
    }
    public void InitColor(StateColor stateColor)
    {
        if (stateColor == StateColor.Norm)
        {
            colorsObjects.InitColor(clickColor);
        }else if(stateColor == StateColor.Red)
        {
            colorsObjects.InitColor(redColor);
        }
    }
    #endregion
    #region Pointers
    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("Click");
        TakeObjects._house = this; //через те що дом буде активним при тому коли його беруть
        if (stateHouse == StateHouse.NotActive)
        {
            startMove = true;
            stateHouse = StateHouse.IsActive;
            colorsObjects.InitColor(clickColor);
        }
        if (stateHouse == StateHouse.Neytral)
        {
            stateHouse = StateHouse.IsActive;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("Down");
        if(stateHouse == StateHouse.IsActive)
        {
            stateHouse = StateHouse.Neytral;
        }
    }
    #endregion
}

public class DataHouseTextOnButton //дание для кнопок в магазине
{
    public int currentBuildThisHouse = 0;
    public int MaxCountBuild;
    public TimeBuild TimeBuild;
    public ButtonChange buttonChange;
    public DataHouseTextOnButton(int maxCountBuild, TimeBuild timeBuild)
    {
        MaxCountBuild = maxCountBuild;
        TimeBuild = timeBuild;
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
    public void InitColor(Color _clickColor)
    {
        InitializeOrReturnColor(TakeObjects._house.transform, _clickColor);
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
                for (int i = 0; i < _trnsf.childCount; i++)
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
                    countChild++;
                    Init(_trnsf.GetChild(i), _color_);
                }
            }
        }
    }
}

