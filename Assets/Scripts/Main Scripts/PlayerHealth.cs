using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public static float MaxHP;
    public static float PlayersHP;
    public static float Damage;
    public Texture HP_Indercator;
    public Texture HP_Container;
    Vector2 pos  = new Vector2(10,30);
    Vector2 size  = new Vector2(200,20);

	// Use this for initialization
	void Start () {
        MaxHP = 20;
        PlayersHP = MaxHP;
        Damage = 0;
	}

    void OnGUI()
    {

        // draw the background:
        GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
        GUI.DrawTexture(new Rect(0, 0, size.x, size.y), HP_Container);

        // Removes health from bar - bar total = 200
        GUI.BeginGroup(new Rect(0, 0, size.x - (Damage * 10), size.y));
        GUI.DrawTexture(new Rect(0, 0, size.x, size.y), HP_Indercator);

        GUI.EndGroup();
        GUI.EndGroup();

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown("space"))
        {
            doDamage(1);
        }

        if (PlayersHP <= 0)
        {
            //gameObject.SetActive(false);
            GameMaster.SoftReset();
        }
    }

    public static void doDamage(int damageAmount)
    {
        Damage += damageAmount;
        PlayersHP = MaxHP - Damage;
    }

    public static void heal(int healAmount)
    {
        Damage = Damage - healAmount < 0 ? 0 : Damage - healAmount;
        PlayersHP = MaxHP - Damage;
    }
}
