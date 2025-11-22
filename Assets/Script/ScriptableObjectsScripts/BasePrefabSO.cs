using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePrefabSO : ScriptableObject
{
    [CollapsibleGroup("Base Fields", 100)]
    public GameObject prefab;
    [field: SerializeField]
    public Rarity rarity { get; set; }
    public int cost;
    [TextArea(3, 10)]
    public string effect;
}
