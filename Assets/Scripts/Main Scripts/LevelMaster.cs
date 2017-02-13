using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using InControl;

public class LevelMaster : MonoBehaviour
{

    PlayerAction characterActions;
    //add incontrol - so can use controller buttons

    void Start() {
       characterActions = new PlayerAction();

        characterActions.Level1.AddDefaultBinding(InputControlType.Action2);
        characterActions.Level1.AddDefaultBinding(Key.X);

        characterActions.Controls.AddDefaultBinding(InputControlType.Action3);
        characterActions.Controls.AddDefaultBinding(Key.C);


    }

void Update(){

LoadLevel1();
//LoadLevelControls();


}
    public void LoadLevel1()
    {
        //if button is ... 
        Debug.Log("New Level load: ");
        if (characterActions.Level1)
         //if(Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene("Intro");
         }
    }

    /*public void LoadLevelControls()
    {
        //if button is ... 
        Debug.Log("New Level load: ");
         if (characterActions.Controls)
         //if(Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene("Controls");
         }
    }*/





    /*public void QuitRequest()
    {
        Debug.Log("Quit requested");
        Application.Quit();
    }*/

}
