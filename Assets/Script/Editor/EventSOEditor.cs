using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(EventSO))]
public class EventSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EventSO eventSO = (EventSO)target;
        base.OnInspectorGUI();
        GUILayout.Space(10f);
        if (GUILayout.Button("Invoke"))
        {
            if (Application.isPlaying)
            {
                eventSO.Invoke();
            }
        }
    }
}
