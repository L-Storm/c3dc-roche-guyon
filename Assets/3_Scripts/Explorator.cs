/// <summary>
/// Author: Sonia SEDDIKI
/// Copyright (c) Val'EISTI - 2015
/// Explorator state allows the player to movefreely around the room and to be notified whenever he is close to an inspectable object.
/// </summary>
using UnityEngine;


public class Explorator : State
{
    private Transform _cam;

    // Constructor
    public Explorator() : base()
    {
        _cam = GameObject.Find("FPSController/FirstPersonCharacter").transform;
    }

    public override void trigger()
    {
        // TODO: define Explorator mode behaviour
        Debug.Log("Launching Inspector mode");
        _user.setState(_user.INSPECTOR);
    }

    private GameObject catchObject()
    {
        RaycastHit _hit;

        Debug.DrawRay(_cam.position, _cam.transform.forward * 8, Color.black);
        // Use a rayast to check if an inspectable object is faced by the camera

        int layerMask = 1 << LayerMask.NameToLayer("Inspectable");
        if (Physics.Raycast(_cam.position, _cam.forward, out _hit, 10, layerMask))
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
        Debug.Log("I'm the Explorator mode!");

        bool _catchObject = false;
        GameObject _inspectableObject = null;

        // On récupère le GameObject qui est à inspecter ou null
        _inspectableObject = catchObject();
        // On actualise le marqueur signifiant qu'un object peut être inspecter
        _catchObject = (_inspectableObject == null ? false : true);

        if (_catchObject)
		{
            // The faced GO is inspectable, showing a popup
            Debug.Log("An object is inspectable : " + _inspectableObject.name);
        }
        else
        {
            Debug.Log("Nothing is inspectable");
        }
    }
}
