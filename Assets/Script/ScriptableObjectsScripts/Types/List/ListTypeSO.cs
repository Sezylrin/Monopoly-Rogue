using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEditor;
using UnityEngine;

public class ListTypeSO<T> : TypeSO<T>, ITypeCanReset, IEnumerable<T>
{
    [CollapsibleGroup("List")]
    [SerializeField]
    private List<T> list = new List<T>();
    public int Count { get { return list.Count; } }

    public T this[int key]
    {
        get { return list[key]; }
        set { list[key] = value; ValueChanged();  }
    }
    public void Add(T item)
    {
        list.Add(item);
        ValueChanged();
    }
    public void Remove(T item)
    {
        list.Remove(item);
        ValueChanged();
    }
    public void Clear()
    {
        list.Clear();
        ValueChanged();
    }
    public bool Contains(T item)
    {
        return list.Contains(item);
    }
    public void RemoveAt(int index)
    {
        list.RemoveAt(index);
        ValueChanged();
    }
    public void ValueChanged()
    {
        onValueChanged.Invoke(this, EventArgs.Empty);
    }

    [field: CollapsibleGroup("Reset Value", 99), SerializeField]
    public bool ShouldReset { get; set; }
    public void ResetValue()
    {
        list.Clear();
    }
#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        string assetPath = "Assets/ScriptableObject/Types/ResetterSO.asset";
        ResetterOBJ contentsRoot = AssetDatabase.LoadAssetAtPath<ResetterOBJ>(assetPath) as ResetterOBJ;
        if (contentsRoot == null)
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
    public IEnumerator<T> GetEnumerator()
    {
        return ((IEnumerable<T>)list).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)list).GetEnumerator();
    }
}
