using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxManager : MonoBehaviour
{
    [SerializeField]
    private FloatSO currentMoneySO;
    [SerializeField]
    private BoolSO IsGameOver;
    [SerializeField]
    private List<int> taxes = new List<int>();
    [SerializeField]
    private EventSO OnTaxTurnSO;
    [SerializeField]
    private IntSO taxTurn;
    [SerializeField]
    private IntSO taxAmount;
    private void Start()
    {
        OnTaxTurnSO.onEventOver += CheckTax;
        taxAmount.Int = taxes[taxTurn.Int];
    }

    private void CheckTax(object sender, EventArgs e)
    {        
        currentMoneySO.Float -= taxes[taxTurn.Int];
        taxTurn.Int++;
        if (currentMoneySO.Float < 0)
            IsGameOver.Bool = true;
        if (taxTurn.Int < taxes.Count)
            taxAmount.Int = taxes[taxTurn.Int];
    }
    private void Update()
    {
        
    }
}
