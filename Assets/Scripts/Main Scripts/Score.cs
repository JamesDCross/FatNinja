using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {
    private static float lastAttack;
    private static int chainMultipler = 1;
    private static int score;

	// Update is called once per frame
	void Start () {
        lastAttack = 0;
        score = 0;
    }

    public static void calcScore(string attack)
    {
        Debug.Log(attack);
        if (attack == "UpperCut" || attack == "HurricaneKick")
        {
            chainMultipler += 1;
            lastAttack = Time.time;
        }

        if (lastAttack + 3f < Time.time)
        {
            Debug.Log("reste score");
            chainMultipler = 1;
        }

        int add = 100 * chainMultipler;
        score += add;
        Debug.Log(chainMultipler);
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
