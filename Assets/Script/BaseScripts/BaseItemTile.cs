using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class BaseItemTile : BaseItem
{
    [SerializeField]
    protected IntSO currentPosSO;
    [SerializeField]
    protected IntSO gridSizeSO;
    [SerializeField]
    protected BoolSO IsDisableButtonSO;
    [SerializeField]
    protected ButtonAction action;
    public override void AttemptItemUse()
    {
        if (CantUseItem())
        {
            ItemUseCancel();
            return;
        }
        bool found = false;
        ShouldOpenTileUICondition(true,out found);
        if (!found)
        {
            ItemUseCancel();
            return;
        }
        IsDisableButtonSO.Bool = true;
        IsDisableButtonSO.onValueChanged += ItemUsed;
    }
    protected virtual bool CantUseItem()
    {
        return TileIsProtected();
    }
    protected bool IsCurrentPos(int pos)
    {
        return pos == currentPosSO.Int;
    }
    protected virtual void ShouldOpenTileUICondition(bool shouldOpen, out bool found)
    {
        found = false;
    }
    protected void ModifyUI(bool value, int i)
    {
        if (value)
            TileGrid.Instance.EnableTileButtonUI(i, action);
        else
            TileGrid.Instance.DisableTileButtonUI(i, action);
    }
    protected void OnDisable()
    {
        IsDisableButtonSO.onValueChanged -= ItemUsed;
    }
    protected void ItemUsed(object sender, EventArgs e)
    {
        if (IsDisableButtonSO.Bool)
            return;
        ShouldOpenTileUICondition(false, out bool found);
        ItemUseSuccessful();
    }
}
