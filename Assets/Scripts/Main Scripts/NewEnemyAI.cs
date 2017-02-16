// using System;
// using System.Collections;
// using UnityEngine;
// using Random = UnityEngine.Random;      //1Tells Random to use the Unity Engine random number generator.

// public class NewEnemyAI : MonoBehaviour
// {
//     public GameObject enemyPrefab;
//     public bool isDead = false;
//     public bool isSit = false;
//     public float sitTime = 2f;
//     public int enemyHP = 10;
//     public int damage = 2;
//     public float enemySpeed = 2f;
//     //public AudioSource hurtMeSound;
//     public int runAwayHP = 3;
//     public float randomWalkRange = 1f; //when enemy do a random action selection near player, how far should he go.
//     public float checkDistance = 1.5f; // The distance between enemy and player, when real distance is smaller, enemy will start to walk.
//     public int chanceToAttack = 4; // min 0, max 10;

//     public float chanceToBeStunned = 0.5f;//Chance to be stunned between 0 and 1;

//     //Audio
//     public AudioClip[] painSounds;
//     public AudioSource audioE;

//     // Blood effect
//     public GameObject bloodPrefab;

//     private float lastHitTime;
//     private float timeSinceLastHit;

//     private bool caughtPlayer = false;
//     private bool attacking = false;
//     private Animator animator;
//     private Transform player;
//     private NavMeshAgent2D enemy;
//     private bool BeenHit = false;

//     private bool isRandomMove;
//     private Vector2 RandomDestination;
//     private Transform playerCollider;
//     private bool[] formerStatus; //1-move; 2-kick;
//     private Vector3 playerLastPosition;
//     private int beenHitState = Animator.StringToHash("Base Layer.hit");
//     private int runState = Animator.StringToHash("Base Layer.PlayerMovement");
//     private int walkState = Animator.StringToHash("Base Layer.walk");
//     private int deathState = Animator.StringToHash("Base Layer.EnemyDeath");
//     private int idleState = Animator.StringToHash("Base Layer.idle");
//     private AnimatorStateInfo currentBaseState;
//     private int kickState = Animator.StringToHash("Base Layer.PlayerKicking");
//     private enum AnimationParams
//     {
//         PlayerMoving, EnemyWalking, PlayerKicking, hitme
//     }

//     void Start()
//     {
//         lastHitTime = 1f;
//         animator = GetComponent<Animator>();
//         enemy = GetComponent<NavMeshAgent2D>();
//         animator.SetBool("PlayerKicking", false);
//         player = GameObject.FindGameObjectWithTag("Player").transform;
//         formerStatus = new bool[3];
//         playerCollider = null;

//         if (isSit)
//         {
//             animator.SetBool("IsSit", true);
//         }

//         if (audioE == null)
//         {
//             audioE = GetComponentInChildren<AudioSource>();
//         }

//         ApplyAnimationEventToKickAnimation(CreateAnimationEvent());
//         //enemy.destination = player.position;
//     }

//     private AnimationEvent CreateAnimationEvent()
//     {
//         // new event created
//         return new AnimationEvent()
//         {
//             time = 0.06f,
//             functionName = "KickPlayer"
//         };
//     }

//     private void ApplyAnimationEventToKickAnimation(AnimationEvent evt)
//     {
//         foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
//         {
//             string name = clip.name;
//             if (name.StartsWith("Kick") || name.StartsWith("sword-slash"))
//             {
//                 bool isAdded = false;
//                 foreach (AnimationEvent e in clip.events)
//                 {
//                     if (e.functionName == evt.functionName)
//                     {
//                         isAdded = true;
//                     }
//                 }
//                 if (!isAdded)
//                 {
//                     clip.AddEvent(evt);
//                 }
//             }
//         }
//     }

//     public void EnemyRestoreFromHit()
//     {
//         animator.SetBool("PlayerMoving", formerStatus[0]);
//         animator.SetBool("PlayerKicking", formerStatus[1]);
//         animator.SetBool("EnemyWalking", formerStatus[2]);
//         animator.SetBool("hitme", false);
//         BeenHit = false;
//     }

