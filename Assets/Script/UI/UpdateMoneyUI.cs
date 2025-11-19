using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateMoneyUI : MonoBehaviour
{
    public TMP_Text UIText;
    public FloatSO currentMoney;
    // Start is called before the first frame update
    void Start()
    {
        currentMoney.onValueChanged += UpdateValueListner;
        UpdateValue();
    }

    private void UpdateValueListner(object sender, EventArgs e)
    {
        UpdateValue();
    }

    private void UpdateValue()
    {
        UIText.text = "currentMoney: $" + currentMoney.Float.ToString("0.00");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
