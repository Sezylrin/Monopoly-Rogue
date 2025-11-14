using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeritageEffect : TileEffect
{
    public override void ApplyEffect()
    {
        currentTile.GetCurrentBuilding().ModifyMultiplier(1.5f);
    }

    public override void RemoveEffect()
    {
        currentTile.GetCurrentBuilding().ModifyMultiplier(-1.5f);
    }
}
