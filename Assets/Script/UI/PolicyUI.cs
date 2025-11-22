using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolicyUI : MonoBehaviour
{
    [SerializeField]
    private BasePolicyListSO policies;
    [SerializeField]
    private List<ItemCardUI> cards;
    // Start is called before the first frame update
    void Start()
    {
        policies.onValueChanged += UpdateItemsUIListener;
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
            if (i >= policies.Count)
            {
                cards[i].gameObject.SetActive(false);
            }
            else
            {
                cards[i].gameObject.SetActive(true);
                cards[i].SetValueFromSO(policies[i].GetSO());
            }
        }
    }
}
