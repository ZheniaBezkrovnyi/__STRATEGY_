using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTerrain : MonoBehaviour
{
    [SerializeField] private JSON json;
    private int[,] setMap; // не сохранять, но при восстановлении зданий с ними вместе end визивать
    [SerializeField] private List<Vector2Int> pointsMap;
    public static int xMin, xMax,zMin,zMax;
    private TakeObjects takeObjects;
    [SerializeField] private GameObject zatychki;
    [SerializeField] private Field field;
    public static int sizeOneCell;
    [SerializeField] private int SizeOneCell;
    [SerializeField] private Canvas canvasWindowsForHouse;
    //public List<string> list;
    private void Awake()
    {
        sizeOneCell = SizeOneCell;
        if(pointsMap.Count >= 2)
        {
            xMin = Mathf.Min(pointsMap[0].x, pointsMap[1].x);
            xMax = Mathf.Max(pointsMap[0].x, pointsMap[1].x);
            zMin = Mathf.Min(pointsMap[0].y, pointsMap[1].y);
            zMax = Mathf.Max(pointsMap[0].y, pointsMap[1].y);
            Instantiate(zatychki,new Vector3(xMin * sizeOneCell, zatychki.transform.localScale.y/2, zMin * sizeOneCell),Quaternion.identity);
            Instantiate(zatychki, new Vector3(xMax * sizeOneCell, zatychki.transform.localScale.y / 2, zMin * sizeOneCell), Quaternion.identity);
            Instantiate(zatychki, new Vector3(xMin * sizeOneCell, zatychki.transform.localScale.y / 2, zMax * sizeOneCell), Quaternion.identity);
            Instantiate(zatychki, new Vector3(xMax * sizeOneCell, zatychki.transform.localScale.y / 2, zMax * sizeOneCell), Quaternion.identity);
        }
        else { 
            Debug.LogError("не заполнени точки карти"); 
        }
        setMap = new int[xMax - xMin, zMax - zMin];
        takeObjects = new TakeObjects(setMap);
        field.CreateField();
    }
    void Update()
    {
        takeObjects.MoveHouse(xMin,zMin);

        /*CheckCell();
        void CheckCell()
        {
            list = new List<string>();
            for (int x = 0; x < 4; x++)
            {
                for (int z = 0; z < 4; z++)
                {
                    list.Add(x + " / " + z + " / " + setMap[x, z]);
                }
            }
        }*/
    }
    public void TakeHouse(House house, ButtonChange buttonChange) // вісить на AddListener при створенні кнопок у Shop
    {
        takeObjects.TakeHouse(house,buttonChange,canvasWindowsForHouse);
    }
}

public class TakeObjects : MonoBehaviour
{
    public static House _house;
    private static int[,] _setMap;
    private static House myHouse;
    private bool canPut = true;
    private int SizeOneCell;
    private SaveInJSON saveInJSON;
    public TakeObjects(int[,] _setMap_)
    {
        saveInJSON = new SaveInJSON();
        _setMap = _setMap_;
        SizeOneCell = MyTerrain.sizeOneCell;
    }
    public void TakeHouse(House house,ButtonChange buttonChange,Canvas canvasOnWindows) // при взятии переносить камеру на видимую зону обьекта чрез интерполяцию
    { 
        if (_house != null) Destroy(_house.gameObject);
        _house = Instantiate(house);
        _house.canvasWindows = canvasOnWindows;
        myHouse = _house;
        x = 0;
        z = 0;
        CheckPosTakeHouse(ref x, ref z);
        Debug.Log(x + " "+ z + "startPos");
        myHouse.dataHouse.posit = new Posit(x,z);
        myHouse.transform.position += new Vector3(x * SizeOneCell + MyTerrain.xMin * SizeOneCell + (float)myHouse.NeParniX / 2f* SizeOneCell, myHouse.transform.localScale.y / 2, z * SizeOneCell + MyTerrain.zMin * SizeOneCell + (float)myHouse.NeParniZ / 2f * SizeOneCell);
        myHouse.stateHouse = StateHouse.InBlue;
        myHouse.currentColor = StateColor.Blue;
        myHouse.OpenCanvasHouse(myHouse.canvasWindows);
        myHouse.houseTextOnShop.buttonChange = buttonChange; // закинув для смени текста на кнопках, треба тільки на тих шо беру
        _house = myHouse;
        //End(x,z,myHouse,false);
    }
    private int x;
    private int _Px;
    private int z;
    private int _Pz;

