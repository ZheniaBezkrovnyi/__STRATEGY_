using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelHeroes : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private RectTransform scrollRect;
    [SerializeField] private RectTransform canvas;
    [SerializeField] private RectTransform buttonPrefab;
    [SerializeField] private ReturnAllOnStart returnAllStart;
    [SerializeField] private Money money;
    [SerializeField] private List<NameHouse> listHeroes;

    private bool IsCreate;
    private Vector3 localScaleCanvas;
    private void OnEnable()
    {
        InitializeShop();
    }
    private void InitializeShop()
    {
        IsCreate = true;
        localScaleCanvas = canvas.localScale;
        float heightCanvas = canvas.rect.height * canvas.localScale.y;
        float widthCanvas = canvas.rect.width * canvas.localScale.x;
        float heightContent = heightCanvas * (scrollRect.anchorMax.y - scrollRect.anchorMin.y) * (content.anchorMax.y - content.anchorMin.y);
        float widthContent = widthCanvas * (scrollRect.anchorMax.x - scrollRect.anchorMin.x) * (content.anchorMax.x - content.anchorMin.x);
        float diffX = widthCanvas * (scrollRect.anchorMin.x + (scrollRect.anchorMax.x - scrollRect.anchorMin.x) * content.anchorMin.x);
        float diffY = heightCanvas * (scrollRect.anchorMin.y + (scrollRect.anchorMax.y - scrollRect.anchorMin.y) * content.anchorMax.y);
        float DiffContent = (diffWidth()*2 + buttonPrefab.rect.width * canvas.localScale.x + (5-1) * (buttonPrefab.rect.width * canvas.localScale.x + diffWidth())) / widthContent;
        if (DiffContent > 1)
        {
            content.anchorMax = new Vector2(DiffContent, 1);
        }

        for (int i = 0; i < listHeroes.Count; i++)
        {
            CreatePanel(i);
        }



        void CreatePanel(int I)
        {
            GeneralHouse _house = null;
            for (int i = 0; i < returnAllStart.allHousePrefab.listHouse.Count; i++) //з енама витягується, все норм тут, але в майбутньому оптимізувати
            {
                if (listHeroes[I] == returnAllStart.allHousePrefab.listHouse[i][0].dataHouse.NameThisHouse)
                {
                    _house = returnAllStart.allHousePrefab.listHouse[i][0];
                    break;
                }
                if (i == returnAllStart.allHousePrefab.listHouse.Count - 1)
                {
                    Debug.Log("not exist " + listHeroes[I]);
                    return;
                }
            }

            RectTransform panel = Instantiate(buttonPrefab);
/*
            void OnButton()
            {
                
            }
            _button.onClick.AddListener(OnButton);
*/
            panel.gameObject.SetActive(true);
            panel.SetParent(content);
            panel.position = new Vector3(diffWidth() + diffX + panel.rect.width / 2 * canvas.localScale.x + (I%5) * (panel.rect.width * canvas.localScale.x + diffWidth()),
                diffY - panel.rect.height / 2 * canvas.localScale.y - Height(),
                0
            );
            panel.localScale = new Vector3(1, 1, 1);
            float Height()
            {
                if(I < 5)
                {
                    return diffHeight();
                }
                else
                {
                    return diffHeight()*2 + buttonPrefab.rect.height * canvas.localScale.y;
                }

                float diffHeight()
                {
                    return (heightContent - 2 * buttonPrefab.rect.height * canvas.localScale.y)/3;
                }
            }

        }
        float diffWidth()
        {
            return (widthContent - 5 * buttonPrefab.rect.width * canvas.localScale.x) / 6;
        }
    }
}
