using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "BuildingSynergyGenerator", menuName = "ScriptableObjects/Utility/BuildingSynergyGenerator")]

public class BuildingSynergyGenerator : ScriptableObject
{
    public BuildingSOListSO allBuildings;
    public List<KeywordRule> keywords = new List<KeywordRule>();
    public List<string> categoryKeywords = new List<string>();
    public List<string> debugList = new List<string>();
    public List<Comparisons> debugComparison = new List<Comparisons>();
    [ContextMenu("Test generate")]
    public void GenerateEffect()
    {
        allBuildings.CreateDict();
        debugList.Clear();
        debugComparison.Clear();
        foreach (BuildingSO so in allBuildings.List)
        {
            so.comparisonChecks.Clear();
            string[] uniqueEffects = so.effect.Split(". ");
            debugList.AddRange(uniqueEffects);
            foreach (string effect in uniqueEffects)
            {
                if (effect.Equals("Does nothing"))
                    continue;
                Comparisons compare = new Comparisons();
                compare.compareSpecific = new List<BuildingSO>();
                compare = MatchEffectType(compare, effect);
                compare = MatchValue(compare, effect);
                compare = MatchNeighbourDist(compare, effect);
                compare = MatchCategoryOrSpecific(compare, effect);
                so.comparisonChecks.Add(compare);
                debugComparison.Add(compare);
            }
            EditorUtility.SetDirty(so);
        }
        AssetDatabase.SaveAssets();
        Debug.Log("Comparisons generated");
    }
    public BuildingSOListSO debugListForTesting;
    private Comparisons MatchCategoryOrSpecific(Comparisons compare, string effect)
    {
        string extracted = ExtractCategoryAndSpecfic(effect);
        extracted = extracted.Replace(" and ", ", ");
        string[] splitExtracted = extracted.Split(", ");
        string[] categoryNames = Enum.GetNames(typeof(BuildingCategory));
        compare.categoryCompare = 0;
        compare.compareSpecific.Clear();
        foreach (string name in splitExtracted)
        {
            if (categoryNames.Contains(name.ToLower()))
            {
                if (Enum.TryParse<BuildingCategory>(name.ToLower(), out BuildingCategory value))
                    compare.categoryCompare += (int)value;
                else
                    Debug.Log("Error found in \"" + effect + "\" at word " + name);
            }
            else
            {
                if (debugListForTesting.Dict.TryGetValue(name, out BuildingSO building))
                    compare.compareSpecific.Add(building);
                else
                    Debug.Log(name + " not found in effect \"" + effect + "\"");
            }
        }
        if (compare.categoryCompare > 0)
            compare.compareCategory = true;
        return compare;
    }
    private string ExtractCategoryAndSpecfic(string input)
    {
        string result = "";
        int index = 0;
        foreach (string category in categoryKeywords)
        {
            index = input.IndexOf(category);

            if (index != -1)
            {
                result = input.Substring(index + category.Length).TrimStart();
                break;
            }
        }
        if (result.Equals(""))
            Debug.Log("Input string did not contain necessary word. Input string: " + input);
        index = result.IndexOf("tiles");
        if (index != -1)
        {
            result = result.Substring(0, index).TrimEnd();
        }
        else
            Debug.Log("Error with formating, tiles word not found, check \"" + input + "\" format again");

        return result;
    }
    private Comparisons MatchNeighbourDist(Comparisons compare, string effect)
    {
        compare.neighbourDist = -1;
        foreach (KeywordRule rule in keywords)
        {
            if (effect.Contains(rule.Keyword))
            {
                compare.neighbourDist = rule.value;
            }
        }
        return compare;
    }

    private Comparisons MatchEffectType(Comparisons compare, string effect)
    {
        if (effect.Contains("value"))
        {
            compare.effectType = EffectType.Value;

        }
        else if (effect.Contains("multiplier"))
        {
            compare.effectType = EffectType.Multiplier;
        }
        else if (effect.Contains("range"))
        {
            compare.effectType = EffectType.Range;
        }
        else
        {
            compare.effectType = EffectType.None;
        }
        return compare;
    }

    private Comparisons MatchValue(Comparisons compare, string effect)
    {
        Match match = Regex.Match(effect, @"[-+]?\d+(\.\d+)?"); // Regex to match a float

        if (match.Success)
        {
            string floatSubstring = match.Value;
            float extractedFloat = float.Parse(floatSubstring);
            if (effect.Contains("Decrease"))
                extractedFloat *= -1;
            compare.value = extractedFloat;
        }
        else
        {
            Debug.Log("No float found in string: " + effect);
        }
        return compare;
    }
}
[Serializable]
public struct KeywordRule
{
    public string Keyword;
    public int value;
}