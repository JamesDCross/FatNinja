using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillQuote : MonoBehaviour {

    public string quote;
    public Text Line;

    public float pauseBeforeText;
    public float displayTextTime;
    public bool speak = false;

    private bool spoken = false;

   
    public Image speechBubble;
    // Use this for initialization
    void Start () {
        Line.GetComponent<Text>().enabled = false;
        speechBubble.GetComponent<Image>().enabled = false;

    }

    
	
	// Update is called once per frame
	void Update () {

        //speak = false;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
        {
            speak = true;
        }

        if (speak && !spoken) {
            StartCoroutine(Timer());
            spoken = true;
        }

    }

    public IEnumerator Timer()
    {
        Line.text = quote;

        //Line2.text = quote2;

        yield return new WaitForSeconds(pauseBeforeText);
        Line.GetComponent<Text>().enabled = true;
        speechBubble.GetComponent<Image>().enabled = true;

        yield return new WaitForSeconds(displayTextTime);
        Line.GetComponent<Text>().enabled = false;
        speechBubble.GetComponent<Image>().enabled = false;




    }





}


