using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnActivate : MonoBehaviour {
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") {
            GetComponent<SpawnWaves>().enabled = true;
            
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
