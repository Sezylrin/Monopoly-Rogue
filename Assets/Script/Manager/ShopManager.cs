using KevinCastejon.MissingFeatures.MissingAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShopManager : MonoBehaviour
{
    [CollapsibleGroup("All Data")]
    [SerializeField]
    private PolicySOListSO AllPolicySOListSO;
    [SerializeField]
    private ItemSOListSO AllItemSOListSO;
    [SerializeField]
    private CalculateRarityListSO calculateAllRarity;

    [SerializeField]
    private List<ListWrapper<PolicySO>> sortedPoliciesSO = new List<ListWrapper<PolicySO>>();
    [SerializeField]
    private List<ListWrapper<ItemSO>> sortedItemsSO = new List<ListWrapper<ItemSO>>();
    
    private void Start()
    {
        IsGenerateShopItem.onValueChanged += IsGenerate;
        GenerateNewShopItem();
    }

    [ContextMenu("SortLists")]
    private void SortLists()
    {
        sortedPoliciesSO.Clear();
        sortedItemsSO.Clear();
        for (int i = 0; i < Enum.GetValues(typeof(Rarity)).Length; i++)
        {
            sortedPoliciesSO.Add(new ListWrapper<PolicySO>(Enum.GetNames(typeof(Rarity))[i].ToString()));
            sortedItemsSO.Add(new ListWrapper<ItemSO>(Enum.GetNames(typeof(Rarity))[i].ToString()));
        }
        foreach (PolicySO policy in AllPolicySOListSO)
        {
            sortedPoliciesSO[(int)policy.rarity].Add(policy);
        }
        foreach (ItemSO item in AllItemSOListSO)
        {
            sortedItemsSO[(int)item.rarity].Add(item);
        }
    }
    [CollapsibleGroup("Generated Objects")]
    [SerializeField]
    private ItemSOListSO newItems;
    [SerializeField]
    private PolicySOListSO newPolicies;
    [SerializeField]
    private int AmountToGenerate;
    [SerializeField]
    private BoolSO IsGenerateShopItem;
    private void IsGenerate(object sender, EventArgs e)
    {
        GenerateNewShopItem();
        IsGenerateShopItem.ResetValueDelay();
    }
    private void GenerateNewShopItem()
    {
        newItems.ResetValue();
        newPolicies.ResetValue();
        List<ItemSO> tempItem = new List<ItemSO>();
        List<PolicySO> tempPolicies = new List<PolicySO>();
        for (int i = 0; i < AmountToGenerate; i++)
        {
            bool found = false;
#if UNITY_EDITOR
            int debug = 0;
#endif
            while (!found)
            {
#if UNITY_EDITOR
                debug++;
                if (debug >= 100)
                {
                    Debug.LogWarning("Stuck in infinite while loops");
                    Debug.Break();
                    return;
                }
#endif
                Rarity selectedRarity = calculateAllRarity.CalculateRarity();
                if (sortedItemsSO[(int)selectedRarity].Count == 0)
                    continue;
                int random = Random.Range(0, sortedItemsSO[(int)selectedRarity].Count);
                ItemSO selectedItem = sortedItemsSO[(int)selectedRarity][random];
                newItems.AddInvokeless(selectedItem);
                tempItem.Add(selectedItem);
                sortedItemsSO[(int)selectedRarity].RemoveAt(random);
                found = true;
            }

            found = false;
            debug = 0;
            while (!found)
            {
#if UNITY_EDITOR
                debug++;
                if (debug >= 100)
                {
                    Debug.LogWarning("Stuck in infinite while loops");
                    Debug.Break();
                    return;
                }
#endif
                Rarity selectedRarity = calculateAllRarity.CalculateRarity();
                if (sortedPoliciesSO[(int)selectedRarity].Count == 0)
                    continue;
                int random = Random.Range(0, sortedPoliciesSO[(int)selectedRarity].Count);
                PolicySO selectedItem = sortedPoliciesSO[(int)selectedRarity][random];
                newPolicies.AddInvokeless(selectedItem);
                tempPolicies.Add(selectedItem);
                sortedPoliciesSO[(int)selectedRarity].RemoveAt(random);
                found = true;
            }
        }
        newItems.ValueChanged();
        newPolicies.ValueChanged();
        foreach (ItemSO so in tempItem)
        {
            sortedItemsSO[(int)so.rarity].Add(so);
        }
        foreach (PolicySO so in tempPolicies)
        {
            sortedPoliciesSO[(int)so.rarity].Add(so);
        }
    }

    public void ReAddObject(PolicySO policySO)
    {
        sortedPoliciesSO[(int)policySO.rarity].Add(policySO);
    }
    public void ReAddObject(ItemSO itemSO)
    {
        sortedItemsSO[(int)itemSO.rarity].Add(itemSO);
    }
}
