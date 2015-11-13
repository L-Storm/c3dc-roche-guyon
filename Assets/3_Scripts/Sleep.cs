/// <summary>
/// Author: Sonia SEDDIKI
/// Copyright (c) Val'EISTI - 2015
/// Sleep state launches an animation for the camera in order to have automatic moves inside the room.
/// </summary>
using UnityEngine;
using System.Collections;

public class Sleep : State
{
    // Constructor
    public Sleep() : base() { }

    public override void trigger()
    {
        // Nothing else to add!
        Debug.Log("Back to Explorator mode");
        _user.setState(_user.EXPLORATOR);
    }

    public new void timeout()
    {
        // Nothing else to add! Redefines the State timeout method since we don't want anything for Sleep mode
    }

    public override void behave()
    {
        Debug.Log("I'm the Sleep mode!");
    }
}
