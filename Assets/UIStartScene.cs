using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStartScene : MonoBehaviour
{
    [SerializeField] private CameraMove cameraMove;
    [SerializeField] private Button buttonShop;
    [SerializeField] private Button buttonBackShop;
    [SerializeField] private RectTransform shop;
    private void Awake()
    {
        ActiveOrNotShop(buttonShop,true);
        ActiveOrNotShop(buttonBackShop, false);
    }
    private void ActiveOrNotShop(Button button,bool _bool)
    {
        button.onClick.AddListener(() => {
            shop.gameObject.SetActive(_bool);
            cameraMove.possibleMove = !_bool;
        });
    }
}
