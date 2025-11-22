using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PolicyPointUI : MonoBehaviour
{
    [SerializeField]
    private IntSO CurrentPolicyPointSO;
    [SerializeField]
    private TMP_Text policyPointText;
    void Start()
    {
        CurrentPolicyPointSO.onValueChanged += ShouldUpdateText;
        UpdateText();
    }
    private void ShouldUpdateText(object sender, EventArgs e)
    {
        UpdateText();
    }
    private void UpdateText()
    {
        policyPointText.text = "Policy Points: " + CurrentPolicyPointSO.Int.ToString();
    }
}
