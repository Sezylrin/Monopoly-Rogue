using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BasicItemLottery : BasePolicy
{
    [SerializeField]
    private BoolSO IsTurnChangeSO;
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
        IsTurnChangeSO.onValueChanged += AttemptItemGenerate;
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
