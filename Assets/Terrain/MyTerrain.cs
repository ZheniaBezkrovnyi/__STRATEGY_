using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTerrain : MonoBehaviour
{
    private int[,] setMap; // не сохранять, но при восстановлении зданий с ними вместе end визивать
    [SerializeField] private List<Vector2Int> pointsMap;
    public static int xMin, xMax,zMin,zMax;
    private TakeObjects takeObjects;
    [SerializeField] private GameObject zatychki;
    [SerializeField] private Field field;
    public static int sizeOneCell;
    [SerializeField] private int SizeOneCell;
    //public List<string> l;
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
        /*
        l = new List<string>();
        for (int i=0; i < setMap.GetLength(0); i++)
        {
            for (int j = 0; j < setMap.GetLength(1); j++)
            {
                l.Add(setMap[i, j] + " /" + i + " / " + j);
            }
        }
        */
    }
    public void TakeHouse(House house)
    {
        takeObjects.TakeHouse(house);
    }
}

public class TakeObjects : MonoBehaviour
{
    public static House _house;
    private static int[,] _setMap;
    private House myHouse;
    private bool canPut = true;
    private int SizeOneCell;
    public TakeObjects(int[,] _setMap_)
    {
        _setMap = _setMap_;
        SizeOneCell = MyTerrain.sizeOneCell;
    }
    public void TakeHouse(House house) // при взятии переносить камеру на видимую зону обьекта чрез интерполяцию
    { 
        if (_house != null) Destroy(_house.gameObject);

        _house = Instantiate(house);
        myHouse = _house;
        x = 0;
        z = 0;
        CheckPosTakeHouse(ref x, ref z);
        Debug.Log(x + " "+ z + "startPos");
        myHouse.transform.position += new Vector3(x * SizeOneCell + MyTerrain.xMin * SizeOneCell + (float)myHouse.NeParniX / 2f* SizeOneCell, myHouse.transform.localScale.y / 2, z * SizeOneCell + MyTerrain.zMin * SizeOneCell + (float)myHouse.NeParniZ / 2f * SizeOneCell);
        myHouse.stateHouse = StateHouse.IsActive;
        myHouse.startMove = true;
        myHouse.InitColor(StateColor.Norm);
        //Debug.Log(myHouse.dataTextOnButton.textTimeBuild);
        //Debug.Log(x +" "+z);
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
        if (_house != null && _house.Drag)
        {
            if(myHouse == null)
            {
                myHouse = _house;
            }
            var newplane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (newplane.Raycast(ray, out float position))
            {
                Vector3 worldPos = ray.GetPoint(position);
                x = Mathf.RoundToInt(worldPos.x / SizeOneCell) - _xMin;
                z = Mathf.RoundToInt(worldPos.z / SizeOneCell) - _zMin;
                myHouse.transform.position = new Vector3((x + (float)myHouse.NeParniX / 2f + _xMin) * SizeOneCell, myHouse.transform.position.y, (z + (float)myHouse.NeParniZ / 2f + _xMin) * SizeOneCell);                
            }
            if (myHouse.startMove)
            {
                myHouse.startMove = false;
                ZeroCell();
            }
        }
        Upd();
        void Upd()
        {
            if (ChangePos)
            {
                _Px = x;
                _Pz = z;
                canPut = CanPut(x, z);
            }
            if (myHouse != null)
            {
                if (canPut)
                {
                    myHouse.currentColor = StateColor.Norm;
                }
                else
                {
                    myHouse.currentColor = StateColor.Red;
                }
            }
        }
        if (_house == null && myHouse != null && myHouse.endMove)
        {
            Debug.Log("end");
            myHouse.endMove = false;
            End(x, z,myHouse);
            myHouse.existOrNot = true;
            myHouse = null;
        }
        else if (myHouse != null && _house == null)
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
                    //Debug.Log(x + " " + z);
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
    public static void End(int X, int Z,House _house)
    {
        _house.posOnMap = new int[2, _house.Sides.x, _house.Sides.y];
        for (int x = X - _house.Sides.x / 2, dX = 0; x < X + _house.Sides.x / 2 + _house.NeParniX; x++,dX++)
        {
            for (int z = Z - _house.Sides.y / 2, dZ = 0; z < Z + _house.Sides.y / 2 + _house.NeParniZ; z++,dZ++)
            {
                Debug.Log(x + " " + z + "end");
                _setMap[x, z] = 1;
                _house.posOnMap[0, dX, dZ] = x;
                _house.posOnMap[1, dX, dZ] = z;
            }
        }
    }
    private void ZeroCell()
    {
        if (myHouse.posOnMap != null)
        {
            for (int i = 0; i < myHouse.posOnMap.GetLength(1); i++)
            {
                for (int j = 0; j < myHouse.posOnMap.GetLength(2); j++)
                {
                    Debug.Log(myHouse.posOnMap[0, i, j] + " " + myHouse.posOnMap[1, i, j] + "zero");
                    _setMap[myHouse.posOnMap[0, i, j], myHouse.posOnMap[1, i, j]] = 0;
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
                Debug.Log(1);
                randomX = Random.Range(MyTerrain.xMin / 2, 0);
                randomZ = Random.Range(MyTerrain.zMin / 2, 0);}
            else if(i < iMax*2/5)
            {
                Debug.Log(2);
                randomX = Random.Range(MyTerrain.xMin, 0);
                randomZ = Random.Range(MyTerrain.zMin, 0);
            }
            else if (i < iMax*3/5)
            {
                Debug.Log(3);
                randomX = Random.Range(MyTerrain.xMin, 0);
                randomZ = Random.Range(0, MyTerrain.zMax);
            }
            else if (i < iMax * 4 / 5)
            {
                Debug.Log(4);
                randomX = Random.Range(0, MyTerrain.xMax);
                randomZ = Random.Range(MyTerrain.zMin, 0);
            }
            else
            {
                Debug.Log(5);
                randomX = Random.Range(0, MyTerrain.xMax);
                randomZ = Random.Range(0, MyTerrain.zMax);
            }
            if (CanPut(randomX - MyTerrain.xMin, randomZ - MyTerrain.zMin))
            {
                Debug.Log(randomX + " " + randomZ + " First " + i);
                X += randomX - MyTerrain.xMin;
                Z += randomZ - MyTerrain.zMin;
                return true;
            }
            else
            {
                Debug.Log(randomX + " " + randomZ + " prosto " + i);
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