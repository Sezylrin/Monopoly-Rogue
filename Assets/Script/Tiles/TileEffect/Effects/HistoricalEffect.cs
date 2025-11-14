using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoricalEffect : TileEffect
{
    public override void ApplyEffect()
    {
        currentTile.ModifyMultiplier(1);
    }

    public override void RemoveEffect()
    {
        currentTile.ModifyMultiplier(-1);
    }
}
