using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogTimer : MonoBehaviour
{
    public float pauseBeforeText;
    public float displayTextTime;
   


    public string quote;
    

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

        

        yield return new WaitForSeconds(pauseBeforeText);
        Line.GetComponent<Text>().enabled = true;
        speechBubble.GetComponent<Image>().enabled = true;

        yield return new WaitForSeconds(displayTextTime);
        Line.GetComponent<Text>().enabled = false;
        speechBubble.GetComponent<Image>().enabled = false;

        



    }
}
