using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopPopupView : MonoBehaviour
{

    private const sbyte
        CostBoost = 50,
        CostRepair = 50,
        CostFireball = 65;

    public TextMeshProUGUI
        countGold,
        countBoosts,
        countRepairs,
        countFireballs;

    public Animator animatorPanelBottom;

    public void OnEnable()
    {

        if (animatorPanelBottom.GetBool("BtnState")) animatorPanelBottom.SetBool("BtnState", false);

    }

    public void BtnClose()
    {
        ClosePopup();
    }

    public void BtnAddBoost()
    {
        if (GameModel.gold >= CostBoost)
        {
            GameModel.gold -= CostBoost;
            GameModel.countBoosts++;

            countGold.text = GameModel.gold.ToString();
            countBoosts.text = GameModel.countBoosts.ToString();
        }
    }

    public void BtnAddRepair()
    {
        if (GameModel.gold >= CostRepair)
        {
            GameModel.gold -= CostRepair;
            GameModel.countRepairs++;

            countGold.text = GameModel.gold.ToString();
            countRepairs.text = GameModel.countRepairs.ToString();
        }
    }

    public void BtnAddFireball()
    {
        if (GameModel.gold >= CostFireball)
        {
            GameModel.gold -= CostFireball;
            GameModel.countFireballs++;

            countGold.text = GameModel.gold.ToString();
            countFireballs.text = GameModel.countFireballs.ToString();
        }
    }

    private void ClosePopup()
    {

//      GameModel.pause = false;

        transform.gameObject.SetActive(false);

    }

}
