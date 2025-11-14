using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duplicate : BaseItem
{
    [SerializeField]
    private IntSO currentPosSO;
    [SerializeField]
    private IntSO gridSizeSO;
    [SerializeField]
    private BoolSO IsDisableButtonSO;
    public override void AttemptItemUse()
    {
        bool cancel = TileGrid.Instance.IsTileProtected();
        if (cancel)
        {
            base.ItemUseCancel();
            return;
        }
        bool found = false;
        for (int i = 0; i < gridSizeSO.Int; i++)
        {
            cancel = i == currentPosSO.Int || !TileGrid.Instance.GetBuildingOnTile(i) || TileGrid.Instance.IsNotAtLimit(TileGrid.Instance.GetBuildingOnTile(i).GetSO());
            if (cancel)
                continue;
            found = true;
            TileGrid.Instance.EnableTileButtonUIIgnoreProtection(i, ButtonAction.duplicate);
        }
        if (!found)
        {
            base.ItemUseCancel();
            return;
        }
        IsDisableButtonSO.Bool = true;
    }
    private void OnEnable()
    {
        IsDisableButtonSO.onValueChanged += ItemUsed;
    }
    private void OnDisable()
    {
        IsDisableButtonSO.onValueChanged -= ItemUsed;
    }
    private void ItemUsed(object sender, EventArgs e)
    {
        if (IsDisableButtonSO.Bool)
            return;
        for (int i = 0; i < gridSizeSO.Int; i++)
        {
            if (i == currentPosSO.Int)
                continue;
            int pos = (currentPosSO.Int - i + gridSizeSO.Int) % gridSizeSO.Int;
            TileGrid.Instance.DisableTileButtonUI(pos, ButtonAction.duplicate);
        }
        base.ItemUseSuccessful();
    }
}
