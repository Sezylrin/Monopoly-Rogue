using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class RarityPolicy : BuildingPolicy
{
    [SerializeField]
    protected BuildingTypeSO newBuildingSO;
    [SerializeField]
    protected Rarity targetRarity;
    [SerializeField]
    protected int value;
    [SerializeField]
    protected int multiplier;
    protected override void Initialise()
    {
        base.Initialise();
        newBuildingSO.onValueChanged += UpdateNewBuilding;
    }
    public override void ApplyEffect()
    {
        Tile[] tiles = grid.GetTile();
        for (int i = 0; i < tiles.Length; i++)
        {
            Building current = tiles[i].GetCurrentBuilding();
            AddBuilding(current);
        }
    }
    private void AddBuilding(Building current)
    {
        if (current && (current.GetSO().rarity == targetRarity))
        {
            affected.Add(current);
            current.ModifyValue(value);
            current.ModifyMultiplier(multiplier);
        }
    }
    protected virtual void UpdateNewBuilding(object sender, EventArgs e)
    {
        AddBuilding(newBuildingSO.Building);
    }
    public override void RemoveEffect()
    {
        for (int i = affected.Count - 1; i >= 0; i--)
        {
            affected[i].ModifyValue(-value);
            affected[i].ModifyMultiplier(-multiplier);
            affected.RemoveAt(i);
        }
    }
}
