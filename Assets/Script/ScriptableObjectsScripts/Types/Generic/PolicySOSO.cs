using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PolicySOSO", menuName = "ScriptableObjects/Types/PolicySOSO")]

public class PolicySOSO : ResetableTypeSO<PolicySO>
{
    [CollapsibleGroup("PolicySO")]
    [SerializeField]
    private PolicySO _policySO;
    public PolicySO PolicySO
    {
        get { return _policySO; }
        set
        {
            if (_policySO == value)
            {
                return;
            }
            _policySO = value;
            onValueChanged?.Invoke(this, EventArgs.Empty);
            DelayReset();
        }
    }

    public override void ResetValue()
    {
        _policySO = defaultValue;
    }
}
