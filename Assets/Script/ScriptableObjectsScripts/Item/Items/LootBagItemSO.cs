using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LootBagSO", menuName = "ScriptableObjects/Items/LootBag")]

public class LootBagItemSO : ItemSO
{
    [CollapsibleGroup("LootBag")]
    public int amount;
}
