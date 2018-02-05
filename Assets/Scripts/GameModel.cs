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

    public const float
        waveWaitTime = 10f,
        dT = .1f;

    public static bool
        play,
        waiting;

    public static int
        hp,
        gold,
        points,

        countArchers,
        waveHP,
        waveProgress,

        countBoosts,
        countRepairs,
        countFireballs;

    public static float
        speed = 1.0f;

    public static Archer[] archer;

}
