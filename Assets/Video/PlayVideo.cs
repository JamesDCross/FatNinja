using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayVideo : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //((MovieTexture)GetComponent<Renderer>().material.mainTexture).Play();
        StartCoroutine(WaitAndLoad(3f, "Intro")); //8f
        

    }
    private IEnumerator WaitAndLoad(float value, string scene)
    {
        yield return new WaitForSeconds(value);
        SceneManager.LoadScene(scene);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
