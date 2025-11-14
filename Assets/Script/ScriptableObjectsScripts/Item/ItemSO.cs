using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemSO", menuName = "ScriptableObjects/Items/ItemSO")]

public class ItemSO : ScriptableObject
{

    [field: SerializeField, CollapsibleGroup("Base Fields", 100)]
    public Rarity rarity { get; set; }
    public GameObject prefab;
    [TextArea(3, 10)]
    public string description;
}

