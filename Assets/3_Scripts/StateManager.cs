
using System.Collections.Generic;
/// <summary>
/// Author: Sonia SEDDIKI
/// Copyright (c) Val'EISTI - 2015
/// This script is the state manager. It describes the different states and how to switch between them.
/// </summary>
using UnityEngine;

/// <summary>
/// Class used to set the states.
/// </summary>
public class StateManager : MonoBehaviour
{
    // States
    public State EXPLORATOR;
    public State INSPECTOR;
    public State SLEEP;

    // Variable to pass inspectable object between states without much coupling.
    public GameObject _inspectableObject = null;
    public Vector3 _inspectablePos;
    public Quaternion _inspectableRot;

    private State _state;
    private static StateManager _userInstance;

    // Cameras : FirstPersonCharacters and the inspector one
    public GameObject _camInspectorGO;
    public Camera _camInspector;
    public GameObject _fpsGO;
    public Camera _fpsCam;
    public GameObject _fpsController;

    private float _countdown = 5;
    private float _currentTime;
    private Animation _anim;

    // Instectable objects are on a dictonary with its id and its mode
    [HideInInspector]
    public Dictionary<GameObject, Reference> _dictionary = new Dictionary<GameObject, Reference>();

    // Constructor
    private StateManager() {}

    // Accessors
    public float Countdown
    {
        get{
            return _countdown;
        }
    }
    public float CurrentTime
    {
        get{
            return _currentTime;
        }
        set {
            _currentTime = value;
        }
    }
    public State CurrentState
    {
        get { return _state; }
    }

    // Single instance of StateManager
    public static StateManager Instance
    {
        get
        {
            if (_userInstance == null)
            {
                _userInstance = GameObject.FindObjectOfType(typeof(StateManager)) as StateManager;
            }
            return _userInstance;
        }
    }

    public void Start() 
    {
        Debug.Log("On Start");

        // Catch all gameObjects on the layer InspectableObjects
        GameObject[] _objectsArray = FindGameObjectsWithLayer(LayerMask.NameToLayer("InspectableObjects"));
        Debug.Log("Nombre d'object inspectables : " + _objectsArray.Length);

        // Add all these objects on the dictionary
        for (int i = 0; i < _objectsArray.Length; i++)
        {
            _dictionary.Add(_objectsArray[i], _objectsArray[i].GetComponent<Parameters>()._ref);
        }

        // Initialize all cameras
        _fpsCam = GameObject.Find("FPSController/FirstPersonCharacter").GetComponent<Camera>();
        _fpsGO = GameObject.Find("FPSController/FirstPersonCharacter");
        _camInspector = GameObject.Find("CameraInspector").GetComponent<Camera>();
        _camInspectorGO = GameObject.Find("CameraInspector");
        _fpsController = GameObject.Find("FPSController");
        Debug.Log("Cameras initialized");


        EXPLORATOR = new Explorator();
        INSPECTOR = new Inspector();
        SLEEP = new Sleep();


        //Initialize state to Explorator
        _state = Instance.EXPLORATOR;

        // Initialize time
        _currentTime = _countdown;
    }

    public void FixedUpdate()
    {
        if (Input.GetKeyDown("i") || (Input.anyKeyDown && _state == Instance.SLEEP))
        {
            _state.trigger();
        }
        _state.behave();
        _state.timeout();
    }

    public void OnGUI()
    {
        _state.GUI();
    }

    public void SetState(State state)
    {
        _state = state;
    }

    public string getState()
    {
        return _state.ToString();
    }

    public GameObject[] FindGameObjectsWithLayer(LayerMask layer)
    {
        GameObject[] goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        var goList = new List<GameObject>();
        for (int i = 0; i < goArray.Length; i++)
        {
            if (goArray[i].layer == layer)
            {
                goList.Add(goArray[i]);
            }
        }
        if (goList.Count == 0)
            return null;
        return goList.ToArray();
    }
}

/// <summary>
/// Class to define a template for every state. Each state inherits from the State class.
/// </summary>
public abstract class State 
{
    // Attributes
    protected StateManager _user;

    // Constructor
    public State()
    {
        _user = StateManager.Instance;
    }

    /// <summary>
    /// Trigger the state change.
    /// </summary>
    public abstract void trigger();

    /// <summary>
    /// What happens when the state is activated.
    /// </summary>
    public abstract void behave();

    /// <summary>
    /// Showing whatever the state needs to show.
    /// </summary>
    public abstract void GUI();

    /// <summary>
    /// What happens when the timer reaches 0 (used to trigger the Sleep mode).
    /// </summary>
    public void timeout()
    {
        // Restart the countdown
        if (Input.anyKey)
        {
            _user.CurrentTime = _user.Countdown;
            // _sleepMessage.SetActive(false);
        }

        // Decrease the timer
        if (!Input.anyKey && _user.CurrentTime > 0)
        {
            _user.CurrentTime -= Time.deltaTime;
        }

        // Trigger Sleep Mode
        if (_user.CurrentTime <= 0)
        {
            if (_user.CurrentState == StateManager.Instance.INSPECTOR)
            {
                _user._camInspectorGO.SetActive(false);
                _user._fpsController.SetActive(true);
            }
            Debug.Log("Entering Sleep Mode...");
            _user.SetState(_user.SLEEP);
        }

    }

}

public enum Mode { Rotate, Plane };

[System.Serializable]
public class Reference
{

    public string _id;
    public Mode _mode;

    public Reference(string id, Mode mode)
    {
        _id = id;
        _mode = mode;
    }
    ~Reference() { }
}