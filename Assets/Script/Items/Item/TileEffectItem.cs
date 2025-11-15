using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEffectItem : BaseItem
{
    public override void AttemptItemUse()
    {
        if(TileGrid.Instance.AddTileEffect((itemSO as PFItemSO).objectPF))
        {
            ItemUseSuccessful();
        }
        else
        {
            ItemUseCancel();
        }
    }
}
