using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave
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

    public List<Wave>[] levelTimeline;

    protected int countWaves = 0;

    private void Awake()
    {

        GameModel.hp = startHP;
        GameModel.gold = startGold;
        GameModel.points = 0;
        GameModel.archersCount = 0;

        GameModel.iceblasts = GameModel.fireballs = GameModel.lightnings = 0;

        GameModel.archer = new Archer[]
        {
            new Archer() { damage = 20, speed = 1f,   price = 50,  effect = 0 },
            new Archer() { damage = 25, speed = 1.1f, price = 75,  effect = 1 },
            new Archer() { damage = 35, speed = 1.5f, price = 150, effect = 2 }
        };

        GameModel.play = true;

        countWaves = 0;

        levelTimeline = new List<Wave>[2];

        levelTimeline[0] = new List<Wave>
        {
            new Wave() { eNum = 1, time = 9f, yOffset = 120 },
            new Wave() { eNum = 2, time = 2f, yOffset = 250 },
            new Wave() { eNum = 0, time = 4f, yOffset = 100 },
            new Wave() { eNum = 1, time = 0f, yOffset = 50 }
        };

        levelTimeline[1] = new List<Wave>
        {
            new Wave() { eNum = 0, time = 1f },
            new Wave() { eNum = 0, time = 1f },
            new Wave() { eNum = 0, time = 1f },
            new Wave() { eNum = 0, time = 1f }
        };

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
        if (countWaves >= levelTimeline.Length)
            yield return null;

//      countWaves++;

        foreach (Wave creep in levelTimeline[countWaves])
        {
            int lOrder = 20 + (300 - creep.yOffset) / 15;

            float artWidth = prefabEnenmie[creep.eNum].transform.GetComponent<SpriteRenderer>().sprite.bounds.size.x;

            Vector3 pos = new Vector3( Screen.width, 50f + creep.yOffset );
            pos = Camera.main.ScreenToWorldPoint(pos);
            pos = new Vector3(pos.x + artWidth / 2f, pos.y);

            yield return new WaitForSeconds(creep.time);

            GameObject tmp = Instantiate(prefabEnenmie[creep.eNum], pos, Quaternion.identity);
            tmp.transform.GetComponent<SpriteRenderer>().sortingOrder = lOrder;
        }

    }

}
