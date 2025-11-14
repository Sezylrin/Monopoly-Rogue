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
    private void OnEnable()
    {
        IsOpenBuildMenuUI.onValueChanged += ItemUsed;
    }

    private void OnDisable()
    {
        IsOpenBuildMenuUI.onValueChanged -= ItemUsed;
    }
    public override void AttemptItemUse()
    {
        if (TileGrid.Instance.IsTileProtected())
        {
            
            base.ItemUseCancel();
            return;
        }
        developmentRarity.Rarity = itemSO.rarity;
        IsOpenBuildMenuUI.Bool = true;
    }

    private void ItemUsed(object sender, EventArgs e)
    {
        if (IsOpenBuildMenuUI.Bool == false)
            base.ItemUseSuccessful();
    }
}
