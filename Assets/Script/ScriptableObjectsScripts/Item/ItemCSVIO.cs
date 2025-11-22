using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemCSVObj", menuName = "ScriptableObjects/Utility/ItemCSVIO")]

public class ItemCSVIO : ScriptableObject
{
    public ItemSOListSO allItems;

    //private string fileName = "";

    public TextAsset TextAssetData;

    private Dictionary<string, ItemSO> itemDict = new Dictionary<string, ItemSO>();
    
    [SerializeField]
    private string[] importData;
    [SerializeField]
    private int colAmount;

    public void ReadCSV()
    {
        GenerateDict();
        List<string> tempAllSO = new List<string>();
        foreach (ItemSO itemSO in allItems.GetList())
            tempAllSO.Add(itemSO.name); 
        
        string unSplit = TextAssetData.text; 
        unSplit = unSplit.Replace(", ", "@").Replace("\"", "");
        string[] data = unSplit.Split(new string[] { ",", "\n" }, StringSplitOptions.None);
        importData = data;
        int tableSize = data.Length / colAmount;
        Debug.Log(tableSize - 1 + " rows of data");
        for (int i = 1; i < tableSize; i++)
        {
            ItemSO itemSO = null;
            if (itemDict.ContainsKey(data[colAmount * i]))
            {
                itemSO = itemDict[data[colAmount * i]];
                tempAllSO.Remove(itemSO.name);
            }
            if (itemSO == null)
            {
                Debug.Log("failed to find \"" + data[colAmount * i] + "\" at row " + i);
                continue;
            }
            itemSO.rarity = (Rarity)Enum.Parse(typeof(Rarity), data[colAmount * i + 1]);
            itemSO.cost = int.Parse(data[colAmount * i + 2]);
            itemSO.effect = data[colAmount * i + 3].Replace("\"", "").Replace("\n", "").Replace("\r", "").Replace("@", ", ");

            EditorUtility.SetDirty(itemSO);
        }
        foreach (string name in tempAllSO)
            Debug.Log("Data not found for " + name);
        tempAllSO.Clear();
        tempAllSO = null;
        AssetDatabase.SaveAssets();
        Debug.Log("CSV data import complete");
    }
    private void GenerateDict()
    {
        itemDict.Clear();
        foreach (ItemSO item in allItems.GetList())
        {
            itemDict.Add(item.name, item);
        }
    }
}
