using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
[Flags]
public enum BuildingCategory
{
    residential = 1,
    industrial = 2,
    commercial = 4,
    energy = 8,
    educational = 16,
    environmental = 32,
}

public enum Rarity : int
{
    common,
    rare,
    epic,
    legendary,
}
[Flags]
public enum BoostType
{
    none = 0,
    self = 1,
    other = 2,
    generic = 4,
    specific = 8,
    conditional = 16,
}
public class Building : MonoBehaviour
{
    [CollapsibleGroup("Building")]
    [SerializeField]
    private int currentValue = 0;
    private int baseValue;
    [SerializeField]
    private BuildingSO buildingSO;
    private int baseMultiplier = 1;
    private float currentMultiplier;
    private int currentPosition;
    [SerializeField]
    private IntSO gridSize;
    /// <summary>
    /// other buildings where your values matched their requirement
    /// </summary>
    [SerializeField]
    private List<Building> buildingAffectingSelf = new List<Building>();
    /// <summary>
    /// buildings which matched your requirements
    /// </summary>
    [SerializeField]
    private List<ListWrapper<Building>> buildingSelfAffecting = new List<ListWrapper<Building>>();
    [SerializeField]
    private List<ListWrapper<Building>> buildingModified = new List<ListWrapper<Building>>();
    [SerializeField]
    private List<Comparisons> comparisons;
    [SerializeField]
    private Tile currentTile;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initiate(BuildingSO SO)
    {
        buildingSO = SO;
        baseValue = buildingSO.baseValue;
        currentValue = baseValue;
        currentMultiplier = baseMultiplier;
        comparisons = new List<Comparisons>(buildingSO.comparisonChecks);
        for (int i = 0; i < comparisons.Count; i++)
        {
            buildingModified.Add(new ListWrapper<Building>());
            buildingSelfAffecting.Add(new ListWrapper<Building>());
        }
    }
    public void AssignCurrentTile(Tile tile)
    {
        currentTile = tile;
    }
    public void AssignPosition(int position)
    {
        currentPosition = position;
    }

    public void DestroyBuilding()
    {
        currentTile.ChangeBuilding(null, currentPosition);
    }

    public float GetCurrentValue()
    {
        return currentValue * currentMultiplier;
    }

    public void ResetAllValues()
    {
        currentValue = baseValue;
        currentMultiplier = baseMultiplier;
    }
    public BuildingSO GetSO()
    {
        return buildingSO;
    }

    public int GetCurrentPos()
    {
        return currentPosition;
    }

    #region modifying associated lists

    //affecting self: other building has me in their comparison requirments
    //self affecting: i have other building in my comparison requirments
    public void AddBuildingToList(Building toBuilding, int pos)
    {
        if (!buildingSelfAffecting[pos].Contains(toBuilding))
            buildingSelfAffecting[pos].Add(toBuilding);
        toBuilding.AddBuildingReference(this);
    }

    public void AddBuildingReference(Building reference)
    {
        if (!buildingAffectingSelf.Contains(reference))
            buildingAffectingSelf.Add(reference);
    }
    public void RemoveSelfAffecting(Building fromBuilding)
    {
        ApplyEffects(true);
        for (int i = 0; i < buildingSelfAffecting.Count; i++)
        {
            buildingSelfAffecting[i].Remove(fromBuilding);
            buildingModified[i].Remove(fromBuilding);
        }
        ApplyEffects();
    }
    public void RemoveAffectingSelf(Building fromBuilding)
    {
        buildingAffectingSelf.Remove(fromBuilding);
    }
    public void RemoveAllAffected()
    {
        ApplyEffects(true);
        foreach (Building building in buildingAffectingSelf)
            building.RemoveSelfAffecting(this);
        for (int i = buildingSelfAffecting.Count - 1; i >=0; i--)
        {
            for (int j = buildingSelfAffecting[i].Count - 1; j >= 0; j--)
            {
                buildingSelfAffecting[i][j].RemoveAffectingSelf(this);
            }
            buildingSelfAffecting[i].Clear();
            buildingModified[i].Clear();
        }
    }
    #endregion

