using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class ArcherView : MonoBehaviour
{

    static public float attackLine = 2.0f;

    static public List<GameObject> attackList = new List<GameObject>();

    public int modelNumber;

    public GameObject arrowPrefab;

    private UnityAction
        NewTargetListener,

        GamePauseListener,
        GamePlayListener,
        GameOverListener;

    private bool shooting;

    private void Awake()
    {
        GamePauseListener = GameOverListener = new UnityAction(Pause);
        GamePlayListener = new UnityAction(Play);

        NewTargetListener = new UnityAction(NewTarget);
    }

    private void OnEnable()
    {
        EventManager.StartListening("GamePause", GamePauseListener);
        EventManager.StartListening("GamePlay", GamePlayListener);
        EventManager.StartListening("GameOver", GameOverListener);

        EventManager.StartListening("NewTarget", NewTargetListener);
    }

    private void OnDisable()
    {
        EventManager.StopListening("GamePause", GamePauseListener);
        EventManager.StopListening("GamePlay", GamePlayListener);
        EventManager.StopListening("GameOver", GameOverListener);

        EventManager.StopListening("NewTarget", NewTargetListener);
    }

    void Start ()
    {
        shooting = false;
	}
	
	void Update ()
    {
		
	}

    private IEnumerator Shoot()
    {

        //float animTime = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        shooting = true;

        GetComponent<Animator>().SetBool("Shoot", true);
        GameObject target = attackList.First();

        while ( target != null )
        {

            target.gameObject.tag = "Target";

            while (target.GetComponent<EnemyView>().alive)
            {
                float animTime = GetComponent<Animator>().GetFloat("MyLength");
                if (float.IsInfinity(animTime))
                    animTime = GetComponent<Animator>().GetNextAnimatorStateInfo(0).length;
                Debug.Log("And here we get animation lenth, and it is " + GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
                yield return new WaitForSeconds(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

                if (!GameModel.play)
                {
                    yield return new WaitUntil(() => GameModel.play);
                    Debug.Log("here we go back");
                }

                if (target == null) break;

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

        EventManager.TriggerEvent("ArrowHit", GameModel.archer[modelNumber].damage);
        Destroy(arrow);

    }

    private void NewTarget()
    {

        GameObject newTarget = GameObject.FindGameObjectWithTag("New target") as GameObject;

        if (newTarget != null)
        {
            attackList.Add(newTarget);
            newTarget.tag = "Enemy";
        }

        if ( !shooting && attackList.Count > 0) StartCoroutine(Shoot());

    }

    private void Pause()
    {
        transform.GetComponent<Animator>().speed = 0f;
    }

    private void Play()
    {

        transform.GetComponent<Animator>().speed = 1f;

    }

}
