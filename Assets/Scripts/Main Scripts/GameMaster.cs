using UnityEngine;
using InControl;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {
    PlayerAction characterActions;
    public static bool pause;
    bool pressed;
    bool keyboard;
    bool combo;
    private Rect PauseMenu;
    public Texture Background;
    private float posX = (Screen.width /2) - ((Screen.width * .3f) / 2);
    private float posY = (Screen.height /2) - ((Screen.height * .7f) / 2);
    private float width = (Screen.width * .3f);
    private float height = (Screen.height * .7f);
    private float exitsY;

    void Start () {
        
        characterActions = new PlayerAction();
        pause = false;
        combo = false;
        pressed = false;
        characterActions.Pause.AddDefaultBinding(InputControlType.Command);
        characterActions.Pause.AddDefaultBinding(Key.Escape);
        PauseMenu = new Rect(posX, posY, width, height);
        exitsY = posY + (height * .34f);
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
        if (GUI.Button(new Rect(posX + (width * .15f), posY + (height * .22f), width *.7f, 20), "Resume"))
        {
            pause = false;
            Time.timeScale = 1;
        }

        if (GUI.Button(new Rect(posX + (width * .15f), posY + (height * .28f), width * .7f, 20), "Combos"))
        {
            combo = combo ? false : true;
            if (combo)
            {
                exitsY = posY + (height * .64f);
                //PauseMenu.size = new Vector2(360, 290);
            }
            else
            {
                exitsY = posY + (height * .34f);
                //PauseMenu.size = new Vector2(360, 110);
            }
        }

        if (GUI.Button(new Rect(posX + (width * .15f), exitsY, width * .7f, 20), "Quit"))
        {

        }

        if (combo)
        {
            if (GUI.Button(new Rect(posX + (width * .15f), posY + (height * .34f), width * .34f , 20), "Controller"))
            {
                keyboard = false;
            }

            if (GUI.Button(new Rect(posX + (width * .51f), posY + (height * .34f), width * .34f, 20), "Keyboard"))
            {
                keyboard = true;
            }

            if (keyboard)
            {
                GUI.Label(new Rect(posX + (width * .15f), posY + (height * .40f), width * .7f, 20), "Kick X");
                GUI.Label(new Rect(posX + (width * .15f), posY + (height * .46f), width * .7f, 20), "Punch Z");
                GUI.Label(new Rect(posX + (width * .15f), posY + (height * .52f), width * .7f, 20), "Upper Cut Z Z X");
                GUI.Label(new Rect(posX + (width * .15f), posY + (height * .58f), width * .7f, 20), "Hurricane Kick X X Z");
            } 
            else
            {
                GUI.Label(new Rect(posX + (width * .15f), posY + (height * .40f), width * .7f, 20), "Kick B");
                GUI.Label(new Rect(posX + (width * .15f), posY + (height * .46f), width * .7f, 20), "Punch A");
                GUI.Label(new Rect(posX + (width * .15f), posY + (height * .52f), width * .7f, 20), "Upper Cut B B A");
                GUI.Label(new Rect(posX + (width * .15f), posY + (height * .58f), width * .7f, 20), "Hurricane Kick A A B");
            }
        }
        

    }

    public static void SoftReset()
    {
        PlayerHealth.Damage = 0;
        PlayerHealth.PlayersHP = PlayerHealth.MaxHP; 
         SceneManager.LoadScene("Alpha - FatNinja");

    }

    public static void HardReset()
    {
        SoftReset();
    }
}
