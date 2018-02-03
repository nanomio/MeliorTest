using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUI : MonoBehaviour
{

    private bool btnArrowState = false;

    public GameObject fadeCanvas;

    public GameObject
        popupPause,
        popupArcher,
        popupShop,
        popupGameOver;

    public GameObject defence;

    public TextMeshProUGUI
        countGold,
        countPoints,
        countIceblasts,
        countFireballs,
        countLightnings;

    private UnityAction GameOverListener;

    private void Awake()
    {
        GameOverListener = new UnityAction(GameOver);
    }

    private void OnEnable()
    {
        EventManager.StartListening("GameOver", GameOverListener);
    }

    private void OnDisable()
    {
        EventManager.StopListening("FortDamaged", GameOverListener);
    }

    public void Start()
    {
        countGold.text = GameModel.gold.ToString();
        countPoints.text = GameModel.points.ToString();

        countIceblasts.text = GameModel.iceblasts.ToString();
        countFireballs.text = GameModel.fireballs.ToString();
        countLightnings.text = GameModel.lightnings.ToString();
    }

    public void BtnPause()
    {
        fadeCanvas.SetActive(true);
        popupPause.SetActive(true);

        GameModel.play = false;

        EventManager.TriggerEvent("GamePause");
    }

    public void BtnArrow()
    {
        Animator panelBottomAnim = transform.Find("Bottom Panel").gameObject.GetComponent<Animator>();

        if ( btnArrowState )
        {
            panelBottomAnim.SetBool("BtnState", true);
        }
        else
        {
            panelBottomAnim.SetBool("BtnState", false);
        }

        btnArrowState = !btnArrowState;
    }

    public void BtnAddArcher()
    {

        if ( GameModel.archersCount < 3 )
        {

            GameObject addArcher = defence.transform.GetChild(GameModel.archersCount).gameObject;

            addArcher.SetActive(true);
            addArcher.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, .36f);

            //fadeCanvas.SetActive(true);
            popupArcher.SetActive(true);
        }
        else
        {

        }

    }

    public void BtnShop()
    {

        popupShop.SetActive(true);

    }

    public void GameOver()
    {

        fadeCanvas.SetActive(true);
        popupGameOver.SetActive(true);

    }

}
