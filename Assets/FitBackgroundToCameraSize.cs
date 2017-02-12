using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitBackgroundToCameraSize : MonoBehaviour {

	public Material mat;

	// Use this for initialization
	void Start () {
		float size = GetComponent<Camera>().orthographicSize;
		mat.SetFloat ("_cameraSize", size);
		//Renderer.// .SetFloat ("Cam Size", size);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
