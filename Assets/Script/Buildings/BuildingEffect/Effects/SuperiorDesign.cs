using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperiorDesign : BuildingEffect
{
    public override void ApplyEffect()
    {
        ModifyValue();
    }

    public override void RemoveEffect()
    {
        ModifyValue(true);
    }
}
