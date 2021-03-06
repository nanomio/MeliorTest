﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AddArcherPopupView : MonoBehaviour
{

    public GameObject defence;
    public TextMeshProUGUI countGold;

    public RuntimeAnimatorController[] ArcherAnim;

    public void OnEnable()
    {

    }

    public void BtnClose()
    {

        GameObject addArcher = defence.transform.GetChild(GameModel.countArchers).gameObject;
        addArcher.SetActive(false);

        ClosePopup();

    }

    public void BtnAddArcher(int num)
    {
        if (GameModel.gold >= GameModel.archer[num].price)
        {
            GameObject addArcher = defence.transform.GetChild(GameModel.countArchers).gameObject;

            addArcher.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 255f);
            addArcher.GetComponent<Animator>().runtimeAnimatorController = ArcherAnim[num];

            addArcher.GetComponent<ArcherView>().modelNumber = num;
            addArcher.GetComponent<ArcherView>().Activate();

            GameModel.gold -= GameModel.archer[num].price;
            GameModel.countArchers++;

            ClosePopup();
        }
    }

    private void ClosePopup()
    {

        countGold.text = GameModel.gold.ToString();

        transform.gameObject.SetActive(false);

    }
}
