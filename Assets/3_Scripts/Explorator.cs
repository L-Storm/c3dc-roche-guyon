/// <summary>
/// Author: Valentin FAMELART
/// Copyright (c) Val'EISTI - 2015
/// Explorator state allows the player to movefreely around the room and to be notified whenever he is close to an inspectable object.
/// </summary>
using UnityEngine;
using UnityEngine.UI;

public class Explorator : State
{
    private Transform _cam;
    private GameObject _panel;

    // Constructor
    public Explorator() : base()
    {
        _cam = GameObject.Find("FPSController/FirstPersonCharacter").transform;
        _panel = GameObject.Find("StatePanels").GetComponentsInChildren<Transform>(true)[1].gameObject;
        Debug.Log("Panel initialized");
    }

    public override void trigger()
    {
        // TODO: define Explorator mode behaviour
        Debug.Log("Launching Inspector mode");
        _panel.SetActive(false);
        _user.SetState(_user.INSPECTOR);
    }

    /// <summary>
    /// Function using Raycast to return inspectable objects or null
    /// </summary>
    /// <returns>An inspectable objects or null if object is not inspectable</returns>
    private GameObject catchObject()
    {
        RaycastHit _hit;

        Debug.DrawRay(_cam.position, _cam.transform.forward * 5, Color.black);
        // Use a raycast to check if an inspectable object is faced by the camera

        int layerMask = 1 << LayerMask.NameToLayer("InspectableObjects");
        if (Physics.Raycast(_cam.position, _cam.forward, out _hit, 3f, layerMask))
        {
            return _hit.transform.gameObject;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Called in the update() of StateManager, detect interaction with objects 
    /// </summary>
    public override void behave()
    {
        // Catched the gameobject
        _user._inspectableObject = catchObject();
        if (_user._inspectableObject != null) {
            _user._inspectablePos = _user._inspectableObject.transform.position;
            _user._inspectableRot = _user._inspectableObject.transform.rotation;
        }
        

       if (_user._inspectableObject != null && !_panel.activeSelf)
        {
            // The faced GO is inspectable, showing a popup
            _panel.SetActive(true);
        }
        if (_user._inspectableObject == null)
        {
            _panel.SetActive(false);
        }
    }
    /// <summary>
    /// Function called by the StateManager to deal with the GUI
    /// </summary>
    public override void GUI()
    {
        if (_panel.activeSelf)
        {
            string id = _user._inspectableObject.GetComponent<Parameters>()._id;
            GameObject.Find("StatePanels/Explorator/ObjectName").GetComponent<Text>().text = Database.Instance.GetData("object", id + ":title");
            GameObject.Find("StatePanels/Explorator/InfoText").GetComponent<Text>().text = Database.Instance.GetData("explorator", "popup");
        }
    }
}
