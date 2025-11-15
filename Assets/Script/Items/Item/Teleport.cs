using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : BaseItemTile
{
    protected override bool CantUseItem()
    {
        return false;
    }

    protected override void ShouldOpenTileUICondition(bool shouldOpen, out bool found)
    {
        found = true;
        for (int i = 0; i < gridSizeSO.Int; i++)
        {
            if (IsCurrentPos(i))
                continue;
            ModifyUI(shouldOpen, i);
        }
    }
}
