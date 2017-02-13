using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level1Dialog: MonoBehaviour
{
    public float pauseBeforeText;
    public float displayTextTime;



    public string quote;


    public Text Line;

    public Image speechBubble;

    public bool levelClear;
    public GameObject[] enemies;
    public GameObject spawner;

    private Scene scene;

    void Start()
    {
        levelClear = false;
        scene = SceneManager.GetActiveScene();
        Debug.Log(scene.name);

        Line.GetComponent<Text>().enabled = false;
        speechBubble.GetComponent<Image>().enabled = false;

        if (scene.name == "LVL1-Balcony" && levelClear == true) {
            Debug.Log("what upp2222");
            
        }
    }

    void Update()
    {
       enemies = GameObject.FindGameObjectsWithTag("Enemy");
      

        

        if (spawner.GetComponent<SpawnWaves>().numberOfWaves == 0 && enemies.Length == 0)
        {
            levelClear = true;
            Debug.Log("what upp");
            
            StartCoroutine(Timer());
           


        }
    }

    public IEnumerator Timer()
    {
        Line.text = quote;

        while (levelClear) {

            yield return new WaitForSeconds(pauseBeforeText);
            Line.GetComponent<Text>().enabled = true;
            speechBubble.GetComponent<Image>().enabled = true;

            yield return new WaitForSeconds(displayTextTime);
            Line.GetComponent<Text>().enabled = false;
            speechBubble.GetComponent<Image>().enabled = false;

            
        }
        
    }

    




}
