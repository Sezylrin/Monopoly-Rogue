using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEffect : BaseEffect
{
    protected Tile currentTile;    

    public void SetTile(Tile tile)
    {
        currentTile = tile;
        SetCurrentBuilding();
    }

    public void SetCurrentBuilding()
    {
        currentBuilding = currentTile.GetCurrentBuilding();
    }
}
