using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer
{
    public int
        damage,
        price,
        effect;

    public float
        speed;
}

public class GameModel
{

    public static bool play;

    public static int
        hp,
        gold,
        points,

        archersCount,

        iceblasts,
        fireballs,
        lightnings;

    public static float speed = 1.0f;

    public static Archer[] archer;

}
