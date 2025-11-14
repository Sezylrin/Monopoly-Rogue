using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEffect : MonoBehaviour
{
    protected Building currentBuilding;
    // Start is called before the first frame update

    public virtual void ApplyEffect()
    {

    }

    public virtual void RemoveEffect()
    {
    }

    [CollapsibleGroup("Modifiers", 100)]
    [SerializeField]
    protected int value, range;
    [SerializeField]
    protected float multiplier;
    protected void ModifyValue(bool remove = false)
    {
        float final = remove ? -value : value;
        currentBuilding?.ModifyValue(final);
    }
    protected void ModifyMultiplier(bool remove = false)
    {
        float final = remove ? -multiplier : multiplier;
        currentBuilding?.ModifyMultiplier(final);
    }

    protected void ModifyRange(bool remove = false)
    {
        float final = remove ? -range : range;
        currentBuilding?.ModifyRange(final);
    }
}
