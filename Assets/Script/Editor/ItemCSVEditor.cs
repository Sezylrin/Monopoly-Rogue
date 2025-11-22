using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(ItemCSVIO))]
public class ItemCSVEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ItemCSVIO manager = (ItemCSVIO)target;
        base.OnInspectorGUI();
        GUILayout.Space(10f);

        if (GUILayout.Button("Import From CSV"))
        {
            manager.ReadCSV();
        }
    }
}
