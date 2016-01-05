/// <summary>
/// Author: Sonia SEDDIKI
/// Copyright (c) Val'EISTI - 2015
/// Sleep state launches an animation for the camera in order to have automatic moves inside the room.
/// </summary>
using UnityEngine;
using System.Collections;
using System;

public class Sleep : State{

    // Constructor
    public Sleep() : base() {
        _panel = GameObject.Find("StatePanels").GetComponentsInChildren<RectTransform>(true)[8].gameObject;
        Debug.Log("Panel sleep initialized" + _panel.ToString());
    }

    public override void trigger()
    {
        // Nothing else to add!
        _user._fpsController.GetComponent<Animation>().Stop();
        _panel.SetActive(false);
        _user.SetState(_user.EXPLORATOR);
    }

    public new void timeout()
    {
        // Nothing else to add! Redefines the State Timeout method since we don't want anything for Sleep mode
    }

    public override void behave()
    {
        _panel.SetActive(true);
        Debug.Log(_panel.ToString());
        Debug.Log(_panel.activeSelf);
        _user._fpsController.GetComponent<Animation>().Play();
        if (Input.anyKeyDown) {
            trigger();
        }
    }

    public override void GUI()
    {

    }
}
