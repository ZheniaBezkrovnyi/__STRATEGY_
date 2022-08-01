using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class AnimTimeBuild : MonoBehaviour
{
    [SerializeField] private RectTransform canvasSlider;
    [SerializeField] private Transform cameraMain;
    [SerializeField] private CanvasHouse canvasHouse;
    [SerializeField] private Text textTime;
    [SerializeField] private Notification notification;
    public IEnumerator BeginBuildHouse(MainHouse _house, bool beginOrContinue)
    {
        TimeBuild timeBuild;
        #region InitTimeBuild
        if (_house.existOrNot != ExistOrNot.Yes)
        {
            timeBuild = _house.houseTextOnShop.TimeNeedBuildStart;
        }
        else
        {
            timeBuild = _house.dataTextOnHouse.timeImprove;
        }
        #endregion
        DateTime dateTimeStart = DateTime.Now;
        int remainsSeconds = 0;
        TimeDateTime timeEndTime = null;
        if (beginOrContinue)
        {
            timeEndTime = TimeDateTime.GetSum(TimeDateTime.ToTimeDateTime(dateTimeStart.ToString()), timeBuild);
            
            _house.dataHouse.dataAnimBuildHouse.timeEndBuild = timeEndTime;
            if (_house.existOrNot == ExistOrNot.Yes)
            {
                canvasHouse.OpenCanvasHouse(_house);
            }

            ReturnAllOnStart.allData.allDataHouses[_house.dataHouse.myIndexOnSave].dataHouse.dataAnimBuildHouse.timeEndBuild = timeEndTime;
            ReturnAllOnStart.json.Save(ReturnAllOnStart.allData);
            remainsSeconds = TimeBuild.InSeconds(timeBuild);
        }
        else
        {
            TimeDateTime diffTime = TimeDateTime.TimeMinusTime(_house.dataHouse.dataAnimBuildHouse.timeEndBuild, TimeDateTime.ToTimeDateTime(dateTimeStart.ToString()));
            int diffTimeInSeconds = TimeDateTime.InSeconds(diffTime);
            remainsSeconds = diffTimeInSeconds;
            if(remainsSeconds < 0)
            {
                remainsSeconds = 0;
            }
        }
        if(remainsSeconds == 0)
        {
            IfTimeIsUp(_house, canvasSlider,false);
            yield break;
        }
        RectTransform rectCanvasSlider = InstCanvasSlider(_house);
        Slider slider = rectCanvasSlider.GetChild(1).GetChild(0).GetComponent<Slider>();
        Text _textTime = rectCanvasSlider.GetChild(0).GetComponent<Text>();

        int timeForBuild = TimeBuild.InSeconds(timeBuild);
        TimeBuild _timeBuild;

        float timeStart = Time.time - 1;
        if (remainsSeconds == 0 && TakeObjects._house == _house) { canvasHouse.OpenCanvasHouse(_house); }
        for (; remainsSeconds > 0; )
        {

            if (Time.time - timeStart >= 1)
            {
                timeStart++;
                --remainsSeconds;
                slider.value = ((float)timeForBuild - (float)remainsSeconds) / (float)timeForBuild;

                _timeBuild = TimeBuild.WithSecondsToTime(remainsSeconds);
                _textTime.text = TimeBuild.ToString(_timeBuild);
            }
            yield return new WaitForSeconds(1f);
        }

        IfTimeIsUp(_house, rectCanvasSlider,true);
    }

    private void IfTimeIsUp(MainHouse _house,RectTransform canvasSlider,bool removeCanvasSlider)
    {
        _house.dataHouse.dataAnimBuildHouse.timeEndBuild = new TimeDateTime(0, 0, 0, 0, 0, 0);
        if (removeCanvasSlider)
        {
            Destroy(canvasSlider.gameObject);
        }
        if (_house.existOrNot == ExistOrNot.Yes)
        {
            MainHouse houseNew = Instantiate(_house.housesNextPrefab,_house.transform.position,_house.transform.rotation);
            SaveInJSON saveInJSON = new SaveInJSON();
            saveInJSON.SaveInsteadThisTwoHouseInList(houseNew,_house);
            Destroy(_house.gameObject);

            if (TakeObjects._house == _house) {
                GeneralHouse.IfClick(houseNew,canvasHouse);
            }
        }
        else
        {
            _house.existOrNot = ExistOrNot.Yes;
            _house.dataHouse.levelHouse = 1;

            ReturnAllOnStart.allData.allDataHouses[_house.dataHouse.myIndexOnSave].dataHouse.dataAnimBuildHouse.timeEndBuild = new TimeDateTime(0, 0, 0, 0, 0, 0);
            ReturnAllOnStart.allData.allDataHouses[_house.dataHouse.myIndexOnSave].dataHouse.levelHouse = 1;
            ReturnAllOnStart.json.Save(ReturnAllOnStart.allData);

            if (TakeObjects._house == _house) { canvasHouse.OpenCanvasHouse(_house); }
        }
    }







    private Quaternion angleCamera;
    private RectTransform InstCanvasSlider(GeneralHouse _house)
    {
        angleCamera = Quaternion.Euler(cameraMain.rotation.eulerAngles.x, cameraMain.rotation.eulerAngles.y,0);
        RectTransform canvasSLIDER = Instantiate(canvasSlider);
        canvasSLIDER.gameObject.SetActive(true);
        Transform trHouse = _house.transform;
        canvasSLIDER.SetParent(trHouse);
        float diffX = trHouse.position.x;
        float diffY = trHouse.position.y;
        float diffZ = trHouse.position.z;
        canvasSLIDER.position = new Vector3(diffX, diffY + 0.75f * trHouse.localScale.y, diffZ);
        canvasSLIDER.rotation = angleCamera;

        int minSide = Mathf.Min(_house.Sides.x, _house.Sides.y);
        canvasSLIDER.localScale = new Vector3(canvasSLIDER.localScale.x * Mathf.Sqrt(2 * minSide * minSide), canvasSLIDER.localScale.y, canvasSLIDER.localScale.z);

        return canvasSLIDER;
    }
}
