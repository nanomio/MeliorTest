using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FortView : MonoBehaviour
{
    public Slider barHP;

    private UnityAction<int> TakeDamageListener;

    private const sbyte lastState = 2;

    private int maxHP;
    private int fortState = 0;

    private void Awake()
    {
        TakeDamageListener = new UnityAction<int>(TakeDamage);
    }

    private void OnEnable()
    {
        EventManager.StartListening("FortDamaged", TakeDamageListener);
    }

    private void OnDisable()
    {
        EventManager.StopListening("FortDamaged", TakeDamageListener);
    }

    private void Start()
    {
        maxHP = GameModel.hp;
        barHP.value = 1.0f;
    }

    public void TakeDamage(int damage)
    {
        if (GameModel.hp > 0)
        {
            GameModel.hp -= damage;

            barHP.value = (float)GameModel.hp / maxHP;
            if (fortState != lastState - GameModel.hp / (maxHP * 1 / (lastState + 1)))
            {
                fortState = lastState - GameModel.hp / (maxHP * 1 / (lastState + 1));

                transform.GetComponent<Animator>().Play("FortDamage", 0, (float) fortState / (lastState + 1) );
            }
        }
        else
        {
            transform.GetComponent<Animator>().Play("FortDamage", 0, .75f);

            GameModel.play = false;
            EventManager.TriggerEvent("GameOver");
            Debug.Log("Game over");
        }
    }
}
