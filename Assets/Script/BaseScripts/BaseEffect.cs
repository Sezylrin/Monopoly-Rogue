using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEffect : MonoBehaviour
{
    protected Building currentBuilding;
    // Start is called before the first frame update

    public virtual void ApplyEffect()
    {
        ModifyValue();
        ModifyMultiplier();
        ModifyRange();
    }

    public virtual void RemoveEffect()
    {
        ModifyValue(true);
        ModifyMultiplier(true);
        ModifyRange(true);
    }

    [CollapsibleGroup("Modifiers", 100)]
    [SerializeField]
    protected int value, range;
    [SerializeField]
    protected float multiplier;
    protected void ModifyValue(bool remove = false)
    {
        if (value == 0)
            return;
        float final = remove ? -value : value;
        currentBuilding?.ModifyValue(final);
    }
    protected void ModifyMultiplier(bool remove = false)
    {
        if (multiplier == 0)
            return;
        float final = remove ? -multiplier : multiplier;
        currentBuilding?.ModifyMultiplier(final);
    }

    protected void ModifyRange(bool remove = false)
    {
        if (range == 0)
            return;
        float final = remove ? -range : range;
        currentBuilding?.ModifyRange(final);
    }
}
