using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Notification : MonoBehaviour
{
    [SerializeField] private Text textNotification;
    public void CallNotification(string _text)
    {
        StartCoroutine(alphaNotification(_text));
    }
    private IEnumerator alphaNotification(string _text)
    {
        textNotification.text = _text;
        for (int i = 1; i <= 10; i++)
        {
            textNotification.color = new Color(textNotification.color.r, textNotification.color.g, textNotification.color.b, i);
            yield return new WaitForSeconds(1f / 50);
        }
        for (int i = 0; i < 10; i++)
        {
            textNotification.color = new Color(textNotification.color.r, textNotification.color.g, textNotification.color.b, 9 - i);
            yield return new WaitForSeconds(1f / 2);
        }
    }
}
