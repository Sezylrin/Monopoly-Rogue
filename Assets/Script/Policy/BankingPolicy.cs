using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankingPolicy : BasePolicy
{
    [SerializeField]
    private EventSO OnTurnChangeSO;
    [SerializeField]
    private int SellIncrease;

    protected override void Start()
    {
        base.Start();
        OnTurnChangeSO.onEventTrigger += IncreaseSellValue;
    }

    private void IncreaseSellValue(object sender, EventArgs e)
    {
        sellPrice += SellIncrease;
    }
}
