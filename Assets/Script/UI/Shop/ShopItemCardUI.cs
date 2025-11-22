using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItemCardUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text itemName;
    [SerializeField]
    private TMP_Text itemEffect;
    [SerializeField]
    private TMP_Text itemCost;

    public void UpdateUI(BasePrefabSO obj)
    {
        itemName.text = obj.name;
        itemEffect.text = obj.effect;
        itemCost.text = obj.cost.ToString();
    }
}
