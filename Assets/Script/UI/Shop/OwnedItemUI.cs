using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OwnedItemUI : MonoBehaviour
{
    [SerializeField]
    private BaseItemListSO items;
    [SerializeField]
    private GameObject[] itemOBJ = new GameObject[5];
    [SerializeField]
    private TMP_Text[] names = new TMP_Text[5];
    [SerializeField]
    private TMP_Text[] cost = new TMP_Text[5];
    private void Start()
    {
        items.onValueChanged += ShouldUpdateItemUI;
        UpdateOwnedItemUI();
    }

    private void ShouldUpdateItemUI(object sender, EventArgs e)
    {
        UpdateOwnedItemUI();
    }

    private void UpdateOwnedItemUI()
    {
        foreach (GameObject obj in itemOBJ)
        {
            obj.SetActive(false);
        }
        for (int i = 0; i < items.Count; i++)
        {
            itemOBJ[i].SetActive(true);
            names[i].text = items[i].GetSO().name;
            cost[i].text = Mathf.FloorToInt(items[i].GetSO().cost * 0.5f).ToString();
        }
    }
}
