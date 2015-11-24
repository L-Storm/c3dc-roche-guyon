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

    private int _countdown;
    private State _state;
    private static StateManager _userInstance;

    // Constructor
    private StateManager() {}

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
        EXPLORATOR = new Explorator();
        INSPECTOR = new Inspector();
        SLEEP = new Sleep();

        _state = Instance.EXPLORATOR;
    }

    public void Update()
    {
        if (Input.GetKeyDown("i")) 
        {
            _state.trigger();
        }
        _state.behave();
    }

    public void OnGUI()
    {
        _state.GUI();
    }

    public void SetState(State state)
    {
        _state = state;
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
        //TODO: define Sleep mode behaviour
        Debug.Log("Sleep mode activated");
        _user.SetState(_user.SLEEP);
    }

}
