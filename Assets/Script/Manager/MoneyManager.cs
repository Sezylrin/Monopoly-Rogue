using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField]
    private IntSO currentMoney;
    [SerializeField]
    private IntSO moneyToAdd;
    // Start is called before the first frame update
    void Start()
    {
        moneyToAdd.onValueChanged += UpdateCurrentMoney;
    }

    private void UpdateCurrentMoney(object sender, EventArgs e)
    {
        currentMoney.Int += moneyToAdd.Int;
        moneyToAdd.ResetValue();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
