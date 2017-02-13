using System;
using UnityEngine;
using Random = UnityEngine.Random;      //1Tells Random to use the Unity Engine random number generator.

public class ArcherAI : MonoBehaviour
{
    // public settings here
    public int HP = 10;
    public float keepDistance = 6f; // if player is in this range, then we should walk back
    public float alertDistance = 15f; // if player is greater than this range, do nothing
    public int damage = 4;
    public float speed = 4f;
    private float chaseWalkDelta = 1f;
    public int chanceToAttack = 5;
    public float attackTimeGap = 1f; // time gap between each attack

    //Audio
    public AudioClip[] painSounds;
    public AudioSource audioE;

    // Blood effect
    public GameObject bloodPrefab;

    // private variables starts here
    private bool isDead = false;
    private bool isWalking = false;
    private int walkState = Animator.StringToHash("Base Layer.walk");
    private int aimState = Animator.StringToHash("Base Layer.aim");
    private int idleState = Animator.StringToHash("Base Layer.idle");
    private float lastHitTime;
    private bool hasAttacked;
    private Animator animator;
    private NavMeshAgent2D enemy;
    private Transform player;
    private AnimatorStateInfo currentBaseState;
    private enum AnimationParams
    {
        isWalk, isAim, isIdle
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        lastHitTime = 1f;
        enemy = GetComponent<NavMeshAgent2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy.speed = speed;
        if (audioE == null)
        {
            audioE = GetComponentInChildren<AudioSource>();
        }
        //ApplyAnimationEventToKickAnimation(CreateAnimationEvent());
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }

        if (HP <= 0)
        {
            //do something
            whenEnemyDead();
            return;
        }

        currentBaseState = animator.GetCurrentAnimatorStateInfo(0);
        float distance = Vector2.Distance(transform.position, player.position);

        // player is out of our sight, we stop;
        if (distance > alertDistance)
        {
            enemy.Stop();
            setToThisAnimation(AnimationParams.isIdle);
        }
        else
        {
            setToThisAnimation(AnimationParams.isWalk);
        }

