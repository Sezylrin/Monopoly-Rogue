using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePolicy : BasePolicy
{
    [SerializeField]
    private BoolSO IsBoolSO;

    public override void ApplyEffect()
    {
        IsBoolSO.Bool = true;
    }

    public override void RemoveEffect()
    {
        IsBoolSO.Bool = false;
    }

}
