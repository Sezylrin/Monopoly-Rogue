using AYellowpaper.SerializedCollections;
using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
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
                ResetObjects();
            }
        }
        private static void ResetObjects()
        {
            string assetPath = "Assets/ScriptableObject/Types/ResetterSO.asset";

            ResetterOBJ contentsRoot = AssetDatabase.LoadAssetAtPath<ResetterOBJ>(assetPath) as ResetterOBJ;
            if (contentsRoot == null)
            {
                Debug.Log("Resetter Obj not found, if location changed please change string path");
                return;
            }
            foreach (ITypeCanReset obj in contentsRoot.ScriptableObjectsToReset)
                obj.ResetValue();

            Debug.Log("Resetted Object");
            AssetDatabase.SaveAssets();
        }
    } 
#endif
}
