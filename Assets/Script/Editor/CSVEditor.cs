using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(CSVIO))]
public class CSVEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CSVIO manager = (CSVIO)target;
        base.OnInspectorGUI();
        GUILayout.Space(10f);
        if (GUILayout.Button("Export To CSV"))
        {
            manager.GenerateCSV();
        }

        if (GUILayout.Button("Import From CSV"))
        {
            manager.ReadCSV();
        }
    }
}
    
