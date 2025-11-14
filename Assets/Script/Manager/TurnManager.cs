using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField]
    private IntSO turnSO;
    [SerializeField]
    private TileGrid grid;
    [SerializeField]
    private IntSO turnsPerRent;
    [SerializeField]
    private BoolSO IsRentTurn;
    [SerializeField]
    private IntSO currentSubTurn;

    [SerializeField]
    private BoolSO IsTurnChangeSO;
    // Start is called before the first frame update
    void Start()
    {
        currentSubTurn.Int = turnsPerRent.Int;
        IsTurnChangeSO.onValueChanged += TriggerSubTurn;
    }

    private void TriggerSubTurn(object sender, EventArgs e)
    {
        if (!IsTurnChangeSO.Bool)
            return;
        IsTurnChangeSO.ResetValue();
        currentSubTurn.Int--;
        if (currentSubTurn.Int == -1)
        {
            currentSubTurn.Int = turnsPerRent.Int;
            turnSO.Int++;
            IsRentTurn.Bool = true;                
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
