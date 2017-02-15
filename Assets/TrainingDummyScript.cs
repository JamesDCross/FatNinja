using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummyScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {

		if (other.tag == "Shibos Attack") {
			Debug.Log ("fuck yea");
			GetComponentInChildren<Animator> ().SetTrigger ("spin");
		}
	}
}
