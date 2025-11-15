using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingEffectItem : BaseItem
{
    public override void AttemptItemUse()
    {
        if (TileGrid.Instance.AddBuildingEffect((itemSO as PFItemSO).objectPF))
        {
            ItemUseSuccessful();
        }
        else
        {
            ItemUseCancel();
        }
    }
}
