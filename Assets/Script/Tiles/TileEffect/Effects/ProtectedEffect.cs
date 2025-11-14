using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectedEffect : TileEffect
{
    public override void ApplyEffect()
    {
        ModifyValue();
        currentTile.SetProtectionEffect(true);
    }

    public override void RemoveEffect()
    {
        ModifyValue(true);
        currentTile.SetProtectionEffect(false);
    }
}
