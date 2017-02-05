using UnityEngine;
using UnityEngine.UI;

public class TextCanvas : MonoBehaviour {

    public static Text combo;

    void Start()
    {
        combo = GetComponent<Text>();
    }

    void Update()
    {
        if (!GameMaster.pause)
        {
            if (CharacterController.comboTracker.Count > 0)
            {
                //combo.text = CharacterController.comboTracker.Count + "";
                //combo.text = "Hit x" + CharacterController.comboTracker.Count;
            } else
            {
               combo.text = "";
            }
        }
    }
}
