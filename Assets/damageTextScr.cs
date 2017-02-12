using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageTextScr : MonoBehaviour {

	public float risingSpeed = 3.0f;
	public float finalSize = 0.3f;
	private float currentSize;
	private float currentAlpha = 1.0f;

	// Use this for initialization
	void Start () {
		currentSize = transform.localScale.x;
		setText("9999");
	}
	
	public void setText(string s) {
		GetComponent<TextMesh>().text = s;
	}

	// Update is called once per frame
	void Update () {
		currentSize = Mathf.Lerp(currentSize, finalSize, Time.deltaTime);
		transform.localScale = currentSize * Vector3.one;
		transform.Translate(new Vector3(0, risingSpeed * Time.deltaTime, 0));
	}
}
