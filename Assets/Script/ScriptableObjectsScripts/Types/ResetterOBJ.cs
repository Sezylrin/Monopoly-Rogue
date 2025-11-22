using AYellowpaper.SerializedCollections;
using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "ResetterSO", menuName = "ScriptableObjects/Types/ResetterSO")]

public class ResetterOBJ : ScriptableObject
{
    public List<ScriptableObject> ScriptableObjectsToReset = new List<ScriptableObject>();

    #if UNITY_EDITOR
    [InitializeOnLoad]
    public static class PlayModeStateChangedExample
    {
        // register an event handler when the class is initialized
        static PlayModeStateChangedExample()
        {
            EditorApplication.playModeStateChanged += LogPlayModeState;
        }

        private static void LogPlayModeState(PlayModeStateChange state)
        {
            if (state.Equals(PlayModeStateChange.EnteredEditMode))
            {
                ResetterOBJ.ResetObjects();
            }
        }
    }
    [ContextMenu("Reset Objects")]
    public static void ResetObjects()
    {
        string assetPath = "Assets/ScriptableObject/Types/ResetterSO.asset";

        ResetterOBJ contentsRoot = AssetDatabase.LoadAssetAtPath<ResetterOBJ>(assetPath) as ResetterOBJ;
        if (contentsRoot == null)
        {
            Debug.Log("Resetter Obj not found, if location changed please change string path");
            return;
        }
        foreach (ITypeCanReset obj in contentsRoot.ScriptableObjectsToReset)
        {
            if(obj == null)
            {
                Debug.Log("Ressetter Broke, failed to reset");
                return;
            }
            obj.ResetValue();
        }
        contentsRoot.GenerateCSV();
        Debug.Log("Resetted Object");
        AssetDatabase.SaveAssets();
    }

    private string filename = "";

    [ContextMenu("GenerateCSV")]
    public void GenerateCSV()
    {
        filename = Application.dataPath + "/SOToReset.csv";
        WriteCSV();
    }
    private void WriteCSV()
    {
        if (ScriptableObjectsToReset.Count <= 0)
            return;
        TextWriter tw = new StreamWriter(filename, false);
        tw.WriteLine("Slot,Description");
        for(int i = 0; i < ScriptableObjectsToReset.Count; i++) 
        {
            string final =  i.ToString() + ",";
            final += ScriptableObjectsToReset[i].name;
            tw.WriteLine(final);
        }
        tw.Close();
        Debug.Log("CSV Generated");
    }
#endif


}
