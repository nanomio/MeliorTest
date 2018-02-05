using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creep
{
    public int eNum { get; set; }
    public float time { get; set; }
    public int yOffset { get; set; }
}

public class GameController : MonoBehaviour
{

    public int startHP;
    public int startGold;

    public GameObject[] prefabEnenmie;

    public List<Creep>[] levelWave;

    protected int countWaves = 0;

    private void Awake()
    {

        GameModel.hp = startHP;
        GameModel.gold = startGold;
        GameModel.points = 0;

        GameModel.countArchers = 0;

        GameModel.countBoosts = GameModel.countRepairs = GameModel.countFireballs = 0;

        GameModel.archer = new Archer[]
        {
            new Archer() { damage = 20, speed = 1f,   price = 50,  effect = 0 },
            new Archer() { damage = 25, speed = 1.1f, price = 75,  effect = 1 },
            new Archer() { damage = 35, speed = 1.5f, price = 150, effect = 2 }
        };

        GameModel.play = true;
        GameModel.waiting = true;

        countWaves = 0;

        levelWave = new List<Creep>[2];

        levelWave[0] = new List<Creep>
        {
            new Creep() { eNum = 1, time = 2f, yOffset = 120 },
            new Creep() { eNum = 2, time = 4f, yOffset = 250 },
            new Creep() { eNum = 0, time = 2f, yOffset = 100 },
            new Creep() { eNum = 1, time = 0f, yOffset = 50 }
        };

        levelWave[1] = new List<Creep>
        {
            new Creep() { eNum = 0, time = 1f },
            new Creep() { eNum = 0, time = 1f },
            new Creep() { eNum = 0, time = 1f },
            new Creep() { eNum = 0, time = 1f }
        };

        int countHP = 0;
        foreach (Creep dummy in levelWave[0])
            countHP += prefabEnenmie[dummy.eNum].GetComponent<EnemyView>().hp;

        GameModel.waveProgress = GameModel.waveHP = countHP;

    }

    void Start()
    {
        StartCoroutine(WaveOn());
    }

    void Update ()
    {
        
	}

    private IEnumerator WaveOn()
    {
        float timer = 0;
        GameObject tmp = null;

        EventManager.TriggerEvent("WavePopup");

        while (timer < GameModel.waveWaitTime && GameModel.waiting)
        {
            yield return new WaitForSeconds(GameModel.dT);
            timer += GameModel.dT;
            if (!GameModel.play) yield return new WaitUntil(() => GameModel.play);
        }

        foreach (Creep creep in levelWave[countWaves])
        {
            int lOrder = 20 + (300 - creep.yOffset) / 15;

            float artWidth = prefabEnenmie[creep.eNum].transform.GetComponent<SpriteRenderer>().sprite.bounds.size.x;

            Vector3 pos = new Vector3( Screen.width, 50f + creep.yOffset );
            pos = Camera.main.ScreenToWorldPoint(pos);
            pos = new Vector3(pos.x + artWidth / 2f, pos.y);

            tmp = Instantiate(prefabEnenmie[creep.eNum], pos, Quaternion.identity);
            tmp.transform.GetComponent<SpriteRenderer>().sortingOrder = lOrder;

            timer = 0;
            while (timer < creep.time)
            {
                yield return new WaitForSeconds(GameModel.dT);
                timer += GameModel.dT;
                if (!GameModel.play) yield return new WaitUntil(() => GameModel.play);
            }
        }

        GameModel.waiting = true;
    }

}