    #region Modifying values
    /// <summary>
    /// Apply all neighbour synergies
    /// </summary>
    /// <param name="removeEffect">set to true to remove the applied synergies</param>
    public virtual void ApplyEffects(bool removeEffect = false)
    {
        int modifier = 1;
        if (removeEffect)
            modifier = -1;
        for (int i =0; i < buildingSelfAffecting.Count; i++)
        {
            for(int j = 0; j < buildingSelfAffecting[i].Count; j++)
            {
                if (!removeEffect && buildingModified[i].Contains(buildingSelfAffecting[i][j]))
                    continue;
                else if (removeEffect && !buildingModified[i].Contains(buildingSelfAffecting[i][j]))
                    continue;
                //update list first to prevent recursive deadlocking
                if (!removeEffect)
                    buildingModified[i].Add(buildingSelfAffecting[i][j]);
                else
                    buildingModified[i].Remove(buildingSelfAffecting[i][j]);
                
                float value = comparisons[i].value * modifier;
                
                switch (comparisons[i].effectType)
                {
                    case EffectType.Value:
                        if (buildingSO.boostType.HasFlag(BoostType.other))
                            buildingSelfAffecting[i][j].ModifyValue(value);
                        else
                            ModifyValue(value);
                        break;
                    case EffectType.Multiplier:
                        if (buildingSO.boostType.HasFlag(BoostType.other))
                            buildingSelfAffecting[i][j].ModifyMultiplier(value);
                        else
                            ModifyMultiplier(value);
                        break;
                    case EffectType.Range:
                            buildingSelfAffecting[i][j].ModifyRange(value);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void ModifyValue(float amount)
    {
        currentValue += (int)amount;
    }
    public void ModifyMultiplier(float amount)
    {
        currentMultiplier += amount;
    }
    public void ModifyRange(float amount)
    {
        int modifiedRange = 0;
        for (int i = 0; i < comparisons.Count; i++)
        {
            if (comparisons[i].neighbourDist > 0)
            {
                Comparisons temp = comparisons[i];
                temp.neighbourDist += (int)amount;
                if (temp.neighbourDist > modifiedRange)
                    modifiedRange = temp.neighbourDist;

                comparisons[i] = temp;
            }
        }

        for (int i = -modifiedRange; i <= modifiedRange; i++)
        {
            if (i == 0)
                continue;
            int pos = (currentPosition + i + gridSize.Int) % gridSize.Int;
            CompareComparisons(TileGrid.Instance.GetBuildingOnTile(pos), pos);
        }

        ApplyEffects();
    }
    #endregion

    #region Comparison
    //Compares current buildings comparison check with a given building
    //if valid adds building to the corresponding list of list to determine
    //which effect to give.
    public Building CompareComparisons(Building BuildingToCheck, int position)
    {
        bool valid = false;
        for (int i = 0; i < comparisons.Count; i++)
        {
            Comparisons comparison = comparisons[i];
        
            if (BuildingToCheck == null)
                continue;
            else if (comparison.categoryCompare != 0 && (comparison.categoryCompare & BuildingToCheck.GetSO().category) == 0)
                continue;
           /* else if (comparison.rarityCompare != 0 && !Equals(comparison.rarityCompare, BuildingToCheck.GetSO().rarity))
                continue;*/
            if (comparison.neighbourDist != -1)
            {
                int diff = (int)MathF.Abs(currentPosition - position);
                int wrapDiff = gridSize.Int - diff;
                int final = (int)MathF.Min(diff, wrapDiff);
                if (final > comparison.neighbourDist)
                    continue;
            }
            if (comparison.compareSpecific.Count != 0)
            {
                if (!comparison.compareSpecific.Contains(BuildingToCheck.GetSO()))
                    continue;
            }
            AddBuildingToList(BuildingToCheck, i);
            valid = true;
        }
        if(valid)
            return this;
        else
            return null;
    }

    #endregion

    #region Building Effects
    [CollapsibleGroup("BuildingEffects")]
    [SerializeField]
    private Dictionary<string, BuildingEffect> effects = new Dictionary<string, BuildingEffect>();

    public void AddBuildingEffect(BuildingEffect effect)
    {
        effects.Add(effect.gameObject.name, effect);
        effect.SetBuilding(this);
        effect.ApplyEffect();
    }
    public bool ContainsBuildingEffect(string effect)
    {
        return effects.ContainsKey(effect);
    }
    #endregion
}
