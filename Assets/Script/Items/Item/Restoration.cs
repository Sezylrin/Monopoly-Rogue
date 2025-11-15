using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restoration : BaseItem
{
    public override void AttemptItemUse()
    {
        if (TileHasNoBuilding() || !BuildingContainsEffects())
        {
            ItemUseCancel();
            return;
        }
        TileGrid.Instance.RemoveAllBuildingEffects();
        ItemUseSuccessful();
    }
}
