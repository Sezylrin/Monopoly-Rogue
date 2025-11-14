using KevinCastejon.MissingFeatures.MissingAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public enum EffectType
{
    None,
    Value,
    Multiplier,
    Range,
}
[Serializable]
public struct Comparisons
{
    public Comparisons(EffectType effectType, float value, bool compareCategory, BuildingCategory buildingCategory, int neighbourDist)
    {
        this.effectType = effectType;
        this.value = value;
        this.compareCategory = compareCategory;
        categoryCompare = buildingCategory;
        //this.compareRarity = compareRarity;
        //rarityCompare = buildingRarity;
        this.neighbourDist = neighbourDist;
        compareSpecific = new List<BuildingSO>();
        
    }

    public EffectType effectType;
    public float value;
    public bool compareCategory;
    public BuildingCategory categoryCompare;
    //public bool compareRarity; 
    //public BuildingRarity rarityCompare;
    public int neighbourDist;
    public List<BuildingSO> compareSpecific;
}

[CreateAssetMenu(fileName = "BuildingSO", menuName = "ScriptableObjects/Building/BuildingSO")]
public class BuildingSO : ScriptableObject
{
    [TextArea(3,10)]
    public string effect;
    public int baseValue;
    public BuildingCategory category = BuildingCategory.residential;
    public Rarity rarity;
    public BoostType boostType;
    public Sprite texture;
    public GameObject building;
    public int maxLimit = 0;
    [SerializeField]
    public List<Comparisons> comparisonChecks = new List<Comparisons>();


#if UNITY_EDITOR
    private void OnValidate()
    {
        for (int i = 0; i < comparisonChecks.Count; i++)
        {
            Comparisons comparisons = comparisonChecks[i];
            if (!comparisons.compareCategory)
                comparisons.categoryCompare = 0;
            /*if (!comparisons.compareRarity)
                comparisons.rarityCompare = 0;*/
            comparisonChecks[i] = comparisons;
        }
    }

    
#endif
}
