using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RarityUI : MonoBehaviour
{
    [SerializeField]
    private IntListSO initialRarityProbability;

    [SerializeField]
    private TMP_Text rarityText;
    [SerializeField]
    private string[] allRarities;
    private void Start()
    {
        initialRarityProbability.onValueChanged += ShouldUpdateText;
        UpdateText();
    }
    private void ShouldUpdateText(object sender, EventArgs e)
    {
        UpdateText();
    }
    private void UpdateText()
    {
        string text = "";
        for (int i = 0; i < allRarities.Length; i++)
        {
            text += allRarities[i] + ": " + initialRarityProbability[i].ToString() + "% ";
        }
        rarityText.text = text;
    }
}
