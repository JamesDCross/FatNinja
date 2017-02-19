using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {
    private static float lastAttack;
    private static int chainMultipler = 1;
    private static int score = 0;
    private static int damage;

    public static float timeLength = 1.0f;

    // Update is called once per frame
    void Start () {
        lastAttack = 0;
    }

    public static void calcScore(string attack)
    {
        //if (attack == "UpperCut" || attack == "HurricaneKick")
        {
            chainMultipler += 1;
            lastAttack = Time.time;
        }

        if (lastAttack + timeLength < Time.time)
        {
            chainMultipler = 1;
        }

        score += damage * chainMultipler;
    }

    public static void setDamage(int dam)
    {
        damage = dam;
    }

    public static int getDamage()
    {
        return damage;
    }

    public static int getMultipler()
    {
        return chainMultipler;
    }
    
    public static void resetMultipler()
    {
        chainMultipler = 1;
    }

    public static int getScore()
    {
        return score;
    }

    public static void scoreReset()
    {
        score = 0;
    }
}
