using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopPopupView : MonoBehaviour
{

    private const sbyte
        CostIceblast = 25,
        CostFireball = 25,
        CostLightning = 35;

    public TextMeshProUGUI 
        countGold,
        countIceblasts,
        countFireballs,
        countLightnngs;

    public Animator animatorPanelBottom;

    public void OnEnable()
    {

        if (animatorPanelBottom.GetBool("BtnState")) animatorPanelBottom.SetBool("BtnState", false);

    }

    public void BtnClose()
    {
        ClosePopup();
    }

    public void BtnAddIceblast()
    {
        if (GameModel.gold >= CostIceblast)
        {
            GameModel.gold -= CostIceblast;
            GameModel.iceblasts++;

            countGold.text = GameModel.gold.ToString();
            countIceblasts.text = GameModel.iceblasts.ToString();
        }
    }

    public void BtnAddFireball()
    {
        if (GameModel.gold >= CostFireball)
        {
            GameModel.gold -= CostFireball;
            GameModel.fireballs++;

            countGold.text = GameModel.gold.ToString();
            countFireballs.text = GameModel.fireballs.ToString();
        }
    }

    public void BtnAddLightning()
    {
        if (GameModel.gold >= CostLightning)
        {
            GameModel.gold -= CostLightning;
            GameModel.lightnings++;

            countGold.text = GameModel.gold.ToString();
            countLightnngs.text = GameModel.lightnings.ToString();
        }
    }

    private void ClosePopup()
    {

//      GameModel.pause = false;

        transform.gameObject.SetActive(false);

    }

}
