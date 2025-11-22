using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OwnedPolicyUI : MonoBehaviour
{
    [SerializeField]
    private BasePolicyListSO policies;
    [SerializeField]
    private GameObject[] policyOBJ = new GameObject[5];
    [SerializeField]
    private TMP_Text[] names = new TMP_Text[5];
    [SerializeField]
    private TMP_Text[] cost = new TMP_Text[5];
    private void Start()
    {
        policies.onValueChanged += ShouldUpdateItemUI;
        UpdateOwnedItemUI();
    }

    private void ShouldUpdateItemUI(object sender, EventArgs e)
    {
        UpdateOwnedItemUI();
    }

    private void UpdateOwnedItemUI()
    {
        foreach (GameObject obj in policyOBJ)
        {
            obj.SetActive(false);
        }
        for (int i = 0; i < policies.Count; i++)
        {
            policyOBJ[i].SetActive(true);
            names[i].text = policies[i].name;
            cost[i].text = policies[i].GetSellPrice().ToString();
        }
    }
}
