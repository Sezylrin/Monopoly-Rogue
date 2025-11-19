using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BoolSO", menuName = "ScriptableObjects/Types/BoolSO")]

public class BoolSO : ResetableTypeSO<bool>
{
    [CollapsibleGroup("BoolSO")]
    [SerializeField]
    private bool _bool;
    public bool Bool
    {
        get { return _bool; }
        set
        {
            if (_bool == value)
            {
                return;
            }
            _bool = value;
            onValueChanged?.Invoke(this, EventArgs.Empty);
            DelayReset();
        }
    }
    public override void ResetValue()
    {
        _bool = defaultValue;
    }
}
