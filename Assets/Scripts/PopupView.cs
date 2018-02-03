using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupView : MonoBehaviour
{

    public GameObject fadeBlack;

    public void BtnPlay()
    {
        GameModel.play = true;
        fadeBlack.SetActive(false);

        EventManager.TriggerEvent("GamePlay");

        transform.gameObject.SetActive(false);
    }

    public void BtnQuit()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void BtnReplay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
