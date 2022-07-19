using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Field : ScriptableObject
{
    [SerializeField]
    private GameObject field;
    public void CreateField()
    {
        int xMin = MyTerrain.xMin;
        int xMax = MyTerrain.xMax;
        int zMin = MyTerrain.zMin;
        int zMax = MyTerrain.zMax;

        GameObject gO = Instantiate(field);
        gO.transform.localScale = new Vector3((xMax - xMin) * MyTerrain.sizeOneCell, (zMax - zMin) * MyTerrain.sizeOneCell, 1);
        gO.GetComponent<Renderer>().material.mainTextureScale = new Vector2((xMax - xMin)/2, (zMax - zMin) / 2);
    }
}
