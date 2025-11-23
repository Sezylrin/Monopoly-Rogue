using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BasicItemLottery : BasePolicy
{
    [SerializeField]
    private EventSO OnTurnChangeSO;
    [SerializeField]
    private int percentPerBuilding;
    [SerializeField]
    private CalculateRarityListSO CalculateAllRarity;
    [SerializeField]
    private RaritySO itemLotteryRaritySO;
    [SerializeField]
    private BoolSO IsAttemptGenerate;
    protected override void Initialise()
    {
        OnTurnChangeSO.onEventTrigger += AttemptItemGenerate;
    }

    private void AttemptItemGenerate(object sender, EventArgs e)
    {
        int percent = percentPerBuilding * TileGrid.Instance.GetTotalBuilding();
        if (Random.Range(0,100) <  percent)
        {
            itemLotteryRaritySO.Rarity = CalculateAllRarity.CalculateRarity();
            IsAttemptGenerate.Bool = true;
        }
    }

}
