/// <summary>
/// Author: Sonia SEDDIKI
/// Copyright (c) Val'EISTI - 2015
/// Sleep state launches an animation for the camera in order to have automatic moves inside the room.
/// </summary>
using UnityEngine;
using System.Collections;
using System;

public class Sleep : State{

    private GameObject _panel;
    // Constructor
    public Sleep() : base() {
        _panel = GameObject.Find("StatePanels").GetComponentsInChildren<Transform>(true)[8].gameObject;
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
        _user._fpsController.GetComponent<Animation>().Play();
    }

    public override void GUI()
    {
        //if (_panel.activeSelf)
        //{
        //    //GameObject.Find("StatePanels/Sleep/Text").GetComponent<Text>().text = Database.Instance.GetData("object", id + ":title");
        //    //GameObject.Find("StatePanels/Inspector/Description").GetComponent<Text>().text = Database.Instance.GetData("object", id + ":desc");
        //}
    }
}
