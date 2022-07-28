using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class AnimTimeBuild : MonoBehaviour
{
    [SerializeField] private Text textTime;
    [SerializeField] private RectTransform canvasSlider;
    [SerializeField] private Transform cameraMain;
    [SerializeField] private CanvasHouse canvasHouse;
    [SerializeField] private Notification notification;
    public IEnumerator BeginBuildHouse(House _house, bool beginOrContinue)
    {
        notification.CallNotification("StartAnim");
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
        notification.CallNotification("StartAnim2");
        TimeDateTime timeEndTime = null;
        if (beginOrContinue)
        {
            notification.CallNotification(dateTimeStart.ToString());
            timeEndTime = TimeDateTime.GetSum(TimeDateTime.ToTimeDateTime(dateTimeStart.ToString()), timeBuild);
            

            _house.dataHouse.dataAnimBuildHouse.timeEndBuild = timeEndTime;
            ReturnAllOnStart.allData.allDataHouses[_house.dataHouse.myIndexOnSave].dataHouse.dataAnimBuildHouse.timeEndBuild = timeEndTime;
            ReturnAllOnStart.json.Save(ReturnAllOnStart.allData);
            remainsSeconds = TimeBuild.InSeconds(timeBuild);
            //notification.CallNotification("StartAnim4");
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
        //notification.CallNotification("StartAnim*");
        RectTransform rectCanvasSlider = InstCanvasSlider(_house);
        //notification.CallNotification("StartAnim**");
        Slider slider = rectCanvasSlider.GetChild(0).GetChild(0).GetComponent<Slider>();
        //notification.CallNotification("StartAnim***");


        int timeForBuild = TimeBuild.InSeconds(timeBuild);
        float timeStart = Time.time - 1;
        //notification.CallNotification("StartAnim5");
        for (; remainsSeconds > 0; )
        {
            if (TakeObjects._house == _house) { canvasHouse.OpenCanvasHouse(_house); }

            if (Time.time - timeStart >= 1)
            {
                timeStart++;
                --remainsSeconds;
                slider.value = ((float)timeForBuild - (float)remainsSeconds) / (float)timeForBuild;
            }
            yield return new WaitForSeconds(1f); //перевірка зверху на час має бути, щоб неточності коректувати
        }
        //notification.CallNotification("StartAnim6");
        IfTimeIsUp(_house, rectCanvasSlider,true);
        //notification.CallNotification("StartAnim7");
    }

    private void IfTimeIsUp(House _house,RectTransform canvasSlider,bool removeCanvasSlider)
    {
        _house.dataHouse.dataAnimBuildHouse.timeEndBuild = new TimeDateTime(0, 0, 0, 0, 0, 0);
        if (removeCanvasSlider)
        {
            Destroy(canvasSlider.gameObject);
        }
        if (_house.existOrNot == ExistOrNot.Yes)
        {
            //Debug.Log(_house.houseTextOnShop.dataHouseChangeOnText.currentBuildThisHouse);
            //Debug.Log("начало замени здания");
            House houseNew = Instantiate(_house.housesNextPrefab,_house.transform.position,_house.transform.rotation);
            SaveInJSON saveInJSON = new SaveInJSON();
            saveInJSON.SaveInsteadThisTwoHouseInList(houseNew,_house);
            Destroy(_house.gameObject);
            Debug.Log("конец замени здания");

            if (TakeObjects._house == _house) {
                House.IfClick(houseNew,canvasHouse);
            }
        }
        else
        {
            _house.existOrNot = ExistOrNot.Yes;
            _house.dataHouse.levelHouse = 1;

            /*ReturnAllOnStart.allData.allDataHouses[_house.dataHouse.myIndexOnSave].dataHouse.dataAnimBuildHouse.timeEndBuild = new TimeDateTime(0, 0, 0, 0, 0, 0);
            ReturnAllOnStart.allData.allDataHouses[_house.dataHouse.myIndexOnSave].dataHouse.levelHouse = 1;
            ReturnAllOnStart.json.Save(ReturnAllOnStart.allData);*/

            if (TakeObjects._house == _house) { canvasHouse.OpenCanvasHouse(_house); }
        }
        notification.CallNotification("EndAnim");
    }







    private Quaternion angleCamera;
    private RectTransform InstCanvasSlider(House _house)
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
