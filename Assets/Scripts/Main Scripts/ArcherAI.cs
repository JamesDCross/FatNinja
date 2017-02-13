using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;      //1Tells Random to use the Unity Engine random number generator.

public class ArcherAI : MonoBehaviour
{
    // public settings here
    public int HP = 10;
    public float keepDistance = 6f; // if player is in this range, then we should walk back
    public float alertDistance = 15f; // if player is greater than this range, do nothing
    public int damange = 4;
    public float speed = 4f;
    public float randomWalkRange = 3f;
    public int chanceToAttack = 5;
    public float attackTimeGap = 1f; // time gap between each attack

    // private variables starts here
    private bool isDead = false;
    private bool isRandomWalk = false;
    private int walkState = Animator.StringToHash("Base Layer.isWalk");
    private int aimState = Animator.StringToHash("Base Layer.isAim");
    private int idleState = Animator.StringToHash("Base Layer.isIdle");
    private float lastHitTime;
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

        /* A Simple State Machine Management starts here */
        if (currentBaseState.fullPathHash.Equals(aimState))
        {
            //fire the arrow
            enemy.Stop();
        }
        else if (currentBaseState.fullPathHash.Equals(walkState))
        {
            // we are at random walk range, check if we have reach the deatination.
            if (isRandomWalk)
            {
                float rDistance = Vector2.Distance(transform.position, enemy.destination);
                if (rDistance <= 0.2f)
                {
                    isRandomWalk = false;
                }
            }

            if (distance > keepDistance)
            {
                if (!isRandomWalk)
                {
                    //use chance System to determine whether we should attack or not
                    int randomNumber = Random.Range(0, 10);

                    if (randomNumber <= chanceToAttack)
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
                        enemy.Resume();
                        enemy.destination = GetRandomNearPosition();
                        isRandomWalk = true;
                    }
                }
            }
            else
            {
                // should walk back
            }
        }
        //Debug.Log(Vector2.Distance(transform.position, player.position));
    }

    void whenEnemyDead()
    {
        isDead = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {

    }

    void OnTriggerExit2D(Collider2D other)
    {

    }

    Vector2 GetRandomNearPosition()
    {
        Vector2 enemyPos = transform.position;

        enemyPos.x += Random.Range(-1 * randomWalkRange, randomWalkRange);
        enemyPos.y += Random.Range(-1 * randomWalkRange, randomWalkRange);

        return enemyPos;
    }

    void setToThisAnimation(AnimationParams type)
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

        // set the direction of the animationClips
        Vector2 pos = EnemyUtils.GetPlayerDirection(player,transform);
        animator.SetFloat("MoveX", pos.x);
        animator.SetFloat("MoveY", pos.y);
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
}
