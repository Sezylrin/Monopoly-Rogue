using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxManager : MonoBehaviour
{
    [SerializeField]
    private IntSO currentMoneySO;
    [SerializeField]
    private BoolSO GameOverSO;
    [SerializeField]
    private List<int> taxes = new List<int>();
    [SerializeField]
    private BoolSO isTaxTurn;
    [SerializeField]
    private IntSO taxTurn;
    [SerializeField]
    private IntSO taxAmount;
    private void Start()
    {
        isTaxTurn.onValueChanged += CheckTax;
        taxAmount.Int = taxes[taxTurn.Int];
    }

    private void CheckTax(object sender, EventArgs e)
    {
        if (isTaxTurn.Bool)
        {
            if(taxTurn.Int < taxes.Count)
            {
                currentMoneySO.Int -= taxes[taxTurn.Int];
                taxTurn.Int++;
                if (currentMoneySO.Int < 0)
                    GameOverSO.Bool = true;
                else
                    taxAmount.Int = taxes[taxTurn.Int];
            }
            isTaxTurn.ResetValue();
        }
    }
    private void Update()
    {
        
    }
}
