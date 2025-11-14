using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryAdjustment : BaseItem
{
    [SerializeField]
    private IntSO currentPosSO;
    [SerializeField]
    private IntSO gridSizeSO;
    [SerializeField]
    private BoolSO IsDisableButtonSO;
    // Start is called before the first frame update
    public override void AttemptItemUse()
    {
        bool cancel = (TileGrid.Instance.GetBuildingOnTile(currentPosSO.Int) == null) 
            || (TileGrid.Instance.IsTileProtected());
        if (cancel)
        {
            base.ItemUseCancel();
            return;
        }
        for (int i = -2; i <= 2; i++)
        {
            if (i == 0)
                continue;
            int pos = (currentPosSO.Int - i + gridSizeSO.Int) % gridSizeSO.Int;
            TileGrid.Instance.EnableBoundaryAdjustmentUI(pos);
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
        for (int i = -2; i <= 2; i++)
        {
            if (i == 0)
                continue;
            int pos = (currentPosSO.Int - i + gridSizeSO.Int) % gridSizeSO.Int;
            TileGrid.Instance.DisableBoundaryAdjustmentUI(pos);
        }
        base.ItemUseSuccessful();
    }
}
