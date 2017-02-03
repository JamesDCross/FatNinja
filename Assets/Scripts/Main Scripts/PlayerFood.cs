using UnityEngine;

public class PlayerFood : MonoBehaviour {
    public static int MaxFood = 4;
    public static int PlayersFood = 0;
    public Texture Food_Empty;
    public Texture Food_Full;

    // Use this for initialization
    void Start () {
		
	}

    void OnGUI() { 
        int count = 10;
        for (int i = 0; i < MaxFood; i++)
        {
            if (i >= PlayersFood) {
                GUI.BeginGroup(new Rect(count, 50, 48, 40));
                GUI.DrawTexture(new Rect(0, 0, 48, 40), Food_Empty);
                GUI.EndGroup();
                count += 50;
            } 
            else
            {
                GUI.BeginGroup(new Rect(count, 50, 48, 40));
                GUI.DrawTexture(new Rect(0, 0, 48, 40), Food_Full);
                GUI.EndGroup();
                count += 50;
            }
        }
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown("e"))
        {
            getFood();
        }
    }

    void eatFood()
    {
        if (PlayersFood > 0)
        {
            PlayersFood--;
        }
    }

    void getFood()
    {
        if (PlayersFood <= MaxFood)
        {
            PlayersFood++;
        }
    }
}
