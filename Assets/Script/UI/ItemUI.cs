using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class ItemUI : MonoBehaviour
{
    [SerializeField]
    private BaseItemListSO items;
    [SerializeField]
    private List<ItemCardUI> cards;
    [SerializeField]
    private IntSO itemSlot;
    // Start is called before the first frame update
    void Start()
    {
        items.onValueChanged += UpdateItemsUIListener;
        UpdateItemUI();
    }
    private void UpdateItemsUIListener(object sender, EventArgs e)
    {
        UpdateItemUI();
    }
    private void UpdateItemUI()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (i >= items.Count)
            {
                cards[i].gameObject.SetActive(false);
            }
            else
            {
                cards[i].gameObject.SetActive(true);
                cards[i].SetValueFromSO(items[i].GetSO());
            }
        }
    }
    public void ItemUsed(int itemSlot)
    {
        this.itemSlot.Int = itemSlot;
    }


}