    private bool ChangePos
    {
        get
        {
            if(_Px != x || _Pz != z)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public void MoveHouse(int _xMin,int _zMin)
    {
        if (_house != null)
        {
            if (myHouse == null)
            {
                x = _house.dataHouse.posit.x;
                z = _house.dataHouse.posit.z;
                _Px = x;
                _Pz = z;
                canPut = true;
                myHouse = _house;
            }
            if (_house.Drag && _house.stateHouse != StateHouse.NotActive)
            {
                var newplane = new Plane(Vector3.up, Vector3.zero);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (newplane.Raycast(ray, out float position))
                {
                    Vector3 worldPos = ray.GetPoint(position);
                    x = Mathf.RoundToInt(worldPos.x / SizeOneCell) - _xMin;
                    z = Mathf.RoundToInt(worldPos.z / SizeOneCell) - _zMin;
                    myHouse.transform.position = new Vector3(Posit.InitInPosit(x,z,myHouse).x, myHouse.transform.position.y, Posit.InitInPosit(x, z, myHouse).y);
                }
            }
        }
        Upd();
        void Upd()
        {
            if (myHouse != null)
            {

                if (ChangePos)
                {
                    
                    _Px = x;
                    _Pz = z;
                    canPut = CanPut(x, z);
                }
                if (canPut)
                {
                    if (myHouse.Drag)
                    {
                        myHouse.stateHouse = StateHouse.IsActive;
                        myHouse.currentColor = StateColor.Green;
                    }
                    else
                    {
                        myHouse.stateHouse = StateHouse.InBlue;
                        myHouse.currentColor = StateColor.Blue;
                    }
                }
                else
                {
                    myHouse.stateHouse = StateHouse.IsActive;
                    myHouse.currentColor = StateColor.Red;
                }
            }
        }
        if (myHouse != null && _house == null)
        {
            Debug.Log("nullMyHouse");
            myHouse = null;
        }
    }


    private bool CanPut(int X, int Z)
    {
        for (int x = X - myHouse.Sides.x / 2; x < X + myHouse.Sides.x / 2 + myHouse.NeParniX; x++)
        {
            for (int z = Z - myHouse.Sides.y / 2; z < Z + myHouse.Sides.y / 2 + myHouse.NeParniZ; z++)
            {
                if (x >= 0 && z >= 0 && x < _setMap.GetLength(0) && z < _setMap.GetLength(1))
                {
                    if (_setMap[x, z] == 1)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        return true;
    }
    public static void End(int X, int Z,House _house__, bool fullEndNotSave, bool saveJson = true)
    {
        if (fullEndNotSave)
        {
            if (_house__.currentColor == StateColor.Red)
            {
                _house__.ReturnCell(_house__);
                X = Posit.DesWithPosit(_house__.transform.position.x, _house__.transform.position.z, _house__).x;
                Z = Posit.DesWithPosit(_house__.transform.position.x, _house__.transform.position.z, _house__).y;
            }
            _house__.stateHouse = StateHouse.NotActive;
            _house__.currentColor = StateColor.Normal;
            _house__.CloseCanvasHouse(_house__.canvasWindows);
            _house = null;
            myHouse = null;
        }
        else
        {
            if (_house__.currentColor == StateColor.Red)
            {
                return;
            }
        }
        _house__.dataHouse.posit = new Posit(X, Z);
        for (int x = X - _house__.Sides.x / 2; x < X + _house__.Sides.x / 2 + _house__.NeParniX; x++)
        {
            for (int z = Z - _house__.Sides.y / 2; z < Z + _house__.Sides.y / 2 + _house__.NeParniZ; z++)
            {
                Debug.Log(x + " " + z + "end");
                _setMap[x, z] = 1;
            }
        }
        if (saveJson)
        {
            ReturnAllOnStart.allData.allDataHouses[_house__.dataHouse.myIndexOnSave].dataHouse.posit = _house__.dataHouse.posit;
            ReturnAllOnStart.json.Save(ReturnAllOnStart.allData);
        }
    }

    public static void ZeroCell(House _house_)
    {
        int X = _house_.dataHouse.posit.x;
        int Z = _house_.dataHouse.posit.z;
        if (_house_.dataHouse.posit != null)
        {
            for (int x = X - _house_.Sides.x / 2; x < X + _house_.Sides.x / 2 + _house_.NeParniX; x++)
            {
                for (int z = Z - _house_.Sides.y / 2; z < Z + _house_.Sides.y / 2 + _house_.NeParniZ; z++)
                {
                    Debug.Log(x + " " + z + "zero");
                    _setMap[x, z] = 0;
                }
            }
        }
    }
    private bool CheckPosTakeHouse(ref int X, ref int Z)
    {
        int randomX;
        int randomZ;
        int iMax = 50;

        for (int i = 0; i < iMax; i++)
        {
            if (i < iMax/5)
            {
                //Debug.Log(1);
                randomX = Random.Range(MyTerrain.xMin / 2, 0);
                randomZ = Random.Range(MyTerrain.zMin / 2, 0);}
            else if(i < iMax*2/5)
            {
                //Debug.Log(2);
                randomX = Random.Range(MyTerrain.xMin, 0);
                randomZ = Random.Range(MyTerrain.zMin, 0);
            }
            else if (i < iMax*3/5)
            {
                //Debug.Log(3);
                randomX = Random.Range(MyTerrain.xMin, 0);
                randomZ = Random.Range(0, MyTerrain.zMax);
            }
            else if (i < iMax * 4 / 5)
            {
                //Debug.Log(4);
                randomX = Random.Range(0, MyTerrain.xMax);
                randomZ = Random.Range(MyTerrain.zMin, 0);
            }
            else
            {
                //Debug.Log(5);
                randomX = Random.Range(0, MyTerrain.xMax);
                randomZ = Random.Range(0, MyTerrain.zMax);
            }
            if (CanPut(randomX - MyTerrain.xMin, randomZ - MyTerrain.zMin))
            {
                Debug.Log(" First " + i);
                X += randomX - MyTerrain.xMin;
                Z += randomZ - MyTerrain.zMin;
                return true;
            }
            else
            {
                //Debug.Log(randomX + " " + randomZ + " prosto " + i);
            }
        }
        randomX = Random.Range(MyTerrain.xMin / 2, 0);
        randomZ = Random.Range(MyTerrain.zMin / 2, 0);
        Debug.Log(randomX + " " + randomZ + " last ");
        X += randomX - MyTerrain.xMin;
        Z += randomZ - MyTerrain.zMin;
        return true;  // пока не упрощу закомиченний, будет ета затичка


        /*
        bool ZeroPossipleWithRandom(ref int _X, ref int _Z) // по всей карте пройдет
        {
            for (int numberCircle = 0; numberCircle < Mathf.Min(Terrain.xMax, Terrain.zMax); numberCircle++)
            {
                for (int i = -numberCircle; i < 1 + numberCircle; i++)
                {
                    for (int j = -numberCircle; j < 1 + numberCircle; j++)
                    {
                        if (Mathf.Abs(i) == numberCircle || Mathf.Abs(j) == numberCircle)
                        {
                            Debug.Log(i + " " + j + " " + numberCircle);
                            if (CanPut(_X + i, _Z + j))
                            {
                                Debug.Log(i + " " + j + " " + numberCircle);
                                _X += i;
                                _Z += j;
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        */
    }
    private int St() //просто проверка
    {
        int d = 0;
        for(int i = 0; i < 25; i++)
        {
            d += A(i);
        }
        int A(int a)
        {
            return a * 7 + 1;
        }
        return d;
    }
}