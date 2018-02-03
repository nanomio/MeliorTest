using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyView : MonoBehaviour
{

    public bool alive = true;

    public int hp;
    public float speed;
    public int damage;

    public float attackPosition;

    public bool shooter = false;

    public GameObject projectile;

    private GameObject shell = null;

    private UnityAction
        GamePauseListener,
        GamePlayListener,
        GameOverListener;

    private UnityAction<int> ArrowHitListener;

    private bool targetSend = false;

    private void Awake()
    {
        GamePauseListener = GameOverListener = new UnityAction(Pause);
        GamePlayListener = new UnityAction(Play);

        ArrowHitListener = new UnityAction<int>(TakeHit);
    }

    private void OnEnable()
    {
        EventManager.StartListening("GamePause", GamePauseListener);
        EventManager.StartListening("GamePlay", GamePlayListener);
        EventManager.StartListening("GameOver", GameOverListener);

        EventManager.StartListening("ArrowHit", ArrowHitListener);
    }

    private void OnDisable()
    {
        EventManager.StopListening("GamePause", GamePauseListener);
        EventManager.StopListening("GamePlay", GamePlayListener);
        EventManager.StopListening("GameOver", GameOverListener);

        EventManager.StopListening("ArrowHit", ArrowHitListener);
    }

    void Start ()
    {
        StartCoroutine(Move());
	}
	
	void Update ()
    {

    }

    private void TakeHit(int damage)
    {
        if( tag == "Target" )
        {
            hp -= damage;

            if (!transform.GetChild(0).gameObject.activeSelf)
                transform.GetChild(0).gameObject.SetActive(true);

            if (hp <= 0)
                StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        alive = false;
        transform.GetComponent<Animator>().SetBool("Die", true);

        yield return new WaitForSeconds(transform.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length - .1f);

        Destroy(gameObject);
    }

    private IEnumerator Move()
    {
        while (alive && transform.position.x > attackPosition)
        {
            if (GameModel.play)
            {
                transform.Translate(Vector3.left * 0.01f * speed);

                if (transform.position.x <= ArcherView.attackLine && !targetSend)
                {
                    tag = "New target";
                    EventManager.TriggerEvent("NewTarget");

                    targetSend = true;
                }

                yield return new WaitForSeconds(GameModel.speed * 0.01f);
            }
            else yield return new WaitUntil(() => GameModel.play);
        }

        Attack();

        if (shooter)
            StartCoroutine(Shoot());
        else
            StartCoroutine((Hit()));

//      StartCoroutine(Die());
    }

    private void Attack()
    {
        transform.GetComponent<Animator>().SetBool("Attack", true);
    }

    private IEnumerator Hit()
    {
        while (alive)
        {
            if (GameModel.play)
            {
                float waitTime = transform.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;

                yield return new WaitForSeconds(waitTime);
                if (!GameModel.play)
                    yield return new WaitUntil(() => GameModel.play);

                EventManager.TriggerEvent("FortDamaged", damage);
            }
            else yield return new WaitUntil(() => GameModel.play);
        }
    }

    private IEnumerator Shoot()
    {
        while (alive)
        {
            if (GameModel.play)
            {
                shell = Instantiate(projectile, transform);
                shell.transform.GetComponent<SpriteRenderer>().sortingOrder = transform.GetComponent<SpriteRenderer>().sortingOrder;
                shell.GetComponent<Rigidbody2D>().AddForce(new Vector2(-44f, 15f));

                yield return new WaitForSeconds(shell.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
                if (!GameModel.play)
                    yield return new WaitUntil(() => GameModel.play);

                EventManager.TriggerEvent("FortDamaged", damage);
                Destroy(shell);
            }
            else yield return new WaitUntil(() => GameModel.play);
        }
    }

    private void Pause()
    {
        transform.GetComponent<Animator>().speed = 0f;
        if (shell != null)
        {
            shell.transform.GetComponent<Rigidbody2D>().Sleep();
            shell.transform.GetComponent<Animator>().speed = 0f;
        }
    }

    private void Play()
    {
        transform.GetComponent<Animator>().speed = 1f;
        if (shell != null)
        {
            shell.transform.GetComponent<Rigidbody2D>().WakeUp();
            shell.transform.GetComponent<Animator>().speed = 1f;
        }
    }

}
