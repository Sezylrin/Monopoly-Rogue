using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisasterPrevention : BuildingEffect
{
    public override void ApplyEffect()
    {
        ModifyValue();
        ModifyMultiplier();
    }

    public override void RemoveEffect()
    {
        ModifyValue(true);
        ModifyMultiplier(true);
    }
}
