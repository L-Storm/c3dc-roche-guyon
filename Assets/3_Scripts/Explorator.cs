/// <summary>
/// Author: Sonia SEDDIKI
/// Copyright (c) Val'EISTI - 2015
/// Explorator state allows the player to movefreely around the room and to be notified whenever he is close to an inspectable object.
/// </summary>
using UnityEngine;
using System.Collections;


public class Explorator : State
{
    // Constructor
    public Explorator() : base() { }

    public override void trigger()
    {
        // TODO: define Explorator mode behaviour
        Debug.Log("Launching Inspector mode");
        _user.SetState(_user.INSPECTOR);
    }

    public override void behave()
    {
        Debug.Log("I'm the Explorator mode!");
    }

    public override void GUI()
    {

    }

}
