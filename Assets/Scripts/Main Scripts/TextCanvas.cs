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
            combo.text = "";            
        }
    }
}
