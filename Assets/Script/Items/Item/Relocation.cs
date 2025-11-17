using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relocation : BaseItem
{
    [SerializeField]
    private IntSO currentPosSO;
    [SerializeField]
    private IntSO newTilePosSO;
    public override void AttemptItemUse()
    {
        if (TileHasNoBuilding())
        {
            ItemUseCancel();
            return;
        }
        List<int> emptySpots = new List<int>();
        for (int i = 0; i < TileGrid.Instance.GetSize(); i++)
        {
            if (TileHasNoBuilding(i))
            {
                emptySpots.Add(i);
            }
        }
        if (emptySpots.Count == 0)
        {
            ItemUseCancel();
            return;
        }
        int chosen = Random.Range(0, emptySpots.Count);
        newTilePosSO.Int = chosen;
        ItemUseSuccessful();
    }
}
