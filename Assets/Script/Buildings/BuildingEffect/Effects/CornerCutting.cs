using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerCutting : BuildingEffect
{
    [CollapsibleGroup("Corner Cutting")]
    [SerializeField]
    private int turnAmount;
    [SerializeField]
    private BoolSO IsTurnChangeSO;
    [SerializeField]
    private int currentTurn = 0;
    private void Start()
    {
        IsTurnChangeSO.onValueChanged += TurnChanged;
    }
    public override void ApplyEffect()
    {
        ModifyValue();
    }

    public override void RemoveEffect()
    {
        ModifyValue(true);
    }

    public void TurnChanged(object sender, EventArgs e)
    {
        currentTurn++;
        if(currentTurn == turnAmount)
        {
            currentBuilding.DestroyBuilding();
        }
    }
}
