using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MagicPanelView : MonoBehaviour
{

    private const float
        cooldownBoostTime = 4.0f;

    public TextMeshProUGUI
        countBoosts,
        countRepairs,
        countFireballs;

    private bool boostAvailable = true;

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public void BtnRepair()
    {
        if (GameModel.countRepairs > 0)
        {

            GameModel.countRepairs--;
            countRepairs.text = GameModel.countRepairs.ToString();

            EventManager.TriggerEvent("FortDamaged", -250);

        }
    }

    public void BtnSpeedBoost()
    {

        if (boostAvailable && GameModel.countBoosts > 0)
        {

            GameModel.countBoosts--;
            countBoosts.text = GameModel.countBoosts.ToString();

            EventManager.TriggerEvent("SpeedBoost");

            boostAvailable = false;
            StartCoroutine(CooldownBoost());

        }

    }

    private IEnumerator CooldownBoost()
    {

        yield return new WaitForSeconds(cooldownBoostTime);

        boostAvailable = true;

    }

}