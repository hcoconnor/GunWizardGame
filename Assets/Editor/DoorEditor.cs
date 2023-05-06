using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Door))]
public class DoorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //DrawDefaultInspector();
        Door doorScript = (Door)target;
        if(GUILayout.Button("Toggle Door"))
        {
            if(doorScript.sr == null || doorScript.bx.Count ==0)
            {
                doorScript.Start();
            }
            doorScript.toggleDoor();
        }

    }
}
