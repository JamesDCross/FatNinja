using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour
{

    static MusicPlayer INSTANCE = null;
    private static bool destroyObj = false;

    void Awake()
    {
        Debug.Log("Music player Awake " + GetInstanceID());
        if (INSTANCE != null)
        {
            Destroy(gameObject);
            print("Duplicate music player self-destructing!");
        }
        else
        {
            INSTANCE = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }

    void Update()
    {
        if (destroyObj)
        {
            Destroy(gameObject);
            destroyObj = false;
        }
    }

    public static void destroyMusic()
    {
        destroyObj = true;
    }
    // Use this for initialization
    void Start()
    {
        Debug.Log("Music player Start " + GetInstanceID());
    }

}

