using KevinCastejon.MissingFeatures.MissingAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TypeSO<T> : BaseTypeSO
{

    public virtual EventHandler onValueChanged { get; set; }

    protected virtual void OnValidate()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.Log("Manual value changed and invoked");
            onValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnDisable()
    {
        if (EditorApplication.isPlaying)
            Debug.Log(name + " OnDisabled Triggered");
        onValueChanged = null;
    }
}

public class BaseTypeSO : ScriptableObject
{
    [CollapsibleGroup("Description",100)]
    [TextArea(3,10)]
    public string Descirption;

}

public class ResetableTypeSO<T> : TypeSO<T>, ITypeSO<T>, ITypeCanReset
{
    [field:CollapsibleGroup("Reset Value", 99),SerializeField]
    public bool ShouldReset { get; set; }
    [field:SerializeField]
    public T defaultValue { get; set; }
    /// <summary>
    /// Reset value without invoking event
    /// </summary>
    public virtual void ResetValue()
    {

    }
#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        string assetPath = "Assets/ScriptableObject/Types/ResetterSO.asset";
        ResetterOBJ contentsRoot = AssetDatabase.LoadAssetAtPath<ResetterOBJ>(assetPath) as ResetterOBJ;
        if(contentsRoot == null)
            return;
        if (ShouldReset)
        {
            if (!contentsRoot.ScriptableObjectsToReset.Contains(this))
            {
                contentsRoot.ScriptableObjectsToReset.Add(this);
            }
        }
        else
        {
            contentsRoot.ScriptableObjectsToReset.Remove(this);
        }

        EditorUtility.SetDirty(contentsRoot);
    }
#endif
}
