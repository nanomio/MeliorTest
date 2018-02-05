using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class GameUI : MonoBehaviour
{

    private bool
        btnArrowState;

    public GameObject fadeCanvas;

    public GameObject
        popupPause,
        popupArcher,
        popupShop,
        popupGameOver,
        popupNextWave,
        popupWinGame;

    public Slider barProgress;
    public GameObject defence;

    public TextMeshProUGUI
        countGold,
        countPoints,
        countBoosts,
        countRepairs,
        countFireballs,
        
        mainTimer;

    private UnityAction
        GameOverListener,
        GameWinListener,
        WavePopupListener;

    private UnityAction<int>
        HitListener;


    private void Awake()
    {
        GameOverListener = new UnityAction(GameOver);
        GameWinListener = new UnityAction(GameWin);
        WavePopupListener = new UnityAction(WavePopupControl);

        HitListener = new UnityAction<int>(HitHandler);
    }

    private void OnEnable()
    {
        EventManager.StartListening("GameOver", GameOverListener);
        EventManager.StartListening("GameWin", GameWinListener);
        EventManager.StartListening("WavePopup", WavePopupListener);

        EventManager.StartListening("ArrowHit", HitListener);
    }

    private void OnDisable()
    {
        EventManager.StopListening("GameOver", GameOverListener);
        EventManager.StopListening("GameWin", GameWinListener);
        EventManager.StopListening("WavePopup", WavePopupListener);

        EventManager.StopListening("ArrowHit", HitListener);
    }

    public void Start()
    {
        btnArrowState = false;

        countGold.text = GameModel.gold.ToString();
        countPoints.text = GameModel.points.ToString();

        countBoosts.text = GameModel.countBoosts.ToString();
        countRepairs.text = GameModel.countRepairs.ToString();
        countFireballs.text = GameModel.countFireballs.ToString();
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

        if ( GameModel.countArchers < 3 )
        {

            GameObject addArcher = defence.transform.GetChild(GameModel.countArchers).gameObject;

            addArcher.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, .36f);

            popupArcher.SetActive(true);

        }

    }

    public void WavePopupControl()
    {

//      popupNextWave.SetActive(true);
        popupNextWave.GetComponent<Animator>().SetBool("On", false);

        StartCoroutine(CountDown(GameModel.waveWaitTime));

    }

    public void BtnGo()
    {

        GameModel.waiting = false;
        StopAllCoroutines();

        mainTimer.text = "GO";

        popupNextWave.GetComponent<Animator>().SetBool("On", true);
//      popupNextWave.SetActive(false);

    }

    public void BtnShop()
    {

        popupShop.SetActive(true);

    }

    public void GameWin()
    {

        fadeCanvas.SetActive(true);
        popupWinGame.SetActive(true);

    }

    public void GameOver()
    {

        fadeCanvas.SetActive(true);
        popupGameOver.SetActive(true);

    }

    public void HitHandler(int model)
    {

        GameModel.waveProgress -= GameModel.archer[model].damage;
        barProgress.value = (float)GameModel.waveProgress / GameModel.waveHP;

        if (GameModel.waveProgress < 0)
            Debug.Log("Ooops");

    }

    private IEnumerator CountDown(float waitTime)
    {
        float timer = 0;
        int
            current,
            last = current = Mathf.RoundToInt(waitTime);

        while (timer < GameModel.waveWaitTime)
        {

            if (current != last - Mathf.RoundToInt(timer))
            {
                current = last - Mathf.RoundToInt(timer);
                mainTimer.text = current.ToString();
            }

            timer += GameModel.dT;
            yield return new WaitForSeconds(GameModel.dT);

            if (!GameModel.play) yield return new WaitUntil(() => GameModel.play);
        }

        BtnGo();

    }

}
