using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using InControl;

[CommandInfo("QTE", "Invoke Troy loadLevel", "Use the method")]
public class invokeTroy : Command
{
    public string sceneName;

    // Use this for initialization
    void Start()
    {

    }

    public override void OnEnter()
    {
        Loading.loadLevel(sceneName);
    }

    void Update()
    {

    }
}