//     public void EnemyBeenHit(int incomingdamage)
//     {
//         BeenHit = true;
//         //hurtMeSound.Play();
//         enemyHP -= incomingdamage;
//         if (Random.Range(0f, 1f) < chanceToBeStunned || CharacterController.getAttack() == "UpperCut")
//         {
//             SetEnemyAnimation(AnimationParams.hitme);
//             formerStatus[0] = animator.GetBool("PlayerMoving");
//             formerStatus[1] = animator.GetBool("PlayerKicking");
//             formerStatus[2] = animator.GetBool("EnemyWalking");
//         }


//         //BeenHit = false;
//         //stop any current action

//         //audio
//         int rand = UnityEngine.Random.Range(0, painSounds.Length);
//         audioE.clip = painSounds[rand];
//         audioE.Play();

//         // spawn blood
//         GameObject blood = Instantiate(bloodPrefab);
//         // set blood position
//         Vector3 bloodPos = this.transform.position;
//         blood.transform.position = bloodPos;
//         // set blood direction
//         float playerAngle = player.gameObject.GetComponent<CharacterController>().getPlayerAngle();
//         blood.GetComponent<BloodScript>().setBlood(playerAngle, (float)incomingdamage / 2f);
//         // set blood damage text
//         blood.GetComponentInChildren<damageTextScr>().setDamage(incomingdamage);

//         if (enemyHP <= 0) { EnemyDead(); }
//     }


//     void SetEnemyAnimation(AnimationParams type)
//     {
//         string targetAnimation = Enum.GetName(typeof(AnimationParams), type);
//         Array values = Enum.GetValues(typeof(AnimationParams));
//         foreach (AnimationParams val in values)
//         {
//             string name = Enum.GetName(typeof(AnimationParams), val);
//             if (val.Equals(type))
//             {
//                 animator.SetBool(name, true);
//             }
//             else
//             {
//                 animator.SetBool(name, false);
//             }
//         }

//         Vector2 pos = GetPlayerDirection(player, transform);
//         if (targetAnimation.Equals("PlayerKicking"))
//         {
//             lastHitTime = Time.time;
//             attacking = true;
//             animator.SetFloat("LastMoveX", pos.x);
//             animator.SetFloat("LastMoveY", pos.y);
//         }
//         else
//         {
//             if (enemyHP <= runAwayHP)
//             {
//                 animator.SetFloat("MoveX", -1 * pos.x);
//                 animator.SetFloat("MoveY", -1 * pos.y);
//             }
//             else
//             {
//                 animator.SetFloat("MoveX", pos.x);
//                 animator.SetFloat("MoveY", pos.y);
//             }
//         }
//     }

//     void StartToAttack()
//     {
//         if (!BeenHit)
//         {
//             enemy.Stop();
//             SetEnemyAnimation(AnimationParams.PlayerKicking);

//         }
//         else
//         {
//             if (!animator.GetCurrentAnimatorStateInfo(0).IsName("hit"))
//             {
//                 BeenHit = false;
//                 //animator.SetBool("hitme", false);
//                 EnemyRestoreFromHit();
//             }
//         }
//     }

//     private float AwayFromPlayerVertically(float playerY, float moveY)
//     {
//         float newY = 0f;
//         if (playerY > 0)
//         {
//             newY -= moveY;
//         }
//         else if (playerY < 0)
//         {
//             newY += moveY;
//         }
//         return newY;
//     }

//     Vector2 GetFurthestPointAfterPlayerToEnemy()
//     {
//         Vector2 playerPosition = GetPlayerDirection(player, transform);
//         Vector2 newPosition = transform.position;

//         float moveX = 5.5f; // delta value to move
//         float moveY = 5.5f; // delta value to move

