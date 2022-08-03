using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasHouseStart : MonoBehaviour
{
    public Button buttonCanvasStartYes, buttonCanvasStartNo;
    [SerializeField] private AnimTimeBuild animTimeBuild;
    [SerializeField] private Money money;
    public Canvas canvasHouseStart;


    public void RealizeCanvasHouseStart(Button _button) //оптимізувати
    {
        if (_button == buttonCanvasStartYes || _button == buttonCanvasStartNo) {
            if (_button.name == "Yes")   // закинуть сюда же загрузку временем сразу
            {
                GeneralHouse thisHouse = TakeObjects._house;
                SaveInJSON saveInJSON = new SaveInJSON();
                saveInJSON.AddThisHouseInList(thisHouse);
                TakeObjects.End(
                    Posit.DesWithPosit(thisHouse.transform.position.x, thisHouse.transform.position.z, thisHouse).x,
                    Posit.DesWithPosit(thisHouse.transform.position.x, thisHouse.transform.position.z, thisHouse).y,
                    thisHouse,
                    true
                );
                if (thisHouse.GetComponent<MainHouse>())
                {
                    StartCoroutine(animTimeBuild.BeginBuildHouse((MainHouse)thisHouse, true));
                    thisHouse.existOrNot = ExistOrNot.Almost;
                }
                else
                {
                    thisHouse.existOrNot = ExistOrNot.Yes;
                }
                money.ChangeMoney(-thisHouse.houseTextOnShop.priceForBuild, thisHouse.houseTextOnShop.typeMoney);
            }
            else if (_button.name == "No")
            {
                Destroy(TakeObjects._house.gameObject);
                TakeObjects._house = null;
            }
            else
            {
                Debug.LogError("щось з назвами у канвас хаус старт");
            }
        }
    }
}
