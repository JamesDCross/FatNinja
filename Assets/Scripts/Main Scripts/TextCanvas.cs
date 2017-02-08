using UnityEngine;
using UnityEngine.UI;

public class TextCanvas : MonoBehaviour {

    public static Text comboText;

    void Start()
    {
        comboText = this.GetComponent<Text>();
    }

    public static void setText(string combo)
    {
        comboText.text = combo;
    }
}
