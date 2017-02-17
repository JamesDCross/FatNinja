using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    private static string level;

    void Update()
    {
        SceneManager.LoadScene(level);
    }

    public static void loadLevel(string lvl)
    {
        if (lvl.Equals("Start Menu11"))
        {
            GameMaster.HardReset();
        }
        else
        {
            SceneManager.LoadScene("Loading", LoadSceneMode.Single);
            level = lvl;
        }
    }
}
