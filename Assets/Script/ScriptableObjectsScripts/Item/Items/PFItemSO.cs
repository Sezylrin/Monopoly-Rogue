using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PFItemSO", menuName = "ScriptableObjects/Items/PFItemSO")]
public class PFItemSO : ItemSO
{
    [CollapsibleGroup("Prefab Item")]
    public GameObject objectPF;
}
