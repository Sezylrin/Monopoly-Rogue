using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ListWrapper<T> : IEnumerable<T>
{    
    [HideInInspector]
    public string Name;
    [SerializeField]
    private List<T> list;
    public int Count { get { return list.Count; } }
    public ListWrapper()
    {
        list = new List<T>();
    }

    public ListWrapper(string name)
    {
        list = new List<T>();
        Name = name;
    }

    public void Add(T item)
    {
        list.Add(item);
    }

    public void Clear()
    {
        list.Clear();        
    }
    public void RemoveAt (int index)
    {
        list.RemoveAt(index);
    }
    public bool Remove (T item)
    {
        return list.Remove(item);
    }

    public bool Contains (T item)
    {
        return list.Contains(item);
    }

    public T this[int key]
    {
        get { return list[key]; }
        set { list[key] = value; }
    }

    public IEnumerator<T> GetEnumerator()
    {
        return ((IEnumerable<T>)list).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)list).GetEnumerator();
    }
}
