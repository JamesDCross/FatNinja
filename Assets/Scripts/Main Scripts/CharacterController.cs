using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;


public class CharacterController : MonoBehaviour {

    private Animator anim;
    private Rigidbody2D myRigidbody;
    public float speed = 1f;
    public bool playerMoving = false;
    public bool playerAttacking = false;
    public static string attackString = "";
    public Vector2 lastMove;

    public static float lastHitTime;
    public static float timeSinceLastHit;
    public static bool comboTiming;
    bool hasAlreadyAttacked = false;
    bool pressed = false;
    public static List<string> comboTracker = new List<string>();
    PlayerAction characterActions;

    private GameObject enemy;
    public BoxCollider2D ShibosAttack;

    void Start() {
        anim = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();

        lastHitTime = 1f;

        characterActions = new PlayerAction();

        characterActions.Punch.AddDefaultBinding(InputControlType.Action1);
        characterActions.Punch.AddDefaultBinding(Key.Z);

        characterActions.Kick.AddDefaultBinding(InputControlType.Action2);
        characterActions.Kick.AddDefaultBinding(Key.X);

        characterActions.Blank.AddDefaultBinding(InputControlType.Action3);
        characterActions.Blank.AddDefaultBinding(Key.C);

        characterActions.Roll.AddDefaultBinding(InputControlType.Action4);
        characterActions.Roll.AddDefaultBinding(Key.V);

        enemy = null;
        ShibosAttack.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (attackString != "" && enemy == null && other.GetType() == typeof(BoxCollider2D))
        {
            if (other.tag == "Enemy")
            {
                enemy = other.gameObject;
                Debug.Log("Lunched Attack");
            }
        }
    }

    void Update() {
        if (!GameMaster.pause)
        {
            timeSinceLastHit = Time.time - lastHitTime;
            PerformMovement();
        }
    }

    void PerformMovement()
    {
        playerMoving = Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0 || playerAttacking ? false : true;

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerKicking") && timeSinceLastHit >= .3f)
        {
            playerAttacking = false;
        }
        else
        {
            myRigidbody.velocity = new Vector2(0f, 0f);
        }

        bool setH = Input.GetAxisRaw("Horizontal") > 0.1 || Input.GetAxisRaw("Horizontal") < -0.1;
        bool setV = Input.GetAxisRaw("Vertical") > 0.1 || Input.GetAxisRaw("Vertical") < -0.1;
        int h = 0;
        int v = 0;
        if (setH)
            h = Input.GetAxisRaw("Horizontal") > 0 ? 1 : -1;
        if (setV)
            v = Input.GetAxisRaw("Vertical") > 0 ? 1 : -1;

        if (!playerAttacking)
        {
            //--movement
            if (h == 1 || h == -1)
            {
                myRigidbody.velocity = new Vector2(h * speed, myRigidbody.velocity.y);
                lastMove = new Vector2(h, v);
            }
            if (v == 1 || v == -1)
            {
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, v * speed);
                lastMove = new Vector2(h, v);
            }

            //stops shibo rigidbody sliding
            if (Input.GetAxisRaw("Horizontal") < 0.1f && Input.GetAxisRaw("Horizontal") > -0.1f)
            {
                myRigidbody.velocity = new Vector2(0f, myRigidbody.velocity.y);
            }
            if (Input.GetAxisRaw("Vertical") < 0.1f && Input.GetAxisRaw("Vertical") > -0.1f)
            {
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, 0f);
            }
            Action();
        }
        anim.SetFloat("MoveX", h);
        anim.SetFloat("MoveY", v);

        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);

        anim.SetBool("PlayerMoving", playerMoving);
        if (playerAttacking && attackString != "")
        {
            anim.SetBool(attackString, playerAttacking);
            //ShibosAttack.enabled = true;
            ShibosAttack.size = new Vector2(1.6f, .29f);
            if (enemy != null)
            {
                EnemyAI test = enemy.GetComponent<EnemyAI>();
                test.EnemyBeenHit(2);
                Debug.Log("Recived Attack");
                enemy = null;
            }
        }
        else if (attackString != "")
        {
            anim.SetBool(attackString, playerAttacking);
            ShibosAttack.enabled = false;
            //attackString = "";
            ShibosAttack.size = new Vector2(0f, 0f);
        }
    }

    void Action() {

        comboTiming =  timeSinceLastHit <= .8f;

        if (comboTracker.Count >= 5 || timeSinceLastHit > .8f)
        {
            comboReset();
        }        

        if (characterActions.Kick || characterActions.Punch || characterActions.Roll || characterActions.Blank)
        {
            if (!pressed && timeSinceLastHit >= 0.3f || !hasAlreadyAttacked)
            {
                pressed = true;
                playerAttacking = true;
                hasAlreadyAttacked = true;
                if (characterActions.Punch)
                {
                    //attackString = "PlayerPunching";
                    lastHitTime = Time.time;
                    comboTracker.Add("Punch");
                } else if (characterActions.Kick)
                {
                    attackString = "PlayerKicking";
                    lastHitTime = Time.time;
                    comboTracker.Add("Kick");
                } else if (characterActions.Blank)
                {
                    lastHitTime = Time.time;
                    comboTracker.Add("Blank");
                } else if (characterActions.Roll)
                {
                    if (comboTracker.Count == 4)
                    {
                        comboTracker.Add("Roll");
                    } else
                    {
                        roll();
                        playerAttacking = false;
                    }
                } 
            }
        } 
        else
        {
            pressed = false;
        }
    }

    void roll()
    {

    }

    void showCollider()
    {
        ShibosAttack.enabled = true;
    }

    public static void comboReset()
    {
        comboTiming = false;
        comboTracker = new List<string>();
    }

    void DestroyActions()
    {
        characterActions.Destroy();
    }
}
