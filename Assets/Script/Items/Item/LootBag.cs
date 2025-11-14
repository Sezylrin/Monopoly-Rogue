using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : BaseItem
{
    public override void AttemptItemUse()
    {
        base.ItemUseSuccessful();
        manager.GenerateMultipleItem(itemSO.rarity, (itemSO as LootBagItemSO).amount, itemSO);
        
    }
}
