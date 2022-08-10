using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MainHouse
{
    [SerializeField] private Transform thisWall;
    private Vector2 sizeColliderXY;
    public bool continueFind;

    private void Update()
    {
        if (End)
        {
            End = false;
            StartCoroutine(FindRotate());
        }
    }
    private IEnumerator FindRotate()
    {
        yield return null;
    }





    private void StartCollider()
    {
        sizeColliderXY = new Vector2(thisWall.GetComponent<BoxCollider>().size.x, thisWall.GetComponent<BoxCollider>().size.y);
    }
    private float MaxColliderX()
    {
        return 3f / (float)(thisWall.localScale.x);
    }
}
