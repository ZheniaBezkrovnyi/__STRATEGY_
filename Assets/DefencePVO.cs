using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public class DefencePVO : AnimPVO
{
    public Transform rocket;
    public AnimationCurve curve;
    private void Start()
    {
        OnStart(rocket, curve);
        
    }
}

public class AnimPVO : MonoBehaviour
{
    public Vector3 positTarget;
    private Transform thisRocket;
    private AnimationCurve curve_;
    public void OnStart(Transform _thisRocket,AnimationCurve _curve)
    {
        thisRocket = _thisRocket;
        curve_ = _curve;
        StartCoroutine(AnimRocket(positTarget,2));
    }
    public IEnumerator AnimRocket(Vector3 target,float time)
    {
        Vector3 startPos = thisRocket.position;
        for (int i = 0; i < 250; i++)
        {
            yield return new WaitForSeconds(time);
            LaunchRocket(startPos, positTarget + new Vector3(UnityEngine.Random.Range(-80,80), UnityEngine.Random.Range(-20, 20), UnityEngine.Random.Range(-280, 280)), time);

        }
    }

    private void LaunchRocket(Vector3 startPos, Vector3 target, float time)
    {
        Vector3 diffToTarget = target - startPos;
        Vector3[] allPoint = new Vector3[10];

        float MaxValue = 0;
        GetMaxValueEvaluate();
        InitPoint();
        Flying();

        ReplaceRocket(startPos,time);

        void GetMaxValueEvaluate()
        {
            for (int i = 0; i < allPoint.Length; i++)
            {
                if (curve_.Evaluate((float)(i + 1) / (float)allPoint.Length) > MaxValue)
                {
                    MaxValue = curve_.Evaluate((float)(i + 1) / (float)allPoint.Length);
                }
            }
        }
        void InitPoint()
        {
            for (int i = 0; i < allPoint.Length; i++)
            {
                float zeroToOne = (float)(i + 1) / (float)allPoint.Length;
                float valueEvaluate = curve_.Evaluate(zeroToOne);
                allPoint[i] = startPos + new Vector3(
                    diffToTarget.x * zeroToOne,
                    diffToTarget.y * valueEvaluate,
                    diffToTarget.z * zeroToOne
                    );
            }
        }
        void Flying()
        {
            Transform flyRocket = Instantiate(thisRocket, startPos, thisRocket.rotation);
            flyRocket.localScale *= MyTerrain.sizeOneCell;

            flyRocket.DOPath(allPoint, time).
                SetLookAt(target).
                OnComplete(() =>
                    Destroy(flyRocket.gameObject)
                );
        }
    }
    private void ReplaceRocket(Vector3 _startPos, float time)
    {
        thisRocket.position = _startPos + new Vector3(0,-5,0);
        thisRocket.DOMove(_startPos, time*0.95f);
    }
}