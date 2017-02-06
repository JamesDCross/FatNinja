using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		if (enemies.Length == 0) {
			this.GetComponent<Animator>().SetBool("OpenDoor", true);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("HIT DOOR WITH OBJECT: " + other.tag);
		if (other.tag == "Player"){
			SceneManager.LoadScene ("Dojo", LoadSceneMode.Single);
		}
	}
}
