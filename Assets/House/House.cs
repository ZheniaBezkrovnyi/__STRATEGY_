using System.Collections;
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
public class House : Touch, IPointerClickHandler, IPointerDownHandler
{
    public DataTextOnHouse dataTextOnHouse;
    public HouseTextOnShop houseTextOnShop;
    public DataHouse dataHouse;

    [HideInInspector] public Canvas canvasWindows;
    public int[,,] posOnMap;
    [SerializeField] private Vector2Int sides;
    public Vector2Int Sides { get { return sides; } }

    private ColorsObjects colorsObjects;
    [SerializeField] private Color clickColor;
    [SerializeField] private Color redColor;

    [HideInInspector] public bool existOrNot; // тільки шоб добавить в список на збереження раз
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
        __house = this;
        stateHouse = StateHouse.NotActive;
        currentColor = StateColor.Normal;
        neParniX = sides.x % 2 == 1 ? 1 : 0;
        neParniZ = sides.y % 2 == 1 ? 1 : 0;
        colorsObjects = new ColorsObjects();
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
    private void OpenCanvasHouse(Canvas canvasHouse)
    {
        canvasWindows.gameObject.SetActive(true);
        for(int i = 0; i < canvasWindows.gameObject.transform.childCount; i++)
        {
            if(canvasWindows.gameObject.transform.GetChild(i).name == "Improve")
            {
                WriteText(canvasWindows.gameObject.transform.GetChild(i),"Price", dataTextOnHouse.priceImprove.ToString());
            }
        }


        void WriteText(Transform childCanvas,string name,string data)
        {
            for (int j = 0; j < childCanvas.childCount; j++)
            {
                if (childCanvas.GetChild(j).name == name)
                {
                    Text text = childCanvas.GetChild(j).GetComponent<Text>();
                    text.text = data;
                    break;
                }
            }
        }
    }
    public void CloseCanvasHouse(Canvas canvasHouse)
    {
        canvasHouse.gameObject.SetActive(false);
    }
    public void ReturnCell() //позиція у масиві точки це її початок зліва
    {
        float _x_ = posOnMap[0, 0, 0] + MyTerrain.xMin + sides.x / 2 + (float)(neParniX) / 2f;
        float _z_ = posOnMap[1, 0, 0] + MyTerrain.zMin + sides.y / 2 + (float)(neParniZ) / 2f;
        transform.position = new Vector3(_x_* MyTerrain.sizeOneCell, transform.position.y,_z_* MyTerrain.sizeOneCell);


        TakeObjects.End(posOnMap[0, 0, 0] + (int)(sides.x / 2), posOnMap[1, 0, 0] + (int)(sides.y / 2), this);
    }
    #region Colors
    private void InitColor(StateColor stateColor)
    {
        if (stateColor == StateColor.Green)
        {
            colorsObjects.InitColor(clickColor);
        }else if(stateColor == StateColor.Red)
        {
            colorsObjects.InitColor(redColor);
        }
        else if (stateColor == StateColor.Blue)
        {
            colorsObjects.InitColor(new Color(0,0,1));
        }else if(stateColor == StateColor.Normal)
        {
            colorsObjects.ReturnOrInitColor(this.transform);
        }
    }
    #endregion
    #region Pointers
    public void OnPointerClick(PointerEventData eventData)
    {
        TakeObjects._house = this; //через те що дом буде активним при тому коли його беруть
        if (stateHouse == StateHouse.NotActive)
        {
            Debug.Log("TakeHouse");
            TakeObjects.ZeroCell(this);
            OpenCanvasHouse(canvasWindows);
            stateHouse = StateHouse.InBlue;
            currentColor = StateColor.Blue;
        }
        if (stateHouse == StateHouse.Neytral)
        {
            stateHouse = StateHouse.InBlue;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Down");
        if(stateHouse == StateHouse.InBlue)
        {
            stateHouse = StateHouse.Neytral;
        }
    }
    #endregion
    ~House()
    {
        Debug.Log("ending");
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

