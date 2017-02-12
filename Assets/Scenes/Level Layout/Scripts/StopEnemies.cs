using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StopEnemies : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public GameObject[] enemies;
    
    // Update is called once per frame
    void Update () {

       enemies =  GameObject.FindGameObjectsWithTag("Enemy");

        if (Input.GetKey(KeyCode.J)) {
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<NavMeshAgent2D>().enabled = false;
            }

        }

        if (Input.GetKey(KeyCode.K)) {

            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<NavMeshAgent2D>().enabled = true;
            }
        }
		
	}
}
