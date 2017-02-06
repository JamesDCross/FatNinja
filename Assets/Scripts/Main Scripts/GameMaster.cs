using UnityEngine;
using InControl;

public class GameMaster : MonoBehaviour {
    PlayerAction characterActions;
    public static bool pause;
    bool pressed;
    bool keyboard;
    bool combo;
    public Rect PauseMenu;
    public Texture Background;

    private bool gameStarting;
    int x = 80;

    void Start () {
        
        characterActions = new PlayerAction();
        pause = true;
        combo = false;
        pressed = false;
        gameStarting  = true;
        characterActions.Pause.AddDefaultBinding(InputControlType.Command);
        characterActions.Pause.AddDefaultBinding(Key.Escape);
    }

    // Update is called once per frame
    void Update()
    {
        if (characterActions.Pause || gameStarting)
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

    }

    void OnGUI()
    {
        if (pause)
        {
            GUI.Window(0, PauseMenu, ThePauseMenu, "Main menu");
            GUI.DrawTexture(PauseMenu, Background);
        }
    }

    void ThePauseMenu(int windowID)
    {
        if (GUI.Button(new Rect(10, 20, 340, 20), "Resume"))
        {
            pause = false;
            Time.timeScale = 1;
        }

        if (GUI.Button(new Rect(10, 50, 340, 20), "Combos"))
        {
            combo = combo ? false : true;
            if (combo)
            {
                x = 260;
                PauseMenu.size = new Vector2(360, 290);
            }
            else
            {
                x = 80;
                PauseMenu.size = new Vector2(360, 110);
            }
        }

        if (GUI.Button(new Rect(10, x, 340, 20), "Quit"))
        {

        }

        if (x > 80)
        {
            if (GUI.Button(new Rect(10, 80, 160, 20), "Controller"))
            {
                keyboard = false;
            }

            if (GUI.Button(new Rect(190, 80, 160, 20), "Keyboard"))
            {
                keyboard = true;
            }

            if (keyboard)
            {
                GUI.Label(new Rect(10, 110, 340, 20), "Kick X");
                GUI.Label(new Rect(10, 140, 340, 20), "Punch Z");
                GUI.Label(new Rect(10, 170, 340, 20), "Spinning kick X X C");
                GUI.Label(new Rect(10, 200, 340, 20), "Sumo Slam C Z X C");
                GUI.Label(new Rect(10, 230, 340, 20), "Rolling Sumo C X X Z V");
            } 
            else
            {
                GUI.Label(new Rect(10, 110, 340, 20), "Kick B");
                GUI.Label(new Rect(10, 140, 340, 20), "Punch A");
                GUI.Label(new Rect(10, 170, 340, 20), "Spinning kick B B X");
                GUI.Label(new Rect(10, 200, 340, 20), "Sumo Slam X A B X");
                GUI.Label(new Rect(10, 230, 340, 20), "Rolling Sumo X B B A Y");
            }
        }
        

    }

    public static void SoftReset()
    {
        PlayerHealth.Damage = 0;
        PlayerHealth.PlayersHP = PlayerHealth.MaxHP;        
    }

    public static void HardReset()
    {
        SoftReset();
    }
}
