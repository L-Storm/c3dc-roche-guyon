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
    private GameObject _inspectableObject = null;
    private GameObject _panel;
    // Constructor
    public Explorator() : base()
    {
        _cam = GameObject.Find("FPSController/FirstPersonCharacter").transform;
        _panel = GameObject.Find("StatePanels").GetComponentsInChildren<Transform>(true)[1].gameObject;
    }

    public override void trigger()
    {
        // TODO: define Explorator mode behaviour
        Debug.Log("Launching Inspector mode");
        _user.SetState(_user.INSPECTOR);
    }

    private GameObject catchObject()
    {
        RaycastHit _hit;

        Debug.DrawRay(_cam.position, _cam.transform.forward * 5, Color.black);
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

    public override void behave()
    {
        bool _catchObject = false;

        // On récupère le GameObject qui est à inspecter ou null
        _inspectableObject = catchObject();
        // On actualise le marqueur signifiant qu'un object peut être inspecter
        _catchObject = (_inspectableObject == null ? false : true);

        if (_catchObject && !_panel.activeSelf)
        {
            // The faced GO is inspectable, showing a popup
            _panel.SetActive(true);
        }
        if (!_catchObject)
        {
            _panel.SetActive(false);
        }
    }

    public override void GUI()
    {
        if (_panel.activeSelf)
        {
            string id = _inspectableObject.GetComponent<Parameters>()._id;
            GameObject.Find("StatePanels/Explorator/ObjectName").GetComponent<Text>().text = Database.Instance.GetData("object", id + ":title");
        }
    }
}
