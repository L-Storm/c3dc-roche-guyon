using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Manager))]
public class ManagerEditor : Editor {

    // For a unique Manager script on the scene
    public Manager _target = null;
    protected Manager Target {
        get {
            if (_target == null){
                _target = target as Manager;
                //_objectsCount = _target._dictionary.Count;
            }
            return _target;
        }
    }

    private bool _fold = true;
    //private int _objectsCount;
    private Vector2 _mousePos;

    public override void OnInspectorGUI(){
        DrawDefaultInspector();

        GUILayout.BeginVertical();
        Rect _rectList = GUILayoutUtility.GetRect(new GUIContent("Inspectable Objects"),GUIStyle.none);
        _fold = EditorGUI.Foldout(_rectList, _fold, "Inspectable Objects");
        GUILayout.Space(2);

        if (Event.current.type == EventType.DragUpdated){
            _mousePos = Event.current.mousePosition;
            DragAndDrop.visualMode = _rectList.Contains(_mousePos) ? DragAndDropVisualMode.Generic : DragAndDropVisualMode.Rejected;
        }
        if (Event.current.type == EventType.DragPerform && _rectList.Contains(_mousePos)){
            if (DragAndDrop.objectReferences.Length != 0){
                foreach (Object o in DragAndDrop.objectReferences){
                    if (o.GetType() == typeof(GameObject))
                        Target._dictionary.Add((GameObject)o, new Reference(null,Mode.Plane));
                }
                _fold = true;
                GUIUtility.ExitGUI();
                return;            
            }
        }

        if (_fold){
            GUILayout.BeginHorizontal();
            GUILayout.Space(8);
            GUILayout.BeginVertical();
       
            // Display Inspectable Objects
            foreach(GameObject obj in Target._dictionary.Keys){
                GUILayout.BeginHorizontal();
                GUILayout.Label("Object");
                GameObject o = (GameObject)EditorGUILayout.ObjectField(obj, typeof(GameObject), true, GUILayout.Width(Screen.width / 4));
                if (!Target._dictionary.ContainsKey(o)){
                    Target._dictionary.Add(o, new Reference(null, Mode.Plane));
                }
                GUILayout.Label("Id");
                GUILayout.TextField(obj.GetComponent<Parameters>()._id);
                //Target._dictionary[obj]._id = EditorGUILayout.TextField(Target._dictionary[obj]._id, GUILayout.Width(Screen.width / 4));
                GUILayout.Label("Mode");
                GUILayout.TextField(obj.GetComponent<Parameters>()._mode.ToString());
                //Mode mod = Target._dictionary[obj]._mode;
                //mod = (Mode) EditorGUILayout.EnumPopup(mod, GUILayout.Width(Screen.width/4));
                //if (Target._dictionary[obj]._mode != mod){
                //    Target._dictionary[obj]._mode = mod;
                //}
                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }
}
