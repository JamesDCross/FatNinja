using System;
using System.Collections;
using UnityEngine;
using InControl;



public class PlayerAction : PlayerActionSet {
    public InControl.PlayerAction Pause;
    public InControl.PlayerAction Punch;
    public InControl.PlayerAction Kick;
    public InControl.PlayerAction Roll;
    public InControl.PlayerAction Blank;

	public PlayerAction() {
        Pause = CreatePlayerAction("Pause");
        Punch = CreatePlayerAction("Punch");
        Kick = CreatePlayerAction("Kick");
        Roll = CreatePlayerAction("Roll");
        Blank = CreatePlayerAction("Blank");
    }
}
