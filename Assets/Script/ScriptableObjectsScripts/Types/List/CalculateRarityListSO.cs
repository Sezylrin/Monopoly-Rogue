using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CalculateRarityListSO", menuName = "ScriptableObjects/Types/Lists/CalculateRarity")]

public class CalculateRarityListSO : RarityListSO
{
    [CollapsibleGroup("Calculated Rarity",-1)]
    public IntListSO CalculatedRarityProbability;
    public Rarity CalculateRarity()
    {
        Rarity selectedRarity = list[list.Count - 1];
        int rarity = Random.Range(0, 100);
        for (int i = 0; i < CalculatedRarityProbability.Count; i++)
        {
            if (rarity < CalculatedRarityProbability[i])
            {
                selectedRarity = list[i];
                break;
            }
        }
        return selectedRarity;
    }
}
