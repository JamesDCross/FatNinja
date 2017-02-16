﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour {

    public string level;
	public BoxCollider2D blockExitCollider = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");
		foreach	(GameObject go in spawners) {
			if (go.GetComponent<SpawnWaves>().numberOfWaves != 0)
			return;
		}

		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		if (enemies.Length == 0) {
			this.GetComponent<Animator>().SetBool("OpenDoor", true);
           // GetComponentInChildren<BoxCollider2D>().enabled = false;//doesnt work wtf
			if (blockExitCollider != null)
				blockExitCollider.enabled = false;
        }
    }

	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("HIT DOOR WITH OBJECT: " + other.tag);
		if (other.tag == "Player"){
			SceneManager.LoadScene (level);
		}
	}
}
