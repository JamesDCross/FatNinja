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

        characterActions.Level1.AddDefaultBinding(InputControlType.Action1);
        characterActions.Level1.AddDefaultBinding(Key.Z);
        characterActions.Level1.AddDefaultBinding(Key.Return);
        characterActions.Level1.AddDefaultBinding(Key.Space);


    }

    void Update()
    {
        LoadLevel1();
    }

    public void LoadLevel1()
    {
        if (characterActions.Level1)
            Loading.loadLevel("LVL1-Balcony");
    }
}
