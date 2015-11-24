/// <summary>
/// Author: Sonia SEDDIKI
/// Copyright (c) Val'EISTI - 2015
/// Inspector state allows the player to observe an inspectable object in detail, with additional information and focus on the object.
/// </summary>
using UnityEngine;
using System.Collections;

public class Inspector : State
{
    // Constructor
    public Inspector() : base() { }

    public override void trigger()
    {
        // TODO: define Inspector mode behaviour
        Debug.Log("Launching Explorator mode");
        _user.SetState(_user.EXPLORATOR);
    }

    public override void behave()
    {
        Debug.Log("I'm the Inspector mode!");
    }

    public override void GUI()
    {

    }
}
