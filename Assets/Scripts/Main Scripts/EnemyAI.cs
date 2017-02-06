using System;
using System.Collections;
using System.Reflection;
using System.ComponentModel.Design;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;      //1Tells Random to use the Unity Engine random number generator.

public class EnemyAI : MonoBehaviour
{
    public int enemyHP = 10;
    public int damage = 2;
    public float enemySpeed = 2f;
    //public AudioSource hurtMeSound;
    public int runAwayHP = 3;
    public float randomWalkRange = 1f; //when enemy do a random action selection near player, how far should he go.
    public float checkDistance = 1.5f; // The distance between enemy and player, when real distance is smaller, enemy will start to walk.
    public int chanceToAttack = 4; // min 0, max 10;

    //Audio
    public AudioClip[] painSounds;
    public AudioSource audio;

    // Blood effect
    public GameObject bloodPrefab;

    public static float lastHitTime;
    public static float timeSinceLastHit;

    private bool caughtPlayer = false;
    private bool attacking = false;
    private Animator animator;
    private Transform player;
    private NavMeshAgent2D enemy;
    private bool BeenHit = false;

    private bool isRandomMove;
    private Vector2 RandomDestination;
    private Transform playerCollider;
    private bool[] formerStatus; //1-move; 2-kick;
    private Vector3 playerLastPosition;

    private enum AnimationParams
    {
        PlayerMoving,
        EnemyWalking,
        PlayerKicking,
        hitme
    }

    void Start()
    {
        lastHitTime = 1f;
        animator = GetComponent<Animator>();
        enemy = GetComponent<NavMeshAgent2D>();
        animator.SetBool("PlayerKicking", false);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        formerStatus = new bool[3];
        playerCollider = null;
        //enemy.destination = player.position;
    }