//         if (playerPosition.x > 0) //player at the right side of enemy
//         {
//             if (playerPosition.y >= 0) //upper right
//             {
//                 newPosition.x -= moveX;
//                 newPosition.y -= moveY;
//             }
//             else if (playerPosition.y < 0) //down right
//             {
//                 newPosition.x -= moveX;
//                 newPosition.y += moveY;
//             }
//         }
//         else if (playerPosition.x < 0) //player at the left side of enemy
//         {
//             if (playerPosition.y >= 0) //upper left
//             {
//                 newPosition.x += moveX;
//                 newPosition.y -= moveY;
//             }
//             else if (playerPosition.y < 0) //down left
//             {
//                 newPosition.x += moveX;
//                 newPosition.y += moveY;
//             }
//         }
//         else if (playerPosition.x == 0)
//         {
//             if (playerPosition.y > 0)
//             { //player is at vertical top
//                 newPosition.x += Random.Range(-1 * moveX, moveX);
//                 newPosition.y -= moveY;
//             }
//             else if (playerPosition.y < 0)
//             { //player is at vertical down
//                 newPosition.x += Random.Range(-1 * moveX, moveX);
//                 newPosition.y += moveY;
//             }
//         }

//         return newPosition;
//     }

//     Vector2 GetRandomNearPlayerPosition()
//     {
//         Vector2 playerPos = GetPlayerDirection(player, transform);
//         Vector2 oldEnemyPos = transform.position;

//         float offSet = randomWalkRange;

//         if (playerPos.x >= 0) //player is at right
//         {
//             oldEnemyPos.x += -1 * Random.Range(0, offSet);
//             oldEnemyPos.y += Random.Range(-1 * offSet, offSet);
//         }
//         else if (playerPos.x < 0) //player is at left
//         {
//             oldEnemyPos.x += Random.Range(0, offSet);
//             oldEnemyPos.y += Random.Range(-1 * offSet, offSet);
//         }

//         return oldEnemyPos;
//     }

//     void EnemyRandomMove()
//     {
//         int randomNumber = Random.Range(0, 10);
//         if (randomNumber >= 0 && randomNumber <= chanceToAttack)
//         { //enemy will attack;
//             if (playerCollider != null)
//             {
//                 //player is at attacking range
//                 if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerKicking") && !attacking)
//                 {
//                     animator.SetBool("PlayerKicking", false);
//                 }
//                 if (!attacking && timeSinceLastHit >= 1)
//                 //if (!attacking)
//                 {
//                     StartToAttack();
//                 }
//             }
//         }
//         else if (randomNumber > chanceToAttack && randomNumber <= 10)
//         { //enemy will moveBack
//             enemy.Resume();
//             SetEnemyAnimation(AnimationParams.EnemyWalking);
//             enemy.destination = GetRandomNearPlayerPosition();

//             isRandomMove = true;
//             RandomDestination = enemy.destination;
//         }
//     }

//     public void KickPlayer()
//     {
//         AnimationEvent ae = new AnimationEvent();
//         ae.messageOptions = SendMessageOptions.DontRequireReceiver;
//     }

//     public void DoDamage()
//     {
//         PlayerHealth.doDamage(damage, this.transform.position);
//     }

//     void EnemyDead()
//     {
//         isDead = true;
//         // enemy.autoBraking = true;
//         // enemy.Stop();
//         // animator.SetBool("IsEnemyDead", true);

//         Vector2 deadPlace = transform.position;
//         Destroy(gameObject);
//         GameObject newOne = Instantiate(enemyPrefab, deadPlace, Quaternion.identity);
//         newOne.GetComponent<EnemyAI>().isDead = true;
//         newOne.GetComponent<Animator>().SetBool("IsEnemyDead", true);
//         return;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (isDead) { return; }

//         currentBaseState = animator.GetCurrentAnimatorStateInfo(0);
//         float remainingDistance = Vector2.Distance(transform.position, player.position);

//         if (isSit)
//         {
//             Wait(sitTime, () => { isSit = false; animator.SetBool("IsSit", false); });
//             return;
//         }

//         //some logic to check the isRandomMove

//         if (currentBaseState.fullPathHash.Equals(beenHitState))
//         {

