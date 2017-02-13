using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Training : MonoBehaviour {
    private static bool trainingMode;

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
