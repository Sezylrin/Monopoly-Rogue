using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duplicate : BaseItemTile
{    
    protected override void ShouldOpenTileUICondition(bool shouldOpen, out bool found)
    {
        found = false;
        for (int i = 0; i < gridSizeSO.Int; i++)
        {
            if (IsCurrentPos(i) || TileHasNoBuilding(i) || BuildingAtMaxLimit(i))
                continue;
            found = true;
            ModifyUI(shouldOpen, i);
        }
    }    
}
