using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
[System.Serializable]

public class ItemManager : MonoBehaviour
{
    [SerializeField]
    private BaseItemListSO itemListSO;
    [SerializeField]
    private IntSO itemSlotUsed;
    [SerializeField]
    private int itemAmount;
    [SerializeField]
    private ItemSOListSO allItemSO;
    [SerializeField]
    private List<ListWrapper<ItemSO>> sortedItemSO = new List<ListWrapper<ItemSO>>();
    private void Start()
    {
        itemSlotUsed.onValueChanged += UseItem;
        IsAttemptGenerate.onValueChanged += AttemptGenerate;
        if(GenerateOnStart)
            TestGenerate();
    }
    

    public void UseItem(object sender, EventArgs e)
    {
       itemListSO[itemSlotUsed.Int].AttemptItemUse();
    }
    private void ItemUsed(object sender, boolEventArgs e)
    {
        if (!e.isSuccess)
        {
            itemSlotUsed.ResetValue();
            return;
        }
        itemListSO[itemSlotUsed.Int].OnItemUse -= ItemUsed;
        Destroy(itemListSO[itemSlotUsed.Int].gameObject);
        itemListSO.RemoveAt(itemSlotUsed.Int);
        itemSlotUsed.ResetValue();
    }
    public void AddItem(BaseItem item)
    {
        if (itemListSO.Count < itemAmount)
        {
            item.OnItemUse += ItemUsed;
            itemListSO.Add(item);
        }
    }

    public void GenerateItem(ItemSO itemToSpawn)
    {
        BaseItem item = Instantiate(itemToSpawn.prefab).GetComponent<BaseItem>();
        item.transform.SetParent(transform);
        item.OnGenerate(itemToSpawn);
        item.AssignManager(this);
        AddItem(item);
    }
    

    [ContextMenu("Sort Items")]
    private void SortItems()
    {
        sortedItemSO.Clear();
        for (int i = 0; i < Enum.GetValues(typeof(Rarity)).Length; i++)
        {
            sortedItemSO.Add(new ListWrapper<ItemSO>(Enum.GetName(typeof(Rarity), i)));
        }
        foreach (ItemSO itemSO in allItemSO)
        {
            sortedItemSO[(int)itemSO.rarity].Add(itemSO);
        }
    }

    public void GenerateMultipleItem(Rarity rarity, int amount, ItemSO sender = null)
    {
        if (sender)
            sortedItemSO[(int)rarity].Remove(sender);
        for (int i = 0; i < amount; i++)
        {
            if (sortedItemSO[(int)rarity].Count == 0)
                return;
            int random = Random.Range(0, sortedItemSO[(int)rarity].Count);
            
            if (itemListSO.Count <= itemAmount)
                GenerateItem(sortedItemSO[(int)rarity][random]);
        }
        if(sender)
            sortedItemSO[(int)rarity].Add(sender);
    }

    #region ItemLottery
    [CollapsibleGroup("Item Lottery")]
    [SerializeField]
    private BoolSO IsAttemptGenerate;
    [SerializeField]
    private RaritySO itemLotteryRaritySO;
    private void AttemptGenerate(object sender, EventArgs e)
    {
        if (IsAttemptGenerate.Bool)
        {
            GenerateMultipleItem(itemLotteryRaritySO.Rarity, 1);
            IsAttemptGenerate.ResetValue();
        }
    }
    #endregion

    #region Debug
    [CollapsibleGroup("Debug")]
    [SerializeField]
    private List<ItemSO> testItem = new List<ItemSO>();
    [SerializeField]
    private bool GenerateOnStart;
    [ContextMenu("test generate")]
    public void TestGenerate()
    {
        if (!Application.isPlaying)
            return;
        foreach (ItemSO itemSO in testItem)
        {
            if (itemListSO.Count < itemAmount)
                GenerateItem(itemSO);
        }
    }
    #endregion
}
