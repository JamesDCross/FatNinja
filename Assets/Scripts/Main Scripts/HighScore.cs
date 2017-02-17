using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour {
    public Text highScore;
    public Text yourScore;

    void Update()
    {
        setHighScore();
        highScore.text = "HIGH SCORE: "+ PlayerPrefs.GetInt("Score");
        yourScore.text = "YOUR SCORE: "+ Score.getScore();
    }

    public static void setHighScore()
    {
        if (!PlayerPrefs.HasKey("Score"))
            PlayerPrefs.SetInt("Score", Score.getScore());
        else
        {
            if (PlayerPrefs.GetInt("Score") < Score.getScore())
            {
                PlayerPrefs.SetInt("Score", Score.getScore());
            }
        }
    }
}
