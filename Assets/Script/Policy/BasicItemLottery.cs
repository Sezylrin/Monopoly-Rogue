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
    private Rarity rarity;
    [SerializeField]
    private RaritySO itemLotteryRaritySO;
    [SerializeField]
    private BoolSO IsAttemptGenerate;
    public override void Initialise()
    {
        IsTurnChangeSO.onValueChanged += AttemptItemGenerate;
    }

    private void AttemptItemGenerate(object sender, EventArgs e)
    {
        int percent = percentPerBuilding * TileGrid.Instance.GetTotalBuilding();
        if (Random.Range(0,100) <  percent)
        {
            itemLotteryRaritySO.Rarity = rarity;
            IsAttemptGenerate.Bool = true;
        }
    }

}
