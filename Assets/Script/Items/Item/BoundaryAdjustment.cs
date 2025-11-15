using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryAdjustment : BaseItemTile
{
    protected override bool CantUseItem()
    {
        return TileIsProtected() || TileHasNoBuilding();
            
    }
    protected override void ShouldOpenTileUICondition(bool shouldOpen, out bool found)
    {
        found = false;
        for (int i = -2; i <= 2; i++)
        {
            if (i == 0 || TileIsProtected(i))
                continue;
            found = true;
            int pos = (currentPosSO.Int - i + gridSizeSO.Int) % gridSizeSO.Int;
            ModifyUI(shouldOpen, pos);
        }
    }
}