        /**************************************************/
        /* A Simple State Machine Management starts here */
        /************************************************/
        if (currentBaseState.fullPathHash.Equals(aimState))
        {
            //TODO: fire the arrow
            Attack();
        }
        else if (currentBaseState.fullPathHash.Equals(walkState))
        {
            hasAttacked = false;

            // we are at random walk range, check if we have reach the deatination.
            if (isWalking)
            {
                if (enemy.remainingDistance <= 0.01f)
                {
                    isWalking = false;
                }
            }

            if (distance > keepDistance)
            {
                if (!isWalking)
                {
                    //use chance System to determine whether we should attack or not
                    RandomlyChooseAttackOrMove(chanceToAttack, () =>
                    {
                        enemy.Resume();
                        enemy.destination = GetRandomNearPosition();
                        isWalking = true;
                    });
                }
            }
            else
            {
                //use chance System to determine whether we should attack or not
                RandomlyChooseAttackOrMove(chanceToAttack / 2, () =>
                {
                    // should walk back
                    enemy.Resume();
                    enemy.destination = GetFurthestPointAfterPlayerToEnemy();
                });
            }
        }
        //Debug.Log(Vector2.Distance(transform.position, player.position));
    }

    private void RandomlyChooseAttackOrMove(int chance, Action callback)
    {
        int randomNumber = Random.Range(0, 10);
        if (randomNumber <= chance)
        {
            //check the god damn time
            float timeSinceLastHit = Time.time - lastHitTime;
            if (timeSinceLastHit >= attackTimeGap)
            {
                setToThisAnimation(AnimationParams.isAim);
            }
        }
        else
        {
            callback();
        }
    }

    private void Attack()
    {
        if (!hasAttacked)
        {
            enemy.Stop();
            lastHitTime = Time.time;
            PlayerHealth.doDamage(damage, this.transform.position);
            hasAttacked = true;
        }
        setToThisAnimation(AnimationParams.isWalk);
    }

    public void EnemyBeenHit(int incomingDamage)
    {
        HP -= incomingDamage;

        int rand = UnityEngine.Random.Range(0, painSounds.Length);
        audioE.clip = painSounds[rand];
        audioE.Play();

        showSomeBlood(incomingDamage);

        if (HP <= 0)
        {
            whenEnemyDead();
        }
        Debug.Log("asd");
    }

    void whenEnemyDead()
    {
        isDead = true;
        Destroy(gameObject);
    }

    Vector2 GetRandomNearPosition()
    {
        Vector2 newPosition = transform.position;

        Vector2 playerPosition = GetPlayerDirection(player, transform);

        if (playerPosition.x > 0) //player at the right side of enemy
        {
            if (playerPosition.y >= 0) //upper right
            {
                newPosition.x = player.position.x - chaseWalkDelta;
                newPosition.y = player.position.x - chaseWalkDelta;
            }
            else if (playerPosition.y < 0) //down right
            {
                newPosition.x = player.position.x - chaseWalkDelta;
                newPosition.y = player.position.y + chaseWalkDelta;
            }
        }
        else if (playerPosition.x < 0) //player at the left side of enemy
        {
            if (playerPosition.y >= 0) //upper left
            {
                newPosition.x = player.position.x + chaseWalkDelta;
                newPosition.y = player.position.y - chaseWalkDelta;
            }
            else if (playerPosition.y < 0) //down left
            {
                newPosition.x = player.position.x + chaseWalkDelta;
                newPosition.y = player.position.y + chaseWalkDelta;
            }
        }
        else if (playerPosition.x == 0)
        {
            if (playerPosition.y > 0)
            { //player is at vertical top
                newPosition.x -= Random.Range(-1 * chaseWalkDelta, chaseWalkDelta);
                newPosition.y -= chaseWalkDelta;
            }
            else if (playerPosition.y < 0)
            { //player is at vertical down
                newPosition.x -= Random.Range(-1 * chaseWalkDelta, chaseWalkDelta);
                newPosition.y += chaseWalkDelta;
            }
        }

        return newPosition;
    }

    void setToThisAnimation(AnimationParams type)
    {
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

        // set the direction of the animationClips
        Vector2 pos = GetPlayerDirection(player, transform);
        animator.SetFloat("moveX", pos.x);
        animator.SetFloat("moveY", pos.y);
    }

    private AnimationEvent CreateAnimationEvent()
    {
        // new event created
        return new AnimationEvent()
        {
            time = 0.06f,
            functionName = "KickPlayer"
        };
    }

    private void ApplyAnimationEventToKickAnimation(AnimationEvent evt)
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            string name = clip.name;
            if (name.StartsWith("Kick") || name.StartsWith("sword-slash"))
            {
                bool isAdded = false;
                foreach (AnimationEvent e in clip.events)
                {
                    if (e.functionName == evt.functionName)
                    {
                        isAdded = true;
                    }
                }
                if (!isAdded)
                {
                    clip.AddEvent(evt);
                }
            }
        }
    }

    private Vector2 GetPlayerDirection(Transform player, Transform enemy)
    {
        Transform transform = enemy;
        float horizontal = player.position.x - transform.position.x;
        float vertical = player.position.y - transform.position.y;

        Vector2 pos = new Vector2(0, 0);
        float offset = 0.7f; //use to make the enemy not that sensetive to direction

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

        // if (enemyHP <= runAwayHP)
        // {
        //     pos.x *= -1;
        //     pos.y *= -1;
        // }

        return pos;
    }

    void showSomeBlood(int incomingdamage)
    {
        GameObject blood = Instantiate(bloodPrefab);
        // set blood position
        Vector3 bloodPos = this.transform.position;
        blood.transform.position = bloodPos;
        // set blood direction
        float playerAngle = player.gameObject.GetComponent<CharacterController>().getPlayerAngle();
        blood.GetComponent<BloodScript>().setBlood(playerAngle, (float)incomingdamage / 2f);
        // set blood damage text
        blood.GetComponentInChildren<damageTextScr>().setDamage(incomingdamage);
    }

    Vector2 GetFurthestPointAfterPlayerToEnemy()
    {
        Vector2 playerPosition = GetPlayerDirection(player, transform);
        Vector2 newPosition = transform.position;

        float moveX = 1.1f; // delta value to move
        float moveY = 1.1f; // delta value to move

        if (playerPosition.x > 0) //player at the right side of enemy
        {
            if (playerPosition.y >= 0) //upper right
            {
                newPosition.x -= moveX;
                newPosition.y -= moveY;
            }
            else if (playerPosition.y < 0) //down right
            {
                newPosition.x -= moveX;
                newPosition.y += moveY;
            }
        }
        else if (playerPosition.x < 0) //player at the left side of enemy
        {
            if (playerPosition.y >= 0) //upper left
            {
                newPosition.x += moveX;
                newPosition.y -= moveY;
            }
            else if (playerPosition.y < 0) //down left
            {
                newPosition.x += moveX;
                newPosition.y += moveY;
            }
        }
        else if (playerPosition.x == 0)
        {
            if (playerPosition.y > 0)
            { //player is at vertical top
                newPosition.x += Random.Range(-1 * moveX, moveX);
                newPosition.y -= moveY;
            }
            else if (playerPosition.y < 0)
            { //player is at vertical down
                newPosition.x += Random.Range(-1 * moveX, moveX);
                newPosition.y += moveY;
            }
        }

        return newPosition;
    }
}
