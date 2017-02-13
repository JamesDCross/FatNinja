using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageTextScr : MonoBehaviour {

	private float risingSpeed = 2.0f;
	private float finalSize = 0.2f;
	private float currentSize;
	private float currentAlpha = 1.0f;

	// Use this for initialization
	void Start () {
		currentSize = transform.localScale.x;
		setDamage(2);
	}
	
	public void setDamage(int damage) {
		GetComponent<TextMesh>().text = "" + ((damage * 100) + Random.Range(0, 100));
	}

	// Update is called once per frame
	void Update () {
		//currentSize = Mathf.Lerp(currentSize, finalSize, Time.deltaTime * 2.0f);
		//transform.localScale = currentSize * Vector3.one;
		
		transform.Translate(new Vector3(0, risingSpeed * Time.deltaTime, 0));

		currentAlpha = Mathf.Lerp(currentAlpha, 0.0f, Time.deltaTime * 2.0f);
		Color c = GetComponent<TextMesh>().color;
		c.a = currentAlpha;
		GetComponent<TextMesh>().color = c;
	}
}
