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
    private EventSO OnTurnChangeSO;
    // Start is called before the first frame update
    void Start()
    {
        currentSubTurnSO.Int = turnsPerTaxSO.Int;
        OnTurnChangeSO.onEventTrigger += TriggerSubTurn;
    }

    private void TriggerSubTurn(object sender, EventArgs e)
    {        
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
