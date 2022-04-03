using UnityEngine;
using System.Collections;
using System.Diagnostics;
using UnityEditor;
using Debug = UnityEngine.Debug;

[CustomEditor(typeof(WorldManager))]
public class WorldManagerInspector : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(20);
        WorldManager myScript = (WorldManager)target;
        if(GUILayout.Button("Generate"))
        {
            myScript.Generate();
        }
    }
}
