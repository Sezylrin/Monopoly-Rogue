using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbabilityItem : BaseItem
{
    [SerializeField]
    private BoolSO IsOpenProbabilityUiSO;
    [SerializeField]
    private IntSO probabilityIncreaseSO;

    private void OnDisable()
    {
        IsOpenProbabilityUiSO.onValueChanged -= ItemUsed;
    }
    public override void AttemptItemUse()
    {
        probabilityIncreaseSO.Int = (itemSO as ProbabilityItemSO).amount;
        IsOpenProbabilityUiSO.Bool = true;
        IsOpenProbabilityUiSO.onValueChanged += ItemUsed;
    }
    private void ItemUsed(object sender, EventArgs e)
    {
        if (!IsOpenProbabilityUiSO.Bool)
            base.ItemUseSuccessful();
    }
}
