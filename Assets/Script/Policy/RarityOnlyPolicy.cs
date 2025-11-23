using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RarityOnlyPolicy : RarityPolicy
{
    [SerializeField]
    private bool isOnlyTargetRarity = true;
    [SerializeField]
    protected List<Building> notTargetRarity = new List<Building>();
    [SerializeField]
    private EventSO OnBuildingDestroyedSO;

    
    protected override void Start()
    {
        base.Start();
        OnBuildingDestroyedSO.onEventTrigger += CheckList;
        
    }

    public void CheckList(object sender, EventArgs e)
    {
        if(!isOnlyTargetRarity)
            notTargetRarity.Remove(null);
        if (notTargetRarity.Count == 0)
        {
            isOnlyTargetRarity = true;
            EnableEffects();
        }
    }

    private void EnableEffects()
    {
        foreach (Building b in affected)
        {
            b.ModifyValue(value);
            b.ModifyMultiplier(multiplier);
        }
    }
    protected override void UpdateNewBuilding(object sender, EventArgs e)
    {
        base.UpdateNewBuilding(sender, e);
        CheckAdd();
    }
    public void CheckAdd()
    {
        if (newBuildingSO.Building.GetSO().rarity != targetRarity)
        {
            notTargetRarity.Add(newBuildingSO.Building);
            isOnlyTargetRarity = false;
            DisableEffect();
        }
    }
    private void DisableEffect()
    {
        foreach (Building b in affected)
        {
            b.ModifyValue(-value);
            b.ModifyMultiplier(-multiplier);
        }
    }
}
