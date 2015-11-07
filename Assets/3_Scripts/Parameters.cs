using UnityEngine;
using System.Collections;

public class Parameters : MonoBehaviour {

    [HideInInspector] public string _id;
    [HideInInspector] public Mode _mode;
    [HideInInspector] public Reference _ref = new Reference(null, Mode.Rotate);

	// Use this for initialization
	void Start () {
        _ref = new Reference(_id, _mode);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
