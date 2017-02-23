using UnityEngine;

public class DoorScript : MonoBehaviour {

    public string level;
	public BoxCollider2D blockExitCollider = null;
    private bool loadLevel;
    //public float[] enemiesDead;
    

    // Use this for initialization
    void Start () {
        loadLevel = false;
    }
	
	// Update is called once per frame
	void Update () {
		GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");
		for	(int i = 0; i < spawners.Length;i++) {
            //enemiesDead[i] = spawners[i].GetComponent<SpawnWaves>().numberOfWaves;
            if (spawners[i].GetComponent<SpawnWaves>().numberOfWaves != 0)
                return;
                Debug.Log("nothing");

        }

		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		if (enemies.Length == 0) {
			this.GetComponent<Animator>().SetBool("OpenDoor", true);
           // GetComponentInChildren<BoxCollider2D>().enabled = false;//doesnt work wtf
			if (blockExitCollider != null)
				blockExitCollider.enabled = false;
        }
        if (loadLevel)
        {
            loadLevel = false;
            Loading.loadLevel(level);
        }
    }

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player"){
            loadLevel = true;
		}
	}
}
