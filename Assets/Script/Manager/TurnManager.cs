using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField]
    private IntSO turnNumberSO;
    [SerializeField]
    private TileGrid grid;
    [SerializeField]
    private IntSO turnsPerTaxSO;
    [SerializeField]
    private BoolSO IsTaxTurn;
    [SerializeField]
    private IntSO currentSubTurnSO;

    [SerializeField]
    private BoolSO IsTurnChangeSO;
    // Start is called before the first frame update
    void Start()
    {
        currentSubTurnSO.Int = turnsPerTaxSO.Int;
        IsTurnChangeSO.onValueChanged += TriggerSubTurn;
    }

    private void TriggerSubTurn(object sender, EventArgs e)
    {
        if (!IsTurnChangeSO.Bool)
            return;
        IsTurnChangeSO.ResetValue();
        currentSubTurnSO.Int--;
        if (currentSubTurnSO.Int == -1)
        {
            currentSubTurnSO.Int = turnsPerTaxSO.Int;
            turnNumberSO.Int++;
            IsTaxTurn.Bool = true;                
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