//         }
//         else if (currentBaseState.fullPathHash.Equals(runState))
//         {
//             if (isRandomMove) { return; }
//             if (!caughtPlayer)
//             {
//                 if (remainingDistance <= checkDistance) { SetEnemyAnimation(AnimationParams.isWalk); }
//             }
//             else
//             {
//                 //caughter player, let's choose to attack
//                 RandomlyChooseAttackOrMove(chanceToAttack);
//             }
//         }
//         else if (currentBaseState.fullPathHash.Equals(walkState))
//         {
//             if (isRandomMove) { return; }
//             if (!caughtPlayer)
//             {
//                 if (remainingDistance > checkDistance) { SetEnemyAnimation(AnimationParams.isRun); }
//             }
//             else
//             {
//                 //caughter player, let's choose to attack                
//             }
//         }
//         else if (currentBaseState.fullPathHash.Equals(deathState))
//         {

//         }
//         else if (currentBaseState.fullPathHash.Equals(idleState))
//         {

//         }
//         else if (currentBaseState.fullPathHash.Equals(kickState))
//         {

//         }

//         playerLastPosition = player.position;
//     }

//     // void OnCollisionEnter2D(Collision2D coll)
//     // {
//     // }

//     // void OnCollisionExit2D(Collision2D coll)
//     // {
//     // }

//     private void RandomlyChooseAttackOrMove(int chance)
//     {
//         int randomNumber = Random.Range(0, 10);
//         if (randomNumber <= chance)
//         {
//             StartToAttack();
//         }
//         else
//         {
//             enemy.Stop();
//             enemy.Resume();
//             enemy.destination = GetRandomNearPlayerPosition();
//             isRandomMove = true;
//             RandomDestination = enemy.destination;
//         }
//     }

//     public void Wait(float seconds, Action action)
//     {
//         StartCoroutine(_wait(seconds, action));
//     }
//     IEnumerator _wait(float time, Action callback)
//     {
//         yield return new WaitForSeconds(time);
//         callback();
//     }

//     IEnumerator DoBlinks(float duration, float blinkTime)
//     {
//         Renderer renderer = enemy.GetComponent<Renderer>();
//         while (duration > 0f)
//         {
//             duration -= Time.deltaTime;

//             //toggle renderer
//             renderer.enabled = !renderer.enabled;

//             //wait for a bit
//             yield return new WaitForSeconds(blinkTime);
//         }

//         //make sure renderer is enabled when we exit
//         renderer.enabled = true;
//         Destroy(gameObject);
//     }

//     void OnTriggerEnter2D(Collider2D other)
//     {
//         if (!BeenHit)
//         {
//             if (other.tag == "Player")
//             {
//                 if (enemyHP > runAwayHP)
//                 {
//                     caughtPlayer = true;
//                     playerCollider = other.gameObject.transform;
//                 }
//             }
//             else if (other.tag == "Enemy")
//             {
//                 Debug.Log("Enemy");
//             }
//         }
//     }

//     void OnTriggerExit2D(Collider2D other)
//     {
//         if (other.tag == "Player")
//         {
//             caughtPlayer = false;
//             playerCollider = null;
//             BeenHit = false;
//             enemy.Resume();
//         }
//     }

//     private Vector2 GetPlayerDirection(Transform player, Transform enemy)
//     {
//         Transform transform = enemy;
//         float horizontal = player.position.x - transform.position.x;
//         float vertical = player.position.y - transform.position.y;

//         Vector2 pos = new Vector2(0, 0);
//         float offset = 0.4f; //use to make the enemy not that sensetive to direction

//         if (horizontal > offset)
//         {
//             pos.x = 1;
//         }
//         else if (horizontal < offset * -1)
//         {
//             pos.x = -1;
//         }
//         else if (horizontal >= offset * -1 && horizontal <= offset)
//         {
//             pos.x = 0;
//         }

//         if (vertical > offset)
//         {
//             pos.y = 1;
//         }
//         else if (vertical < offset * -1)
//         {
//             pos.y = -1;
//         }
//         else if (vertical >= offset * -1 && vertical <= offset)
//         {
//             pos.y = 0;
//         }

//         // if (enemyHP <= runAwayHP)
//         // {
//         //     pos.x *= -1;
//         //     pos.y *= -1;
//         // }

//         return pos;
//     }
// }
