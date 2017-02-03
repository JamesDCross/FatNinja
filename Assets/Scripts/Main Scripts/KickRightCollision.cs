using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickRightCollision : MonoBehaviour
{

    public float lastMoveX;
    public float lastMoveY;
    public bool playerKicking;
    public bool hasHit;

    public bool KickRight;
    public bool KickBackRight;

    void OnTriggerEnter2D(Collider2D other)
    {
        hasHit = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        hasHit = false;
    }

    // Update is called once per frame
    void Update()
    {

        KickRight = false;

        lastMoveX = this.transform.parent.GetComponent<CharacterController>().lastMove.x;
        lastMoveY = this.transform.parent.GetComponent<CharacterController>().lastMove.y;

        playerKicking = CharacterController.attackString == "PlayerKicking";

        if (hasHit)
        { 
            if (playerKicking == true)
            {
                KickRight = true;
            }
        }
    }
}
