using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerCombo : MonoBehaviour {

    public Sprite A;
    public Sprite B;
    public Sprite Y;
    public Sprite X;
    public SpriteRenderer[] combo = new SpriteRenderer[5];
    public Text comboInfo;

    float lastSucces;
    float timeSinceLastSucces;

    string[][][] comboList = new string[][][] {
        new string[][] { new string[] { "Punch", "Punch", "Kick" }, new string[] { "UpperCut" }},
        new string[][] { new string[] {"Kick", "Kick", "Blank"}, new string[] { "CraneKick" } },
        new string[][] { new string[] {"Blank", "Punch", "Kick", "Blank"}, new string[] { "SumoSlam" }},
        new string[][] { new string[] {"Blank", "Kick", "Kick", "Punch", "Roll"}, new string[] { "SumoRoll" }}
    };

    List<string[][]> refindComboList = new List<string[][]>();

    void Update()
    {
        if (!GameMaster.pause)
        {
            List<string[][]> tempComboList = new List<string[][]>();

            if (CharacterController.comboTracker.Count == 1)
            {
                foreach (string[][] moveSet in comboList)
                {
                    if (CharacterController.comboTracker.Count == 1)
                    {
                        if (moveSet[0][0] == CharacterController.comboTracker[0])
                        {
                            refindComboList.Add(moveSet);
                        }
                    }
                }
            } else if (CharacterController.comboTracker.Count > 1)
            {
                bool comboSuccess = false;
                foreach (string[][] moveSet in refindComboList)
                {
                    if (moveSet[0][CharacterController.comboTracker.Count - 1] == CharacterController.comboTracker[CharacterController.comboTracker.Count - 1])
                    {
                        if (moveSet[0].Length == CharacterController.comboTracker.Count)
                        {
                            //Combo Succes
                            comboSuccess = true;
                            lastSucces = Time.time;
                            comboInfo.text = "Combo("+ moveSet[1][0]+") Success";
                        } else
                        {
                            tempComboList.Add(moveSet);
                        }
                    }
                }
                refindComboList = tempComboList;
                if (refindComboList == null || refindComboList.Count == 0 && !comboSuccess)
                {
                    //Combo Fail
                    CharacterController.comboReset();
                    lastSucces = Time.time;
                    comboInfo.text = "Combo Fail";
                }
                if (comboSuccess)
                {
                    CharacterController.comboReset();
                }
            }


            timeSinceLastSucces = Time.time - lastSucces;
            if (timeSinceLastSucces > .5f)
            {
                comboInfo.text = "";
            }


            if (CharacterController.comboTracker.Count > 0 && CharacterController.comboTracker.Count <= 5)
            {
                combo[CharacterController.comboTracker.Count - 1].GetComponent<SpriteRenderer>().enabled = true;
                if (CharacterController.comboTracker[CharacterController.comboTracker.Count - 1] == "Punch")
                {
                    combo[CharacterController.comboTracker.Count - 1].GetComponent<SpriteRenderer>().sprite = A;
                } else if (CharacterController.comboTracker[CharacterController.comboTracker.Count - 1] == "Kick")
                {
                    combo[CharacterController.comboTracker.Count - 1].GetComponent<SpriteRenderer>().sprite = B;
                } else if (CharacterController.comboTracker[CharacterController.comboTracker.Count - 1] == "Roll")
                {
                    combo[CharacterController.comboTracker.Count - 1].GetComponent<SpriteRenderer>().sprite = Y;
                } else if (CharacterController.comboTracker[CharacterController.comboTracker.Count - 1] == "Blank")
                {
                    combo[CharacterController.comboTracker.Count - 1].GetComponent<SpriteRenderer>().sprite = X;
                }
            }

            for (int i = 0; i < 5; i++)
            {
                if (i >= CharacterController.comboTracker.Count)
                {
                    combo[i].GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        }
    } 
}
