using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodScript : MonoBehaviour {

	private ParticleSystem particles = null;

	// Use this for initialization
	void Start () {
		Setup();
	}

	void Setup() {
		if (particles == null) {
			particles = GetComponentInChildren<ParticleSystem>();
		}
	}
	
	public void setBlood(float angle, float size) {
		Setup();
		particles.gameObject.transform.localScale = new Vector3(size, size, size);
		particles.gameObject.transform.Rotate(
			0, angle, 0
		);
	}
}
