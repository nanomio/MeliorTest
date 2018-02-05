using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class ArcherView : MonoBehaviour
{

    private const float boostTime = 2.2f;

    static public float attackLine = 3.3f;

    static public List<GameObject> attackList;

    public int modelNumber;

    public GameObject arrowPrefab;

    private UnityAction
        NewTargetListener,
        TargetDeathListener,

        GamePauseListener,
        GamePlayListener,
        GameOverListener,
        
        SpeedBoostListener;

    private GameObject target;

    private bool
        active,
        shooting,
        boosted;

    private void Awake()
    {
        NewTargetListener = new UnityAction(NewTarget);
        TargetDeathListener = new UnityAction(TargetDeath);

        GamePauseListener = new UnityAction(Pause);
        GamePlayListener = new UnityAction(Play);
        GameOverListener = new UnityAction(GameOver);

        SpeedBoostListener = new UnityAction(SpeedBoost);
    }

    private void OnEnable()
    {
        EventManager.StartListening("NewTarget", NewTargetListener);
        EventManager.StartListening("TargetDeath", TargetDeathListener);

        EventManager.StartListening("GamePause", GamePauseListener);
        EventManager.StartListening("GamePlay", GamePlayListener);
        EventManager.StartListening("GameOver", GameOverListener);

        EventManager.StartListening("SpeedBoost", SpeedBoostListener);
    }

    private void OnDisable()
    {
        EventManager.StopListening("NewTarget", NewTargetListener);
        EventManager.StopListening("TargetDeath", TargetDeathListener);

        EventManager.StopListening("GamePause", GamePauseListener);
        EventManager.StopListening("GamePlay", GamePlayListener);
        EventManager.StopListening("GameOver", GameOverListener);

        EventManager.StopListening("SpeedBoost", SpeedBoostListener);
    }

    void Start ()
    {
        active = shooting = boosted = false;

        target = null;
        attackList = new List<GameObject>();
	}
	
	void Update ()
    {
		
	}

    public void Activate()
    {

        active = true;

        if (attackList.Count > 0)
        {
            StartCoroutine(Shoot());
        }

    }

    private IEnumerator Shoot()
    {

        shooting = true;
        target = attackList.First();

        while ( target != null )
        {

            GetComponent<Animator>().SetBool("Shoot", true);
            target.gameObject.tag = "Target";

            while (target.GetComponent<EnemyView>().alive)
            {

                if (boosted && GetComponent<Animator>().speed < 2f)
                {
                    GetComponent<Animator>().speed = 2f;

                    Debug.Log("Speed up!");

                    yield return new WaitForEndOfFrame();
                }

                if (!boosted && GetComponent<Animator>().speed > 1f)
                {
                    GetComponent<Animator>().speed = 1f;

                    Debug.Log("Boost ended!");

                    yield return new WaitForEndOfFrame();
                }

                float animTime = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;

                if (float.IsInfinity(animTime))
                {
                    yield return new WaitForEndOfFrame();
                    animTime = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
                }

                yield return new WaitForSeconds(animTime);

                if (!GameModel.play) yield return new WaitUntil(() => GameModel.play);

                if (target == null || !GetComponent<Animator>().GetBool("Shoot")) break;

                GameObject arrow = Instantiate(arrowPrefab, transform.position + Vector3.right * 2f, Quaternion.identity);
                arrow.transform.GetComponent<SpriteRenderer>().sortingOrder = target.transform.GetComponent<SpriteRenderer>().sortingOrder;

                StartCoroutine(ArrowFly(arrow, target.transform));

            }

            attackList.Remove(target);
            target = attackList.Count > 0 ? attackList.First() : null;

        }

        GetComponent<Animator>().SetBool("Shoot", false);
        shooting = false;

    }

    private IEnumerator ArrowFly(GameObject arrow, Transform target)
    {

        float offset = 0f;

        while (Vector3.Distance(arrow.transform.position, target.position) > 3f)
        {
            if (GameModel.play)
            {
                arrow.transform.position = new Vector3(Mathf.Lerp(arrow.transform.position.x, target.position.x, offset), Mathf.Lerp(arrow.transform.position.y, target.position.y, offset));
                arrow.transform.Rotate(0, 0, (target.position.y - arrow.transform.position.y) * 0.5f);

                yield return new WaitForSeconds(0.01f);

                offset += .02f;
            }
            else yield return new WaitUntil(() => GameModel.play);

            if (target == null) break;
        }

        EventManager.TriggerEvent("ArrowHit", modelNumber);
        Destroy(arrow);

    }

    private IEnumerator BoostTimer()
    {

        yield return new WaitForSeconds(boostTime);
        boosted = false;

        if (target == null || !shooting)
        {
            GetComponent<Animator>().speed = 1f;
        }
    }

    private void NewTarget()
    {

        GameObject newTarget = GameObject.FindGameObjectWithTag("New target") as GameObject;

        if (newTarget != null)
        {
            attackList.Add(newTarget);
            newTarget.tag = "Enemy";
        }

        if ( !shooting && active) StartCoroutine(Shoot());

    }

    private void TargetDeath()
    {

        GetComponent<Animator>().SetBool("Shoot", false);
        
    }

    private void SpeedBoost()
    {

        boosted = true;
        StartCoroutine(BoostTimer());

    }

    private void Pause()
    {

        transform.GetComponent<Animator>().speed = 0f;

    }

    private void Play()
    {

        transform.GetComponent<Animator>().speed = boosted ? 2f : 1f;

    }

    private void GameOver()
    {

        transform.gameObject.SetActive(false);

    }

}
