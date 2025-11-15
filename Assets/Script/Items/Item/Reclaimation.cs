using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Reclaimation : BaseItem
{
    [SerializeField]
    private BoolSO isRemoveTileEffect;
    public override void AttemptItemUse()
    {
        if (!TileHasEffect())
        {
            ItemUseCancel();
            return;
        }
        isRemoveTileEffect.Bool = true;
        ItemUseSuccessful();
    }
}
