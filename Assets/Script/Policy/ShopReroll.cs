using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopReroll : BasePolicy
{
    [SerializeField]
    private BoolSO IsFreeShopReroll;

    public override void ApplyEffect()
    {
        IsFreeShopReroll.Bool = true;
    }

    public override void RemoveEffect()
    {
        IsFreeShopReroll.Bool = false;
    }
}
