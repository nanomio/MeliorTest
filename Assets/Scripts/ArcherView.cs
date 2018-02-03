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

    private UnityAction newTargetListener;

    private bool shooting;

    private void Awake()
    {
        newTargetListener = new UnityAction(NewTarget);
    }

    private void OnEnable()
    {
        EventManager.StartListening("NewTarget", newTargetListener);
    }

    private void OnDisable()
    {
        EventManager.StopListening("NewTarget", newTargetListener);
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

        GetComponent<Animator>().SetBool("Shoot", true);
        shooting = true;

        GameObject target = attackList.First();

        while ( target != null )
        {

            target.gameObject.tag = "Target";

            while (target.GetComponent<EnemyView>().alive)
            {
                GameObject arrow = Instantiate(arrowPrefab, transform);
                arrow.transform.GetComponent<SpriteRenderer>().sortingOrder = target.transform.GetComponent<SpriteRenderer>().sortingOrder;
                StartCoroutine(ArrowFly(arrow, target.transform));

                yield return new WaitForSeconds(1f * GameModel.archer[modelNumber].speed);
                EventManager.TriggerEvent("ArrowHit", GameModel.archer[modelNumber].damage);

                if (target == null) break;
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
        Vector3 heading = target.position - arrow.transform.position;

        arrow.GetComponent<Rigidbody2D>().velocity = heading / heading.magnitude * 100f;

        while (Vector3.Distance(arrow.transform.position, target.position) > 0.5)
        {
            //arrow.transform.position = new Vector3(Mathf.Lerp(arrow.transform.position.x, target.position.x, offset), Mathf.Lerp(arrow.transform.position.y, target.position.y, offset));
            //arrow.transform.Rotate(Vector3.up);

            yield return new WaitForSeconds(0.01f);

            offset += .02f;
            if (target == null) break;
        }

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

}
