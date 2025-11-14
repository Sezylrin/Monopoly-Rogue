using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDiceUI : MonoBehaviour
{
    [SerializeField]
    private IntSO customDiceRollSO;
    [SerializeField]
    private BoolSO isOpenCustomDiceUiSO;
    public void CustomDiceRoll(int value)
    {        
        customDiceRollSO.Int = value;
        isOpenCustomDiceUiSO.Bool = false;
    }
}
