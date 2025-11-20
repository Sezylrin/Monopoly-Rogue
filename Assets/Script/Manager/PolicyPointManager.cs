using KevinCastejon.MissingFeatures.MissingAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolicyPointManager : MonoBehaviour
{
    [SerializeField]
    private BoolSO IsTurnChangeSO;
    [SerializeField]
    private int rollsPerPolicy;
    [SerializeField, ReadOnlyProp]
    private int currentRoll;
    [SerializeField]
    private IntSO CurrentPolicyPointSO;
    [SerializeField]
    private BoolSO IsBasicInterest;
    [SerializeField]
    private IntSO SpendPolicyPoint;
    [SerializeField]
    private BoolSO canSpendPolicyPoint;
    void Start()
    {
        IsTurnChangeSO.onValueChanged += UpdateRollCounter;
        SpendPolicyPoint.onValueChanged += AttemptSpendPolicyPoint;
    }

    private void UpdateRollCounter(object sender, EventArgs e)
    {
        if (IsTurnChangeSO)
        {
            currentRoll++;
            if (currentRoll >= rollsPerPolicy)
            {
                currentRoll = 0;
                int toEarn = TileGrid.Instance.EarnPolicyPoint();
                if (IsBasicInterest.Bool)
                    toEarn = Mathf.FloorToInt((float)toEarn * 1.05f);
                CurrentPolicyPointSO.Int += toEarn;
            }
        }
    }

    private void AttemptSpendPolicyPoint(object sender, EventArgs e)
    {
        if (SpendPolicyPoint.Int <= CurrentPolicyPointSO.Int)
        {
            CurrentPolicyPointSO.Int -= SpendPolicyPoint.Int;
            canSpendPolicyPoint.Bool = true;
        }
        else
            canSpendPolicyPoint.Bool = false;
        CurrentPolicyPointSO.ResetValue();
    }
}
