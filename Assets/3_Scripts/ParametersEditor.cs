using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Parameters))]
public class ParametersEditor : Editor {
    public Parameters _target = null;
    protected Parameters Target {
        get
        {
            if (_target == null)
            {
                _target = target as Parameters;
            }
            return _target;
        }
    }

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();
        GUILayout.BeginVertical();
        GUILayout.Space(15);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Identity : ");
        Target._id = EditorGUILayout.TextField(Target._id, GUILayout.Width(Screen.width /2));
        GUILayout.Space(40);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Mode : ");
        Mode mod = _target._mode;
        mod = (Mode)EditorGUILayout.EnumPopup(mod, GUILayout.Width(Screen.width /2));
        GUILayout.Space(40);
        if (Target._mode != mod)
        {
            Target._mode = mod;
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
}
