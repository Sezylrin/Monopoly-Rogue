using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevelopmentItem : BaseItem
{
    [SerializeField]
    private BoolSO IsOpenBuildMenuUI;
    [SerializeField]
    private RaritySO developmentRarity;

    private void OnDisable()
    {
        IsOpenBuildMenuUI.onValueChanged -= ItemUsed;
    }
    public override void AttemptItemUse()
    {
        if (TileGrid.Instance.IsTileProtected())
        {
            
            ItemUseCancel();
            return;
        }
        developmentRarity.Rarity = itemSO.rarity;
        IsOpenBuildMenuUI.Bool = true;
        IsOpenBuildMenuUI.onValueChanged += ItemUsed;
    }

    private void ItemUsed(object sender, EventArgs e)
    {
        if (IsOpenBuildMenuUI.Bool == false)
            ItemUseSuccessful();
    }
}
