using KevinCastejon.MissingFeatures.MissingAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    private void Start()
    {
        currentCost = initialCost;
        currentRerollCost = initialRerollCost;
        UpdateUpgradeText();
        UpdateRerollText();
        newItems.onValueChanged += UpdateItemCardUI;
        newPolicies.onValueChanged += UpdatePolicyCardUI;
    }
    

    #region Reroll
    [CollapsibleGroup("Reroll")]
    [SerializeField]
    private BoolSO IsGenerateShopItem;
    [SerializeField]
    private int initialRerollCost;
    [SerializeField, ReadOnlyProp]
    private int currentRerollCost;
    [SerializeField]
    private int rerollCostScaling;
    [SerializeField]
    private TMP_Text rerollText;
    //Called by unity button
    public void Reroll()
    {
        canSpendPolicyPoint.onValueUpdated += CanReroll;
        SpendPolicyPointSO.Int = currentRerollCost;
    }

    private void CanReroll(object sender, EventArgs e)
    {
        if (canSpendPolicyPoint.Bool)
        {
            IsGenerateShopItem.Bool = true;
            currentRerollCost += rerollCostScaling;
            UpdateRerollText();
        }
        canSpendPolicyPoint.onValueUpdated -= CanReroll;
    }
    private void UpdateRerollText()
    {
        rerollText.text = "Reroll: " + currentRerollCost.ToString();
    }

    public void ResetRerollCost()
    {
        currentRerollCost = initialRerollCost;
        UpdateRerollText();
    }
    #endregion

    #region Update ItemUI
    [CollapsibleGroup("Item UI")]
    [SerializeField]
    private ItemSOListSO newItems;
    [SerializeField]
    private List<ShopItemCardUI> itemCardUIs;

    private void UpdateItemCardUI(object sender, EventArgs e)
    {
        for (int i = 0; i < itemCardUIs.Count; i++)
        {
            itemCardUIs[i].gameObject.SetActive(true);
            itemCardUIs[i].UpdateUI(newItems[i]);
        }
    }
    #endregion

    #region Update PolicyUI
    [CollapsibleGroup("Policy UI")]
    [SerializeField]
    private PolicySOListSO newPolicies;
    [SerializeField]
    private List<ShopItemCardUI> policyCardUIs;

    private void UpdatePolicyCardUI(object sender, EventArgs e)
    {
        for (int i = 0; i < newPolicies.Count; i++)
        {
            policyCardUIs[i].gameObject.SetActive(true);
            policyCardUIs[i].UpdateUI(newPolicies[i]);
        }
    }
    #endregion

    #region Buying
    [CollapsibleGroup("policy point")]
    [SerializeField]
    private IntSO SpendPolicyPointSO;
    [SerializeField]
    private BoolSO canSpendPolicyPoint;

    private int slotToBuy;
    #region items
    [CollapsibleGroup("Buying Item")]
    [SerializeField]
    private ItemSOSO ItemToBuySO;
    [SerializeField]
    private BaseItemListSO items;

    public void AttemptBuyItem(int slot)
    {
        if(items.Count == 5)
        {
            return;
        }
        slotToBuy = slot;
        canSpendPolicyPoint.onValueUpdated += BuyItem;
        SpendPolicyPointSO.Int = newItems[slot].cost;
    }

    public void BuyItem(object sender, EventArgs e)
    {
        if (canSpendPolicyPoint.Bool)
        {
            ItemToBuySO.ItemSO = newItems[slotToBuy];
            itemCardUIs[slotToBuy].gameObject.SetActive(false);
        }
        canSpendPolicyPoint.onValueUpdated -= BuyItem;
        canSpendPolicyPoint.ResetValueDelay();
    }
    #endregion

    #region Policy
    [CollapsibleGroup("Buying Policy")]
    [SerializeField]
    private PolicySOSO PolicyToBuySO;
    [SerializeField]
    private BasePolicyListSO policies;

    public void AttemptBuyPolicy(int slot)
    {
        if (policies.Count == 5)
        {
            return;
        }
        slotToBuy = slot;
        canSpendPolicyPoint.onValueUpdated += BuyPolicy;
        SpendPolicyPointSO.Int = newPolicies[slot].cost;
    }

    public void BuyPolicy(object sender, EventArgs e)
    {
        if (canSpendPolicyPoint.Bool)
        {
            PolicyToBuySO.PolicySO = newPolicies[slotToBuy];
            policyCardUIs[slotToBuy].gameObject.SetActive(false);
        }
        canSpendPolicyPoint.onValueUpdated -= BuyPolicy;
        canSpendPolicyPoint.ResetValueDelay();
    }
    #endregion
    #endregion

    #region Selling
    [CollapsibleGroup("Selling")]
    [SerializeField]
    private IntSO CurrentPolicyPointSO;
    #region Item
    [Header("Item")]
    [SerializeField]
    private IntSO ItemSlotToSellSO;

    public void SellItem(int slot)
    {
        CurrentPolicyPointSO.Int += items[slot].GetSellPrice();
        ItemSlotToSellSO.Int = slot;
    }
    #endregion

    #region Policy
    [Header("Policy")]
    [SerializeField]
    private IntSO PolicySlotToSellSO;

    public void SellPolicy(int slot)
    {
        CurrentPolicyPointSO.Int += policies[slot].GetSellPrice();
        PolicySlotToSellSO.Int = slot;
    }
    #endregion

    #endregion

    #region Upgrade probability
    [SerializeField]
    private IntSO ProbabilityLevelSO;
    [SerializeField]
    private int costScalingRate;
    [SerializeField]
    private int initialCost;
    [SerializeField, ReadOnlyProp]
    private int currentCost;
    [SerializeField]
    private int maxLevel;
    [SerializeField]
    private TMP_Text upgradeText;
    public void UpgradeProbabilityLevel()
    {
        if (ProbabilityLevelSO.Int >= maxLevel)
            return;
        canSpendPolicyPoint.onValueUpdated += CanUpgradeProbabilityLevel;
        SpendPolicyPointSO.Int = currentCost;
    }

    public void CanUpgradeProbabilityLevel(object sender, EventArgs e)
    {
        if (canSpendPolicyPoint.Bool)
        {
            currentCost += costScalingRate;
            ProbabilityLevelSO.Int++;
            UpdateUpgradeText();
        }
        canSpendPolicyPoint.onValueUpdated -= CanUpgradeProbabilityLevel;
    }

    public void UpdateUpgradeText()
    {
        upgradeText.text = "Upgrade: " + currentCost.ToString();
    }
    #endregion
}
