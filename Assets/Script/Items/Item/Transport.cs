using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transport : BaseItemTile
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
            if (IsCurrentPos(i) || TileIsProtected(i))
                continue;
            found = true;
            ModifyUI(shouldOpen, i);
        }
    }
}
