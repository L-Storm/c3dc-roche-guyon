using UnityEngine;
using System.Collections.Generic;
using System;

public class Manager : MonoBehaviour {
    private static Manager s_current = null;
    public static Manager Current{
        get {
            if (s_current == null) {
                s_current = GameObject.FindObjectOfType <Manager>();
                if (s_current == null){
                        throw new Exception("There is no Manager in the scene");
                    }
            }
            return s_current;
        }
    }

    // Instectable objects are on a dictonary with its id and its mode
    [HideInInspector] public Dictionary<GameObject,Reference> _dictionary = new Dictionary<GameObject, Reference>();

	// Use this for initialization
	void Start () {
        // Catch all gameObjects on the layer InspectableObjects
        GameObject[] _objectsArray = FindGameObjectsWithLayer(LayerMask.NameToLayer("InspectableObjects"));
        Debug.Log("Nombre d'object inspectables : " + _objectsArray.Length);
        
        // Add all these objects on the disctionary
        for (int i = 0; i< _objectsArray.Length; i++){
            _dictionary.Add(_objectsArray[i], _objectsArray[i].GetComponent<Parameters>()._ref);
        }

	}
	
	// Update is called once per frame
	void Update () { }

    public GameObject[] FindGameObjectsWithLayer(LayerMask layer)
    {
        GameObject[] goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        var goList = new List<GameObject>();
        for (int i = 0; i< goArray.Length; i++)
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

public enum Mode { Rotate, Plane };

public class Reference {

    public string _id;
    public Mode _mode;

    public Reference(string id, Mode mode) {
        _id = id;
        _mode = mode;
    }
    ~Reference() {}
}

