using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Attack : MonoBehaviour
{
    //переробить, це просто поки накидка
    public void GoAttack()
    {
        SceneManager.LoadScene("SceneAttack");
    }
    public void BackAttack()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
