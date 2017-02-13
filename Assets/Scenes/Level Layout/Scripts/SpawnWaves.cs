using UnityEngine;
using System.Collections;

public class SpawnWaves : MonoBehaviour
{
   public GameObject enemyType;
    //public Vector2 spawnValues;
   
    public int howManyInWave;
    public float timeBetweenEnemies;
    public float timeBeforeWavesStart;
    public float timeBetweenWaves;
    public float numberOfWaves;

    public GameObject shibo;

    private Random rand;
    

    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(timeBeforeWavesStart);

        while (numberOfWaves > 0)
        {
            for (int i = 0; i < howManyInWave; i++)
            {
                Vector2 spawnPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
                Quaternion spawnRotation = Quaternion.identity;

                Instantiate(enemyType, spawnPosition, spawnRotation);

                //enemies have a chance to run away
                int rand = UnityEngine.Random.Range(3, 8);

                if (enemyType.tag == "Enemy") {
                    enemyType.GetComponent<EnemyAI>().runAwayHP = rand;
                }

                //Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"));
                yield return new WaitForSeconds(timeBetweenEnemies);
            }
            yield return new WaitForSeconds(timeBetweenWaves);

            numberOfWaves--;
        }

       
    }
}
