using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateMoneyUI : MonoBehaviour
{
    public TMP_Text UIText;
    public IntSO money;
    // Start is called before the first frame update
    void Start()
    {
        money.onValueChanged += UpdateValueListner;
        UpdateValue();
    }

    private void UpdateValueListner(object sender, EventArgs e)
    {
        UpdateValue();
    }

    private void UpdateValue()
    {
        UIText.text = "Money: $" + money.Int.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
