using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "BuildingTypeSO", menuName = "ScriptableObjects/Types/BuildingTypeSO")]


public class BuildingTypeSO : ResetableTypeSO<Building>
{
    private Building _building;
    public Building Building
    {
        get { return _building; }
        set
        {
            if(value != _building)
            {
                _building = value;
                onValueChanged?.Invoke(this, EventArgs.Empty);
                DelayReset();
            }            
        }
    }

    public void LateUpdate()
    {

    }
    public override void ResetValue()
    {
        _building = null;
    }
}
