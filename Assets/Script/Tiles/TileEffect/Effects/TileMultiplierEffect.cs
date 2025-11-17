using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMultiplierEffect : TileEffect
{
    [SerializeField]
    private float tileMultiplierAmount;
    public override void ApplyEffect()
    {
        currentTile.ModifyMultiplier(tileMultiplierAmount);
        base.ApplyEffect();
    }

    public override void RemoveEffect()
    {
        currentTile.ModifyMultiplier(-tileMultiplierAmount);
        base.RemoveEffect();
    }
}
