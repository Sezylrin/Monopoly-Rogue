using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transfer : BaseItemTile
{
    protected override bool CantUseItem()
    {
        return TileIsProtected() || TileHasNoBuilding();
    }
    protected override void ShouldOpenTileUICondition(bool shouldOpen, out bool found)
    {
        found = false;
        for (int i = 0; i < gridSizeSO.Int; i++)
        {
            if (IsCurrentPos(i) || !BuildingContainsEffects(i))
                continue;
            found = true;
            ModifyUI(shouldOpen, i);
        }
    }
}
