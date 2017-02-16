using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;      //1Tells Random to use the Unity Engine random number generator.

public class SumoAI : MonoBehaviour
{
    // public settings here
    public int HP = 10;
    public float timeBeforeChase = 3f;
    public int damage = 4;
    public float speed = 4f;

    //Audio
    public AudioClip[] painSounds;
    public AudioSource audioE;

    // Blood effect
    public GameObject bloodPrefab;

    // private variables starts here
    private bool isDead = false;
    private bool isChasing = false;
    private bool isTired = false;
    private GameObject playerCollider;
    private Vector2 playerPosition;
    private int walkState = Animator.StringToHash("Base Layer.walk");
    private int roarState = Animator.StringToHash("Base Layer.roar");
    private int tiredState = Animator.StringToHash("Base Layer.tired");
    private int punchState = Animator.StringToHash("Base Layer.punch");
    private int idleState = Animator.StringToHash("Base Layer.flex");
    private int beenHitState = Animator.StringToHash("Base Layer.beenHit");
    private bool hasAttacked;
    private Animator animator;
    private float startTiredTime;
    private NavMeshAgent2D enemy;
    private Transform player;
    private AnimatorStateInfo currentBaseState;
    private enum AnimationParams
    {
        isWalk, isPunch, isHit, isIdle, isTired, isRoar
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<NavMeshAgent2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy.speed = speed;
        if (audioE == null)
        {
            audioE = GetComponentInChildren<AudioSource>();
        }
    }

    void Start()
    {
        //ApplyAnimationEventToAnimation(CreateAnimationEvent(4f, "StartToChase"), "st");
    }

    void Update()
    {
        if (HP <= 0)
        {
            Loading.loadLevel("finalQTE");
        }
        Debug.Log(playerPosition);
        currentBaseState = animator.GetCurrentAnimatorStateInfo(0);
        float distance = Vector2.Distance(transform.position, playerPosition);

        /**************************************************/
        /* A Simple State Machine Management starts here */
        /************************************************/
        if (currentBaseState.fullPathHash.Equals(walkState))
        {
            hasAttacked = false;
            if (isChasing)
            {
                enemy.Resume();
                //enemy.destination = playerPosition;
                setEnemyDirection();
                isChasing = false;
            }
            // I have reached the previous player position or I have catched the player
            //if (distance.Equals(0))
            if (enemy.remainingDistance.Equals(0))
            {
                setToThisAnimation(AnimationParams.isPunch);
            }
        }
        else if (currentBaseState.fullPathHash.Equals(punchState))
        {
            if (!hasAttacked)
            {
                hasAttacked = true;
                enemy.Stop();
                float pDistance = Vector2.Distance(transform.position, player.position);

                if (pDistance <= 0.5f)
                {
                    setEnemyDirection();
                    PlayerHealth.doDamage(damage, this.transform.position);
                    playerCollider = null;
                }
            }
            setToThisAnimation(AnimationParams.isTired);
        }
        else if (currentBaseState.fullPathHash.Equals(tiredState))
        {
            isTired = true;
            StartCoroutine(StartToChase());
        }
        else if (currentBaseState.fullPathHash.Equals(beenHitState))
        {
            // anything related to the beenHit state should locates here.
            animator.SetBool("isHit", false);
            //setToThisAnimation(AnimationParams.isRoar);
        }
        else if (currentBaseState.fullPathHash.Equals(roarState))
        {
            //StartCoroutine(StartToChase());            
        }
        else if (currentBaseState.fullPathHash.Equals(idleState))
        {
            StartCoroutine(StartToChase());
        }
    }

    IEnumerator StartToChase()
    {
        yield return new WaitForSeconds(timeBeforeChase);
        isChasing = true;
        isTired = false;
        playerPosition = player.position;
        setToThisAnimation(AnimationParams.isWalk);
        setEnemyDirection();
        enemy.destination = playerPosition;
    }

    // private void StartToChase()
    // {
    //     playerPosition = new Vector2(player.position.x, player.position.y);

    //     enemy.Resume();
    //     enemy.destination = playerPosition;
    //     setToThisAnimation(AnimationParams.isWalk);
    //     setEnemyDirection();
    // }

    public void EnemyBeenHit(int incomingDamage)
    {
        //if (currentBaseState.Equals(tiredState))
        if (isTired)
        {
            HP -= incomingDamage;

            // int rand = UnityEngine.Random.Range(0, painSounds.Length);
            // audioE.clip = painSounds[rand];
            // audioE.Play();
            Debug.Log("here");
            showSomeBlood(incomingDamage);

            setToThisAnimation(AnimationParams.isHit);
            setEnemyDirection();
        }
    }

    void setEnemyDirection()
    {
        // set the direction of the animationClips
        Vector2 pos = GetPlayerDirection(player, transform);
        animator.SetFloat("moveX", pos.x);
        animator.SetFloat("moveY", pos.y);
    }

    void setToThisAnimation(AnimationParams type)
    {
        Array values = Enum.GetValues(typeof(AnimationParams));
        foreach (AnimationParams val in values)
        {
            string name = Enum.GetName(typeof(AnimationParams), val);
            if (val.Equals(type)) { animator.SetBool(name, true); }
            else { animator.SetBool(name, false); }
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

        float moveX = 1f; // delta value to move
        float moveY = 1f; // delta value to move

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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            playerCollider = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            playerCollider = null;
        }
    }

    private AnimationEvent CreateAnimationEvent(float stime, string eventName)
    {
        // new event created
        return new AnimationEvent()
        {
            time = stime,
            functionName = eventName
        };
    }

    private void ApplyAnimationEventToAnimation(AnimationEvent evt, String animationName)
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            string name = clip.name;
            if (name.StartsWith(animationName))
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
}
