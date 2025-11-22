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
    private IntSO SpendPolicyPointSO;
    [SerializeField]
    private BoolSO canSpendPolicyPoint;
    void Start()
    {
        IsTurnChangeSO.onValueChanged += UpdateRollCounter;
        SpendPolicyPointSO.onValueChanged += AttemptSpendPolicyPoint;
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
        if (SpendPolicyPointSO.Int <= CurrentPolicyPointSO.Int)
        {
            CurrentPolicyPointSO.Int -= SpendPolicyPointSO.Int;
            canSpendPolicyPoint.Bool = true;
        }
        else
        {
            canSpendPolicyPoint.Bool = false;
        }
        SpendPolicyPointSO.ResetValueDelay();
    }

    #region Debug
    [CollapsibleGroup("Debug")]
    [SerializeField]
    private int DebugEarn;
    [ContextMenu("Earn money")]
    private void DebugEarnPP()
    {
        CurrentPolicyPointSO.Int += DebugEarn;
    }
    #endregion
}
