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
    private EventSO OnTurnChangeSO;
    [SerializeField]
    private int currentTurn = 0;
    private void Start()
    {
        OnTurnChangeSO.onEventTrigger += TurnChanged;
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
