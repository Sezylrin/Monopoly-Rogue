using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothEffect : TileEffect
{
    public override void ApplyEffect()
    {
        ModifyRange();
    }

    public override void RemoveEffect()
    {
        ModifyRange(true);
    }
}
