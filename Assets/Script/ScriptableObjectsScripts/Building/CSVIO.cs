using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "ExportCSVObj", menuName = "ScriptableObjects/Utility/ExportToCSV")]

public class CSVIO : ScriptableObject
{
    public BuildingSOListSO allBuildings;

    private string filename = "";

    public TextAsset TextAssetData;

    [ContextMenu("GenerateCSV")]
    public void GenerateCSV()
    {
        filename = Application.dataPath + "/BuildingList.csv";
        WriteCSV();
    }

    private void WriteCSV()
    {
        if(allBuildings.List.Count <= 0)
            return;
        TextWriter tw = new StreamWriter(filename,false);
        tw.WriteLine("Name,Rarity,Category,Base Value,Boost Type,Description");
        foreach(BuildingSO building in allBuildings.List)
        {
            string final = building.name + ",";
            final += building.rarity.ToString() + ",";
            final += building.category.ToString().Replace(",","") + ",";
            final += building.baseValue.ToString() + ",";
            final += building.maxLimit.ToString() + ",";
            final += building.boostType.ToString() + ",";
            final += "\""+ building.effect.Replace("\r", "").Replace("\n","") +"\"";
            tw.WriteLine(final);
        }

        tw.WriteLine(GetEnumNames<Rarity>("Rarity Values"));
        tw.WriteLine(GetEnumNames<BuildingCategory>("Category Values"));
        tw.Write(GetEnumNames<BoostType>("Boost Type"));
        tw.Close();
        TextAssetData = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/BuildingList.csv");
        Debug.Log("CSV Generated");
    }

    private string GetEnumNames<T>(string Name) where T : Enum
    {
        Name += ",";
        string[] enumNames = Enum.GetNames(typeof(T));
        foreach (string rarityName in enumNames)
            Name += rarityName + ",";
        Name = Name.Substring(0, Name.Length - 1);
        return Name;
    }

    private Dictionary<string,BuildingSO> buildDict = new Dictionary<string,BuildingSO>();
    [SerializeField]
    private string[] importData;
    [SerializeField]
    private int colAmount;
    public void ReadCSV()
    {
        GenerateDict();
        List<string> tempAllSO = new List<string>();
        foreach (BuildingSO building in allBuildings.List)
            tempAllSO.Add(building.name);

        List<string> SOToCreate = new List<string>();

        string unSplit = TextAssetData.text;
        unSplit = unSplit.Replace(", ", "@").Replace("\"", "");
        string[] data = unSplit.Split(new string[] { ",", "\n" }, StringSplitOptions.None);
        importData = data;
        int tableSize = data.Length / colAmount;
        Debug.Log(tableSize - 1 + " rows of data");
        for (int i = 1; i < tableSize; i++)
        {
            BuildingSO buildingSO;
            if( buildDict.ContainsKey(data[colAmount * i]))
            {
                buildingSO = buildDict[data[colAmount * i]];
                tempAllSO.Remove(buildingSO.name);
            }
            else
            {
                buildingSO = CreateInstance<BuildingSO>();
                buildingSO.name = data[colAmount * i];
                AssetDatabase.CreateAsset(buildingSO, "Assets/ScriptableObject/Buildings/" + buildingSO.name + ".asset");
                if (!allBuildings.List.Contains(buildingSO))
                    allBuildings.List.Add(buildingSO);
            }
            if(buildingSO == null)
            {                
                Debug.Log("failed to find \"" + data[colAmount * i] + "\" at row " + i);
                return;
            }
            buildingSO.rarity = (Rarity)Enum.Parse(typeof(Rarity), data[colAmount * i + 1]);
            buildingSO.category = (BuildingCategory)GetEnum<BuildingCategory>(data[colAmount * i + 2].Replace("@", " "));
            buildingSO.baseValue = int.Parse(data[colAmount * i + 3]);
            buildingSO.maxLimit = int.Parse(data[colAmount * i + 4]);
            buildingSO.boostType = (BoostType)GetEnum<BoostType>(data[colAmount * i + 5].Replace("@", " "));
            buildingSO.effect = data[colAmount * i + 6].Replace("\"", "").Replace("\n", "").Replace("\r", "").Replace("@", ", ");
            EditorUtility.SetDirty(buildingSO);
        }
        foreach (string name in tempAllSO)
            Debug.Log("Data not found for " + name);
        tempAllSO.Clear();
        tempAllSO = null;
        AssetDatabase.SaveAssets();
        Debug.Log("CSV data import complete");
    }

    [ContextMenu("test")]
    public void Test()
    {
        BuildingCategory test;
        test = (BuildingCategory)GetEnum<BuildingCategory>("residential educational");
        Debug.Log(test);
    }
    private int GetEnum<T>(string enumvalue) where T : Enum
    {
        string[] splitValues = enumvalue.Split(" ", StringSplitOptions.None);
        int value = 0;
        foreach (string valueName in splitValues)
        {
            value += (int)(object)(T)Enum.Parse(typeof(T), valueName);
        }
            
        return value;
    }
    private void GenerateDict()
    {
        buildDict.Clear();
        foreach(BuildingSO building in allBuildings.List)
        {
            buildDict.Add(building.name, building);
        }
    }
}
