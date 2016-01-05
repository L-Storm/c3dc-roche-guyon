/// <summary>
/// Author: Sonia SEDDIKI
/// Copyright (c) Val'EISTI - 2015
/// Inspector state allows the player to observe an inspectable _object in detail, with additional information and focus on the _object.
/// </summary>
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Inspector : State
{
    // Parameters 
    private GameObject _obj = StateManager.Instance._inspectableObject;
    private GameObject _camGo = StateManager.Instance._camInspectorGO;
    private GameObject _fps = StateManager.Instance._fpsGO;
    private GameObject _FPSController = GameObject.Find("FPSController");
    private Camera _cam = StateManager.Instance._camInspector;

    private Vector3 _posRef;

    private float _upSize;
    private float _rightSize;

    private float distance = 0;

    private bool _initialized = false;


    // Constructor
    public Inspector() : base()
    {
        _panel = GameObject.Find("StatePanels").GetComponentsInChildren<Transform>(true)[5].gameObject;
        Debug.Log("Panel INSPECTOR initialized");
    }

    public override void trigger()
    {
        // TODO: define Inspector mode behaviour
        Debug.Log("Launching Explorator mode");

        // Reset the object position on the scene;
        _obj.transform.position = _user._inspectablePos;
        _obj.transform.rotation = _user._inspectableRot;

        // Reset the Inspector mode
        _initialized = false;

        // Re-change the cameras
        _FPSController.SetActive(true);
        _fps.SetActive(true);
        _camGo.SetActive(false);
        // Deactivate the panel
        _panel.SetActive(false);
        // Change the mode
        _user.SetState(_user.EXPLORATOR);
    }

    public override void behave()
    {
        // Activate the panel
        _panel.SetActive(true);
        Debug.Log("I'm the Inspector mode!");
        _obj = _user._inspectableObject;

        // Initialize Camerass
        if (!_initialized)
        {
            SetCamera(_obj);
            _initialized = true;
        }

        // Behave : moves 
        if (_user._dictionary[_obj]._mode == Mode.Rotate)
        {
            RotateCenter();
            Zoom();
        }
        
       if (_user._dictionary[_obj]._mode == Mode.Plane)
        {
            TranslatePlane();
            Zoom();
        }

    }

    public override void GUI()
    {
        if (_panel.activeSelf)
        {
            string id = _user._inspectableObject.GetComponent<Parameters>()._id;
            GameObject.Find("StatePanels/Inspector/Title").GetComponent<Text>().text = Database.Instance.GetData("object", id + ":title");
            GameObject.Find("StatePanels/Inspector/Description").GetComponent<Text>().text = Database.Instance.GetData("object", id + ":desc");
        }
    }

    /// <summary>
    /// Rotation for object with 2 degrees of freedom or less on rotation. </summary>
    /// <remarks>
    /// Object remains immobile, Inspector Camera rotates around it. </remarks>
    public void Rotate360()
    {
        // Rotation is only set around the up vector only
        float turnSpeed = 20;
        _camGo.transform.LookAt(_obj.transform );

        if (Input.GetAxis("Horizontal") == -1)
            _camGo.transform.RotateAround(_obj.transform.position, _obj.transform.up, turnSpeed * Time.deltaTime);

        if (Input.GetAxis("Horizontal") == 1)
            _camGo.transform.RotateAround(_obj.transform.position, -_obj.transform.up, turnSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Rotation for objects with 3 degrees of freedom on rotation. </summary>
    /// <remarks>
    /// Object rotates around its center, Inspector Camera remains immobile. </remarks>
    public void RotateCenter()
    {
        float turnSpeed = 20;
        //_camGo.transform.LookAt(_obj.transform);
       

        if (Input.GetAxis("Vertical") == -1)
            _obj.transform.Rotate(-_camGo.transform.right, turnSpeed * Time.deltaTime, Space.World);

        if (Input.GetAxis("Vertical") == 1)
            _obj.transform.Rotate(_camGo.transform.right, turnSpeed * Time.deltaTime, Space.World);

        if (Input.GetAxis("Horizontal") == -1)
            _obj.transform.Rotate(_camGo.transform.up, turnSpeed * Time.deltaTime, Space.World);

        if (Input.GetAxis("Horizontal") == 1)
            _obj.transform.Rotate(-_camGo.transform.up, turnSpeed * Time.deltaTime, Space.World);
    }

    /// <summary>
    /// Translation on plane surfaces. </summary>
    /// <remarks>
    /// Camera translates along the horizontal and vertical axis of the plane. No rotation for the camera. </remarks>
    public void TranslatePlane()
    {
        Vector3 diff =  _camGo.transform.position - _posRef;
        float rightDistance = Vector3.Dot(diff, _camGo.transform.right) / Vector3.Magnitude(_camGo.transform.right);
        float upDistance = Vector3.Dot(diff, _camGo.transform.up) / Vector3.Magnitude(_camGo.transform.up);

        if (Input.GetAxis("Vertical") == -1 && upDistance > -_upSize*0.25f)
            _camGo.transform.position -= 0.02f*_obj.transform.forward;

        if (Input.GetAxis("Vertical") == 1 && upDistance < _upSize * 0.25f)
            _camGo.transform.position += 0.02f * _obj.transform.forward;

        if (Input.GetAxis("Horizontal") == -1 && rightDistance > -_rightSize * 0.25f)
            _camGo.transform.position -= 0.02f * _obj.transform.right;

        if (Input.GetAxis("Horizontal") == 1 && rightDistance < _rightSize * 0.25f)
            _camGo.transform.position += 0.02f * _obj.transform.right;
    }

    /// <summary>
    /// Translation on concave surfaces. </summary>
    /// <remarks>
    /// Camera translates along the horizontal and vertical axis of the plane. Rotates also proportionally to the translation. </remarks>
    public void TranslateConcave()
    {
        // TODO: set InspectorCamera ready for inspection
        // TODO: define behaviour
    }

    public void SetCamera(GameObject obj)
    {
        // Local variables
        Bounds bound = CalculateBound(obj);
                        
        float fovRadHalf = _cam.fieldOfView * (Mathf.PI / 360);
               
        // Activate the Inspector Camera
        _fps.SetActive(false);
        _FPSController.SetActive(false);
        _camGo.SetActive(true);
        
        // Prepare for plane moves
        if (_user._dictionary[_obj]._mode == Mode.Plane)
        {
            // Set InspectorCamera ready for inspection
            distance = Mathf.Abs(bound.extents.x / (4 * Mathf.Tan(fovRadHalf)));
            _camGo.transform.forward = -_obj.transform.up;
            _camGo.transform.position = _obj.transform.up * distance + _obj.transform.position;
            _upSize = bound.extents.z; ;
            _rightSize = bound.extents.x;
        }

        // Prepare for a rotation
        if (_user._dictionary[_obj]._mode == Mode.Rotate)
        {
            // Set InspectorCamera ready for inspection
            _obj.transform.position += _obj.transform.up ;
            distance = 1.2f * Mathf.Abs(bound.extents.y / Mathf.Tan(fovRadHalf));
            _camGo.transform.position = _fps.transform.position;
            Vector3 direction = Vector3.Normalize(_camGo.transform.position - _obj.transform.position);
            _camGo.transform.position = _obj.transform.position + distance * direction;
            _camGo.transform.forward = direction;
            _camGo.transform.LookAt(_obj.transform.position + 0.30f * _obj.transform.right);
        }

        _posRef = _camGo.transform.position;

    }

    public void Zoom()
    {
        // Field of View (degree)
        float fov = _cam.fieldOfView ;

        // Zoom out
        if ((Input.GetAxis("Mouse Y") == -1) && fov < 60 )
        {
            _cam.fieldOfView++;
        }
        // Zoom in
        if (Input.GetAxis("Mouse Y") == 1 && fov > 15)
        {
            _cam.fieldOfView--;
        }

    }

    public Bounds CalculateBound(GameObject obj) {
        if (obj.GetComponent<MeshFilter>() != null)
        {
            MeshFilter meshF = obj.GetComponent<MeshFilter>();
            return meshF.mesh.bounds;
        }
        else if (obj.GetComponent<BoxCollider>() != null)
        {
            BoxCollider collider = obj.GetComponent<BoxCollider>();
            return collider.bounds;
        }
        else if (obj.GetComponent<MeshCollider>() != null) {
            MeshCollider meshCollider = obj.GetComponent<MeshCollider>();
            return meshCollider.bounds;
        }
        throw new System.Exception("Pas de collider");
    }
}