    private void ApplyAnimationEventToKickAnimation(AnimationEvent evt)
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name.StartsWith("W"))
            {
                clip.AddEvent(evt);
            }
        }
    }

    public void EnemyRestoreFromHit()
    {
        animator.SetBool("PlayerMoving", formerStatus[0]);
        animator.SetBool("PlayerKicking", formerStatus[1]);
        animator.SetBool("EnemyWalking", formerStatus[2]);
        animator.SetBool("hitme", false);
        BeenHit = false;
    }

    public void EnemyBeenHit(int damage)
    {
        BeenHit = true;
        //hurtMeSound.Play();
        enemyHP -= damage;
        SetEnemyAnimation(AnimationParams.hitme);
        formerStatus[0] = animator.GetBool("PlayerMoving");
        formerStatus[1] = animator.GetBool("PlayerKicking");
        formerStatus[2] = animator.GetBool("EnemyWalking");

        //BeenHit = false;
        //stop any current action

        //audio
        int rand = UnityEngine.Random.Range(0, painSounds.Length);
        //Debug.Log(rand);
        audio.clip = painSounds[rand];
        audio.Play();

        // spawn blood
        
        //if (damage != 0) {
            GameObject blood = Instantiate(bloodPrefab);
            Vector3 bloodPos = this.transform.position;
            //bloodPos.z = 50;
            blood.transform.position = bloodPos;
            float playerAngle = player.gameObject.GetComponent<CharacterController>().getPlayerAngle();
            blood.GetComponent<BloodScript>().setBlood(playerAngle, (float)damage / 4f);
        //}
        
    }

    Vector2 GetPlayerDirection()
    {
        float horizontal = player.position.x - transform.position.x;
        float vertical = player.position.y - transform.position.y;

        Vector2 pos = new Vector2(0, 0);
        float offset = 0.4f; //use to make the enemy not that sensetive to direction

        if (horizontal > offset)
        {
            pos.x = 1;
        }
        else if (horizontal < offset * -1)
        {
            pos.x = -1;
        }
        else if (horizontal >= offset * -1 && horizontal <= offset)
        {
            pos.x = 0;
        }

        if (vertical > offset)
        {
            pos.y = 1;
        }
        else if (vertical < offset * -1)
        {
            pos.y = -1;
        }
        else if (vertical >= offset * -1 && vertical <= offset)
        {
            pos.y = 0;
        }

        if (enemyHP <= runAwayHP)
        {
            pos.x *= -1;
            pos.y *= -1;
        }

        return pos;
    }

    Vector2 GetDestination(Vector2 playerRawAxis)
    {
        /*
            This method is used to detect the player's facing,
            then return the destination that the enemy should move to.
            The destination == center point of the player's collider2D component
            but slightly move outwards
        */

        Vector2 finalPosition = player.transform.position;

        // player facing up
        if (playerRawAxis.x == 0 && playerRawAxis.y == 1)
        {
            finalPosition.y += 0.5f; // + -> outwards
        }

        // player facing down
        if (playerRawAxis.x == 0 && playerRawAxis.y == -1)
        {
            finalPosition.y -= 0.4f; // - -> outwards
        }

        // player facing right
        if (playerRawAxis.x == 1 && playerRawAxis.y == 0)
        {
            finalPosition.x += 0.4f; // - -> outwards
        }

        // player facing left
        if (playerRawAxis.x == -1 && playerRawAxis.y == 0)
        {
            finalPosition.x -= 0.4f; // - -> outwards
        }

        // =================================

        // player facing up right
        if (playerRawAxis.x > 0 && playerRawAxis.x <= 1 &&
      playerRawAxis.y > 0 && playerRawAxis.y <= 1)
        {
            finalPosition.x += 0.27f; // + -> outwards
            finalPosition.y += 0.4f; // + -> outwards
        }

        // player facing up left
        if (playerRawAxis.x < 0 && playerRawAxis.x >= -1 &&
      playerRawAxis.y > 0 && playerRawAxis.y <= 1)
        {
            finalPosition.x += 0.35f; // - -> outwards
            finalPosition.y -= 0.4f; // + -> outwards
        }

        // player facing down right
        if (playerRawAxis.x > 0 && playerRawAxis.x <= 1 &&
      playerRawAxis.y < 0 && playerRawAxis.y >= -1)
        {
            finalPosition.x += 0.32f; // + -> outwards
            finalPosition.y -= 0.3f;  // + -> outwards
        }

        // player facing down left
        if (playerRawAxis.x < 0 && playerRawAxis.x >= -1 &&
      playerRawAxis.y < 0 && playerRawAxis.y >= -1)
        {
            finalPosition.x -= 0.29f; // - -> outwards
            finalPosition.y -= 0.29f; // - -> outwards
        }

        return finalPosition;
    }

    void SetEnemyAnimation(AnimationParams type)
    {
        string targetAnimation = Enum.GetName(typeof(AnimationParams), type);
        Array values = Enum.GetValues(typeof(AnimationParams));
        foreach (AnimationParams val in values)
        {
            string name = Enum.GetName(typeof(AnimationParams), val);
            if (val.Equals(type))
            {
                animator.SetBool(name, true);
            }
            else
            {
                animator.SetBool(name, false);
            }
        }

        Vector2 pos = GetPlayerDirection();
        if (targetAnimation.Equals("PlayerKicking"))
        {
            lastHitTime = Time.time;
            attacking = true;
            animator.SetFloat("LastMoveX", pos.x);
            animator.SetFloat("LastMoveY", pos.y);
        }
        else
        {
            animator.SetFloat("MoveX", pos.x);
            animator.SetFloat("MoveY", pos.y);
        }
    }

    void StartToAttack()
    {
        if (!BeenHit)
        {
            enemy.Stop();
            SetEnemyAnimation(AnimationParams.PlayerKicking);
        }
        else
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("hit"))
            {
                BeenHit = false;
                //animator.SetBool("hitme", false);
                EnemyRestoreFromHit();
            }
        }
    }

    private float AwayFromPlayerVertically(float playerY, float moveY)
    {
        float newY = 0f;
        if (playerY > 0)
        {
            newY -= moveY;
        }
        else if (playerY < 0)
        {
            newY += moveY;
        }
        return newY;
    }

    Vector2 GetFurthestPointAfterPlayerToEnemy()
    {
        Vector2 playerPosition = GetPlayerDirection();
        Vector2 newPosition = transform.position;

        float moveX = 1.5f; // delta value to move
        float moveY = 1.5f; // delta value to move

        if (playerPosition.x > 0) //player at the right side of enemy
        {
            newPosition.x -= moveX;
        }
        else if (playerPosition.x < 0) //player at the left side of enemy
        {
            newPosition.x += moveX;
        }
        newPosition.y = AwayFromPlayerVertically(playerPosition.y, moveY);

        return newPosition;
    }

    Vector2 GetRandomNearPlayerPosition()
    {
        Vector2 playerPos = GetPlayerDirection();
        Vector2 oldEnemyPos = transform.position;
        Vector2 newEnemyPos = new Vector2(0, 0);

        float offSet = randomWalkRange;

        if (playerPos.x >= 0) //player is at right
        {
            oldEnemyPos.x += -1 * Random.Range(0, offSet);
            oldEnemyPos.y += Random.Range(-1 * offSet, offSet);
        }
        else if (playerPos.x < 0) //player is at left
        {
            oldEnemyPos.x += Random.Range(0, offSet);
            oldEnemyPos.y += Random.Range(-1 * offSet, offSet);
        }

        return oldEnemyPos;
    }

    void EnemyRandomMove()
    {
        int randomNumber = Random.Range(0, 10);
        if (randomNumber >= 0 && randomNumber <= chanceToAttack)
        { //enemy will attack;
            if (playerCollider != null)
            {
                //player is at attacking range
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerKicking") && !attacking)
                {
                    animator.SetBool("PlayerKicking", false);
                }
                if (!attacking && timeSinceLastHit >= 1)
                //if (!attacking)
                {
                    StartToAttack();
                }
            }
        }
        else if (randomNumber > chanceToAttack && randomNumber <= 10)
        { //enemy will moveBack
            enemy.Resume();
            SetEnemyAnimation(AnimationParams.EnemyWalking);
            enemy.destination = GetRandomNearPlayerPosition();

            isRandomMove = true;
            RandomDestination = enemy.destination;
        }
    }

    public void KickPlayer()
    {
        AnimationEvent ae = new AnimationEvent();
        ae.messageOptions = SendMessageOptions.DontRequireReceiver;
        PlayerHealth.doDamage(damage);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.x - playerLastPosition.x > 1 ||
            player.position.y - playerLastPosition.y > 1)
        {
            isRandomMove = false;
            enemy.destination = player.position;
        }

        if (isRandomMove)
        {
            if (enemy.remainingDistance <= 0.3f)
            {
                isRandomMove = false;
            }
            else
            {
                enemy.destination = RandomDestination;
                return;
            }
        }

        //Get's the time snice the emeny last made an attack
        timeSinceLastHit = Time.time - lastHitTime;

        //Makese the kick animation == to false after .3 secounds after it was set to true
        //TODO: Why previous one is 1f?
        if (attacking && timeSinceLastHit >= .3)
        //if (attacking)
        {
            attacking = false;
            //PlayerHealth.doDamage(2);
        }


        if (!BeenHit)
        {
            float remainingDistance = Vector2.Distance(transform.position, player.position);
            //EnemyRestoreFromHit();
            if (enemyHP <= 0)
            {
                //play a dead animation
                Destroy(gameObject);
            }

            if (enemyHP <= runAwayHP)
            {
                //makes enemy run away
                enemy.Resume();
                SetEnemyAnimation(AnimationParams.EnemyWalking);
                enemy.speed = 0.6f * enemySpeed;
                enemy.destination = GetFurthestPointAfterPlayerToEnemy();
                return;
            }

            if (caughtPlayer)
            {
                EnemyRandomMove();
                return;
            }

            if (remainingDistance > 0)
            {
                enemy.Resume();
                if (remainingDistance > checkDistance)
                {
                    enemy.speed = enemySpeed;
                    SetEnemyAnimation(AnimationParams.PlayerMoving);
                }
                else if (remainingDistance <= checkDistance)
                {
                    //enemy.speed = 0.9f * enemySpeed;
                    enemy.Resume();
                    SetEnemyAnimation(AnimationParams.EnemyWalking);
                }

                if (!isRandomMove)
                {
                    enemy.destination = player.position;
                }
            }
            else if (remainingDistance <= 0)
            { // caught the player
                StartToAttack();
            }
        }

        playerLastPosition = player.position;
    }

    // void OnCollisionEnter2D(Collision2D coll)
    // {
    // }

    // void OnCollisionExit2D(Collision2D coll)
    // {
    // }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!BeenHit)
        {
            if (other.tag == "Player")
            {
                if (enemyHP > runAwayHP)
                {
                    caughtPlayer = true;
                    playerCollider = other.gameObject.transform;
                }
            }
            else if (other.tag == "Enemy")
            {
                Debug.Log("Enemy");
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            caughtPlayer = false;
            playerCollider = null;
            if (BeenHit)
            {
                BeenHit = false;
            }
            enemy.Resume();
        }
        else if (other.tag == "Enemy")
        {
            Debug.Log("Enemy");
        }
    }
}
