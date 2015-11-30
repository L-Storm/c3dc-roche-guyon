/// <summary>
/// Author: Sonia SEDDIKI
/// Copyright (c) Val'EISTI - 2015
/// Explorator state allows the player to movefreely around the room and to be notified whenever he is close to an inspectable object.
/// </summary>
using UnityEngine;
using UnityEngine.UI;

public class Explorator : State
{
    private Transform _cam;
    private GameObject _panel;
    private bool _catchObject = false;

    // Constructor
    public Explorator() : base()
    {
        _cam = GameObject.Find("FPSController/FirstPersonCharacter").transform;
        _panel = GameObject.Find("StatePanels").GetComponentsInChildren<Transform>(true)[1].gameObject;
    }

    public override void trigger()
    {
        // Deactivate panel and delete link to DB on language change
        Database.Instance._OnChange -= SetText;
        _panel.SetActive(false);

        // Switching to Inspector mode
        _user.SetState(_user.INSPECTOR);
    }

    /// <summary>
    /// Function using Raycast to return inspectable objects or null
    /// </summary>
    /// <returns>An inspectable objects or null if object is not inspectable</returns>
    private GameObject catchObject()
    {
        RaycastHit _hit;
        
        // Use a raycast to check if an inspectable object is faced by the camera
        int layerMask = 1 << LayerMask.NameToLayer("InspectableObjects");
        if (Physics.Raycast(_cam.position, _cam.forward, out _hit, 5, layerMask))
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
        Database.Instance._OnChange += SetText;

        // On récupère le GameObject qui est à inspecter ou null
        _user._inspectableObject = catchObject();
        // On actualise le marqueur signifiant qu'un object peut être inspecter
        _catchObject = (_user._inspectableObject == null ? false : true);

    }

    private void SetText()
    {
        string id = _user._inspectableObject.GetComponent<Parameters>()._id;
        GameObject.Find("StatePanels/Explorator/ObjectName").GetComponent<Text>().text = Database.Instance.GetData("object", id + ":title");
        GameObject.Find("StatePanels/Explorator/InfoText").GetComponent<Text>().text = Database.Instance.GetData("explorator", "popup");
    }

    /// <summary>
    /// Function called by the StateManager to deal with the GUI
    /// </summary>
    public override void GUI()
    {
        if (_catchObject && !_panel.activeSelf)
        {
            // The faced GO is inspectable, showing a popup and refreshing text
            _panel.SetActive(true);
            SetText();
        }
        if (!_catchObject && _panel.activeSelf)
        {
            _panel.SetActive(false);
        }
    }
}
