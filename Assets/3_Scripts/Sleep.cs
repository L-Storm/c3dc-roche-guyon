/// <summary>
/// Author: Sonia SEDDIKI
/// Copyright (c) Val'EISTI - 2015
/// Sleep state launches an animation for the camera in order to have automatic moves inside the room.
/// </summary>
using UnityEngine;
using System.Collections;

public class Sleep : State
{
    // Constructor
    public Sleep() : base() { }

    public override void trigger()
    {
        // Nothing else to add!
        Debug.Log("Back to Explorator mode");
        _user.SetState(_user.EXPLORATOR);
    }

    public new void timeout()
    {
        // Nothing else to add! Redefines the State timeout method since we don't want anything for Sleep mode
    }

    public override void behave()
    {
        Debug.Log("I'm the Sleep mode!");
    }

    public override void GUI()
    {

    }
}


//public class Sleep : MonoBehaviour
//{

//    public float _timerSeconds;
//    public GameObject _animatedCam;
//    public static bool _sleep;
//    public GameObject _sleepMessage;

//    private float _currentTime;
//    private Vector3 _animStartPosition;


//    // Use this for initialization
//    void Start()
//    {
//        _currentTime = _timerSeconds;
//        _animStartPosition = _animatedCam.transform.position;
//        _sleep = false;
//        _sleepMessage.SetActive(false);
//    }

//    // Update is called once per frame
//    void FixedUpdate()
//    {

//        // Restart the countdown
//        if (Input.anyKey)
//        {
//            _currentTime = _timerSeconds;
//            _animatedCam.GetComponent<Animation>().Stop();
//            StopAllCoroutines();
//            _sleepMessage.SetActive(false);
//            _sleep = false;
//        }
//        // Decrease the timer
//        if (!Input.anyKey && _currentTime > 0)
//        {
//            _currentTime -= Time.deltaTime;
//        }
//        // Start Sleep Mode
//        if (_currentTime <= 0)
//        {
//            _sleep = true;
//            _sleepMessage.SetActive(true);
//            if (Inspector.Instance().Activated)
//            {
//                Inspector.Instance().RestoreExplorationMode();
//                // Set manually the value to false because the only way to change the value in the inspector script is to press the "Inspecter" button.
//                Inspector.Instance().Activated = false;
//            }
//            StartCoroutine(MoveThenAnimate());
//        }
//    }

//    IEnumerator MoveToStart()
//    {
//        do
//        {
//            _animatedCam.transform.position = Vector3.Lerp(_animatedCam.transform.position, _animStartPosition, Time.deltaTime);
//            yield return new WaitForSeconds(1);
//        }
//        while (Vector3.Distance(_animatedCam.transform.position, _animStartPosition) > 0.5f);
//        yield return new WaitForSeconds(2);
//    }

//    IEnumerator PlayAnimation()
//    {
//        _animatedCam.GetComponent<Animation>().Play();
//        yield return null;
//    }

//    IEnumerator MoveThenAnimate()
//    {
//        yield return StartCoroutine(MoveToStart());
//        yield return StartCoroutine(PlayAnimation());
//    }
//}



