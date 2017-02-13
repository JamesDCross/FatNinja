using UnityEngine;



public class PlayerHealth : MonoBehaviour {

    public static float MaxHP;
    public static float PlayersHP = 0;
    public static float Damage;
    public Texture HP_Indercator;
    public Texture HP_Container;
    Vector2 pos  = new Vector2(10,30);
    Vector2 size  = new Vector2(200,20);

public GameObject bloodPrefabTEMP;
    private static GameObject bloodPrefab;

	// Use this for initialization
	void Start () {
        MaxHP = 20;
        PlayersHP = MaxHP;
        Damage = 0;
        bloodPrefab = bloodPrefabTEMP;
	}

    void OnGUI()
    {

        // draw the background:
        GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
        GUI.DrawTexture(new Rect(0, 0, size.x, size.y), HP_Container);

        // Removes health from bar - bar total = 20
        GUI.BeginGroup(new Rect(0, 0, size.x - (Damage * 10), size.y));
        GUI.DrawTexture(new Rect(0, 0, size.x, size.y), HP_Indercator);

        GUI.EndGroup();
        GUI.EndGroup();

    }

    // Update is called once per frame
    void Update () {
        if (PlayersHP <= 0)
        {
            //gameObject.SetActive(false);
            GameMaster.SoftReset();
        }
    }

    public static void doDamage(int damageAmount, Vector3 enemyPosition)
    {
        Damage += damageAmount;
        PlayersHP = MaxHP - Damage;

        // spawn blood
        GameObject blood = Instantiate(bloodPrefab);
        // set blood position
        Vector3 bloodPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        blood.transform.position = bloodPos;
        // set blood direction
        Vector2 enemyDirection = (GameObject.FindGameObjectWithTag("Player").transform.position - enemyPosition).normalized;
        float enemyAngle = enemyDirection.x * Vector2.Angle(Vector2.up, enemyDirection);
        //float playerAngle = player.gameObject.GetComponent<CharacterController>().getPlayerAngle();
        blood.GetComponent<BloodScript>().setBlood(enemyAngle, (float)damageAmount / 4f);
        // set blood damage text
        blood.GetComponentInChildren<damageTextScr>().setDamage(damageAmount);
    }

    public static void heal(int healAmount)
    {
        Damage = Damage - healAmount < 0 ? 0 : Damage - healAmount;
        PlayersHP = MaxHP - Damage;
    }
}
