using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewLandPolicy : BasePolicy
{
    [SerializeField]
    private EventSO OnCollectCashSO;
    [SerializeField]
    private FloatSO MoneyEarned;
    [SerializeField]
    private IntSO GridSizeSO;
    [SerializeField]
    private int amountPerEmptyLand;
    protected override void Start()
    {
        base.Start();
        OnCollectCashSO.onEventTrigger += EarnMoney;
    }

    private void EarnMoney(object sender, EventArgs e)
    {
        MoneyEarned.Float += (GridSizeSO.Int - TileGrid.Instance.GetTotalBuilding()) * amountPerEmptyLand;
    }
}
