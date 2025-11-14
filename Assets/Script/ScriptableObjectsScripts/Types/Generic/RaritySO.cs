using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RaritySO", menuName = "ScriptableObjects/Types/RaritySO")]

public class RaritySO : TypeSO<Rarity>
{
    [CollapsibleGroup("RaritySO")]
    [SerializeField]
    private Rarity _rarity;
    public Rarity Rarity
    {
        get { return _rarity; }
        set
        {
            if (_rarity == value)
            {
                return;
            }
            _rarity = value;
            onValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }

}
