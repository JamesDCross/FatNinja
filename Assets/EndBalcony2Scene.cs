using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndBalcony2Scene : MonoBehaviour
{
    public float pauseBeforeText;
    public float displayTextTime;
    public float nextLine;//should be when the enemy runs away


    public string quote;
    //public string quote2;

    public Text Line;
    public Image speechBubble;




    void Start()
    {
        Line.GetComponent<Text>().enabled = false;
        speechBubble.GetComponent<Image>().enabled = false;

        StartCoroutine(Timer());

       
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

        SceneManager.LoadScene("balcony2", LoadSceneMode.Single);

    }
}

