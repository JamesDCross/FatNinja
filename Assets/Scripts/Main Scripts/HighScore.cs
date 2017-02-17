using InControl;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour {
    public Text highScore;
    public Text yourScore;
    PlayerAction characterActions;

    void Start()
    {
        characterActions = new PlayerAction();
        characterActions.Pause.AddDefaultBinding(InputControlType.Command);
        characterActions.Pause.AddDefaultBinding(Key.Escape);
    }

    void Update()
    {
        setHighScore();
        highScore.text = "HIGH SCORE: "+ PlayerPrefs.GetInt("Score");
        yourScore.text = "YOUR SCORE: "+ Score.getScore();

        if (characterActions.Pause)
        {
            Loading.loadLevel("Credits");
        }
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
