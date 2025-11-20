using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValuePolicy : BuildingPolicy
{
    [SerializeField]
    protected BuildingCategory targetCategory;
    [SerializeField]
    protected int value;
    [SerializeField]
    protected float multiplier;
    [SerializeField]
    protected BuildingTypeSO newBuildingSO;
    // Start is called before the first frame update
    public override void Initialise()
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
        if (current && (current.CompareCategory(targetCategory) || targetCategory == 0))
        {
            affected.Add(current);
            current.ModifyValue(value);
            current.ModifyMultiplier(multiplier);
        }
    }
    public override void RemoveEffect()
    {
        for (int i = affected.Count - 1; i >= 0; i--)
        {
            affected[i].ModifyValue(value);
            affected[i].ModifyMultiplier(multiplier);
            affected.RemoveAt(i);
        }
    }

    private void UpdateNewBuilding(object sender, EventArgs e)
    {
        AddBuilding(newBuildingSO.Building);
    }
}
