using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Training : MonoBehaviour {
    private static bool trainingMode;
    private static Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //if(!anim.GetCurrentAnimatorStateInfo(0).IsName("punchingbaganimation"))
            //anim.SetBool("hit", false);
    }

    public static void animate(bool check)
    {
        anim.SetBool("hit", check);
    }

    public static void training(string combo)
    {


    }

    public static void setTrainingMode(bool train)
    {
        trainingMode = train;
    }

    public static bool getTrainingMode()
    {
        return trainingMode;
    }
}
