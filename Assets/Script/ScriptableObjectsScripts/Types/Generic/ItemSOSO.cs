using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemSOSO", menuName = "ScriptableObjects/Types/ItemSOSO")]

public class ItemSOSO : ResetableTypeSO<ItemSO>
{
    [CollapsibleGroup("ItemSOSO")]
    [SerializeField]
    private ItemSO _itemSO;
    public ItemSO ItemSO
    {
        get { return _itemSO; }
        set
        {
            if (_itemSO == value)
            {
                return;
            }
            _itemSO = value;
            onValueChanged?.Invoke(this, EventArgs.Empty);
            DelayReset();
        }
    }
    public override void ResetValue()
    {
        _itemSO = defaultValue;
    }
}
