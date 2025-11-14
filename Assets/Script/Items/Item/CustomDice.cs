using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDice : BaseItem
{
    [SerializeField]
    private BoolSO isOpenCustomDiceUiSO;
    private void Start()
    {
    }
    private void OnEnable()
    {
        isOpenCustomDiceUiSO.onValueChanged += ItemUsed;
    }
    private void OnDisable()
    {
        isOpenCustomDiceUiSO.onValueChanged -= ItemUsed;
    }
    public override void AttemptItemUse()
    {
        isOpenCustomDiceUiSO.Bool = true;
    }

    private void ItemUsed(object sender, EventArgs e)
    {
        if(!isOpenCustomDiceUiSO.Bool)
            base.ItemUseSuccessful();
    }
}
