using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackCollision : MonoBehaviour {

    private bool playerAttacking;
    private bool Attack;
    public static bool hasHit;


    void OnTriggerEnter2D(Collider2D other)
    {
        Attack = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Attack = false;
    }

    // Update is called once per frame
    void Update()
    {

        hasHit = false;

        playerAttacking = CharacterController.attackString == "PlayerKicking";

        if (Attack && playerAttacking)
        {
            Attack = true;
        }
    }
}
