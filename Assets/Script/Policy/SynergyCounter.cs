using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyCounter : BasePolicy
{
    [SerializeField]
    private BoolSO IsCollectCash;
    [SerializeField]
    private FloatSO MoneyEarnedSO;
    [SerializeField]
    private int moneyPerSynergy;
    protected override void Start()
    {
        base.Start();
        IsCollectCash.onValueChanged += EarnMoney;
    }

    private void EarnMoney(object sender, EventArgs e)
    {
        int total = 0;
        foreach (Tile tile in TileGrid.Instance.GetTile())
        {
            if(tile.GetCurrentBuilding() && tile.GetCurrentBuilding().IsSynergyActive())
                total++;
        }
        MoneyEarnedSO.Float += moneyPerSynergy * total;
    }

    public override void RemoveEffect()
    {
        IsCollectCash.onValueChanged -= EarnMoney;
    }
}
