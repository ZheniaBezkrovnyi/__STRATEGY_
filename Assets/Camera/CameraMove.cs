using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class CameraMove : MonoBehaviour
{
    private float angleY;
    public Transform transformThis;
    private Vector2 posTouch1;
    private Vector2 posTouch2;
    private bool boolTouch;
    private int yMax,yMin;
    private int SizeCell;
    public static bool possibleMove = true;
    private void Awake()
    {
        angleY = transform.rotation.eulerAngles.y;
        SizeCell = MyTerrain.sizeOneCell;
        yMin = 15 * SizeCell / 2;
        yMax = yMin * 5;
        transformThis.position = new Vector3(transformThis.position.x, yMin*2, transformThis.position.z);
    }
    private void FixedUpdate()
    {
        MoveCamera();
        if (!boolTouch)
        {
            boolTouch = true;
            posTouch1.x = 0;
            posTouch2.y = 0;
        }
    }

    private void MoveCamera()
    {
        boolTouch = false;
        if (Input.touchCount > 0 && possibleMove)
        {
            boolTouch = true;
            float PositionX = transformThis.position.x;
            float PositionY = transformThis.position.y;
            float PositionZ = transformThis.position.z;
            UnityEngine.Touch touch1 = Input.GetTouch(0);
            if (touch1.phase == TouchPhase.Moved)
            {
                if (Input.touchCount == 1)
                {
                    Debug.Log(possibleMove);
                    float deltaTouchX1 = touch1.deltaPosition.x;
                    float deltaTouchY1 = touch1.deltaPosition.y;
                    if (TakeObjects._house == null || (TakeObjects._house != null && !TakeObjects._house.Drag))
                    {

                        //Debug.Log(deltaTouchX1);
                        float differentX = PositionX - (deltaTouchX1 / 400 * PositionY + deltaTouchY1 / 400 * PositionY);
                        float differentZ = PositionZ -(-deltaTouchX1 / 400 * PositionY + deltaTouchY1 / 400 * PositionY);
                        transformThis.position = new Vector3(Mathf.Clamp(differentX, -40* SizeCell, 25* SizeCell), PositionY, Mathf.Clamp(differentZ, -40* SizeCell, 25* SizeCell));
                    }

                }
                if (Input.touchCount >= 2)
                {
                    UnityEngine.Touch touch2 = Input.GetTouch(1);
                    Vector2 _posTouch1 = touch1.position;
                    Vector2 _posTouch2 = touch2.position;
                    //Debug.Log(_posTouch1);
                    //Debug.Log(_posTouch2);
                    float diffPos = Vector2.Distance(_posTouch1, _posTouch2) - Vector2.Distance(posTouch1, posTouch2);
                    //Debug.Log(diffPos);
                    if (posTouch1.x != 0 && posTouch2.y != 0)
                    {
                        float differentY = PositionY - (diffPos/17 * SizeCell);
                        transformThis.position = new Vector3(PositionX, Mathf.Clamp(differentY, yMin, yMax), PositionZ);
                    }
                    posTouch1 = _posTouch1;
                    posTouch2 = _posTouch2;
                }
                else
                {
                    posTouch1.x = 0;
                    posTouch2.y = 0;
                }
            }
            
        }
    }
}
