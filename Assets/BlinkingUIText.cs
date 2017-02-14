using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class BlinkingUIText : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Color c = GetComponent<Text>().color;
		c.a = (Mathf.Sin(Time.time * 5) + 1f) / 2f;
		GetComponent<Text>().color = c;
	}
}
