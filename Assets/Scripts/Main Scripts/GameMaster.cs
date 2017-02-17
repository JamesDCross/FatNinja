using UnityEngine;
using InControl;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {
    PlayerAction characterActions;
    public Text scoreText;
    public Text ScoreMultipler;
    public static bool pause;
    bool pressed;
    bool keyboard;
    bool combo;
    private Rect PauseMenu;
    public Texture Background;
    private float posX = (Screen.width / 2) - ((Screen.width * .3f) / 2);
    private float posY = (Screen.height / 2) - ((Screen.height * .7f) / 2);
    private float width = (Screen.width * .3f);
    private float height = (Screen.height * .7f);
    private float exitsY;
    private GUIStyle style;
    private GUIStyle styleButton;
    public Font font;
    public static float scoreUpdateTime = -1.2f;



    void Start()
    {
        characterActions = new PlayerAction();
        pause = false;
        combo = false;
        pressed = false;
        characterActions.Pause.AddDefaultBinding(InputControlType.Command);
        characterActions.Pause.AddDefaultBinding(Key.Escape);
        PauseMenu = new Rect(posX, posY, width, height);
        exitsY = posY + (height * .34f);
        style = new GUIStyle();
        styleButton = new GUIStyle();
    }

    // Update is called once per frame
    void Update()
    {
        if (characterActions.Pause)
        {
            if (!pressed)
            {
                pause = pause ? false : true;
                pressed = true;
                Time.timeScale = pause ? 0 : 1;
            }
        } else
        {
            pressed = false;
        }

        scoreText.text = "SCORE: " + Score.getScore();
        if (scoreUpdateTime + 3f > Time.time)
            ScoreMultipler.text = Score.getDamage() + "  X " + Score.getMultipler();
        else
        {
            ScoreMultipler.text = "X " + Score.getMultipler();
            Score.resetMultipler();
        }
    }

    public static void setScoretimer()
    {
        scoreUpdateTime = Time.time;
    }

    void OnGUI()
    {
        if (pause)
        {
            //GUI.Window(0, PauseMenu, ThePauseMenu, "Main menu");
            GUI.DrawTexture(PauseMenu, Background);
            ThePauseMenu();
        }
    }

    void ThePauseMenu()
    {
        GUI.color = Color.black;
        GUI.backgroundColor = Color.white;
        style.fontSize = 18;
        styleButton.fontSize = 22;
        styleButton.font = font;
        if (GUI.Button(new Rect(posX + (width * .15f), posY + (height * .22f), width * .7f, 20), "Resume", styleButton))
        {
            pause = false;
            Time.timeScale = 1;
        }

        if (GUI.Button(new Rect(posX + (width * .15f), posY + (height * .28f), width * .7f, 20), "Combos", styleButton))
        {
            combo = combo ? false : true;
            if (combo)
            {
                exitsY = posY + (height * .70f);
                //PauseMenu.size = new Vector2(360, 290);
            } else
            {
                exitsY = posY + (height * .34f);
                //PauseMenu.size = new Vector2(360, 110);
            }
        }

        if (GUI.Button(new Rect(posX + (width * .15f), exitsY, width * .7f, 20), "Quit", styleButton))
        {
            HardReset();
        }

        if (combo)
        {
            if (GUI.Button(new Rect(posX + (width * .15f), posY + (height * .34f), width * .34f, 20), "Controller", styleButton))
            {
                keyboard = false;
            }

            if (GUI.Button(new Rect(posX + (width * .51f), posY + (height * .34f), width * .34f, 20), "Keyboard", styleButton))
            {
                keyboard = true;
            }

            if (keyboard)
            {
                GUI.Label(new Rect(posX + (width * .15f), posY + (height * .40f), width * .7f, 20), "Kick X", style);
                GUI.Label(new Rect(posX + (width * .15f), posY + (height * .46f), width * .7f, 20), "Punch Z", style);
                GUI.Label(new Rect(posX + (width * .15f), posY + (height * .52f), width * .7f, 20), "Upper Cut Z Z X", style);
                GUI.Label(new Rect(posX + (width * .15f), posY + (height * .58f), width * .7f, 20), "Hurricane Kick X X Z", style);
                GUI.Label(new Rect(posX + (width * .15f), posY + (height * .64f), width * .7f, 20), "Surasshu Slash X Z Z X", style);
            } else
            {
                GUI.Label(new Rect(posX + (width * .15f), posY + (height * .40f), width * .7f, 20), "Kick B", style);
                GUI.Label(new Rect(posX + (width * .15f), posY + (height * .46f), width * .7f, 20), "Punch A", style);
                GUI.Label(new Rect(posX + (width * .15f), posY + (height * .52f), width * .7f, 20), "Upper Cut A A B", style);
                GUI.Label(new Rect(posX + (width * .15f), posY + (height * .58f), width * .7f, 20), "Hurricane Kick B B A", style);
                GUI.Label(new Rect(posX + (width * .15f), posY + (height * .64f), width * .7f, 20), "Surasshu Slash B A A B", style);
            }
        }


    }

    public static void SoftReset()
    {
        Score.scoreReset();
        Scene scene = SceneManager.GetActiveScene();
        PlayerHealth.Damage = 0;
        PlayerHealth.PlayersHP = PlayerHealth.MaxHP;
        pause = false;
        Time.timeScale = 1;
        Loading.loadLevel(scene.name);
    }

    public static void HardReset()
    {
        Score.scoreReset();
        MusicPlayer.destroyMusic();
        PlayerHealth.Damage = 0;
        PlayerHealth.PlayersHP = PlayerHealth.MaxHP;
        pause = false;
        Time.timeScale = 1;
        Loading.loadLevel("Start Menu");
    }
}
