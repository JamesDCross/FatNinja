using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
this script should be on the enemyhitcollider object

*/
public class EnemyController : MonoBehaviour {

    public static Animator anim;
    public bool hasHit;
    public bool PlayerKick;
    public GameObject playerkickRight;
    public SpriteRenderer PlayerspriteRend;
    public AudioSource audio;

    public GameObject player;
    //enemy follow player


    void OnTriggerEnter2D(Collider2D other)
    {

        hasHit = true;
        //other.gameObject.GetComponent<EnemyAI>().EnemyBeenHit(8);

    }

    void OnTriggerExit2D(Collider2D other)
    {
        hasHit = false;
    }

    public void playHit()
    {
        audio.Play();
    }


    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //bool sent from the player saying he is kicking right
        PlayerKick = CharacterController.attackString == "PlayerKicking";


        if (PlayerKick)
        {
            //other.gameObject.GetComponent<EnemyAI>().EnemyBeenHit(2);

            //Debug.Log("player kickrighttrue");
        }

        if (hasHit)//the player
        {
            //if the 0026 frame of the anim is being played(this is way easier than getting frame out of blend tree)
            //StartCoroutine("MyMethod");
            //StartCoroutine(MyMethod());


            //play animation based on players kicking bool

            //if the enemy is facing down
            anim.SetBool("HitFromRight", PlayerKick);

        }
    }
}