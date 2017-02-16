using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour {
    private static string level;

    void Update()
    {
        SceneManager.LoadScene(level);
    }
	
	public static void loadLevel(string lvl)
    {
        SceneManager.LoadScene("Loading", LoadSceneMode.Single);
        level = lvl;
    }
}
