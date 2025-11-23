using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoneyManager : MonoBehaviour
{
    [SerializeField]
    private FloatSO CurrentMoneySO;
    [SerializeField]
    private BoolSO IsCollectCash;
    [SerializeField]
    private BoolSO IsBasicGambling;
    [SerializeField]
    private FloatSO MoneyEarnedSO;
    // Start is called before the first frame update
    void Start()
    {
        IsCollectCash.onValueChanged += UpdateCurrentMoney;
        MoneyEarnedSO.onValueChanged += EarnMoney;
    }

    private void UpdateCurrentMoney(object sender, EventArgs e)
    {
        float earned = TileGrid.Instance.GetProjectedMoney();
        if (IsBasicGambling.Bool)
        {
            int random = Random.Range(0, 4);
            switch (random)
            {
                case 0:
                    earned *= 0.75f;
                    break;
                case 2:
                    earned *= 1.25f;
                    break;
                case 3:
                    earned *= 1.5f;
                    break;
            }
        }
        CurrentMoneySO.Float += earned;
    }

    private void EarnMoney(object sender, EventArgs e)
    {
        CurrentMoneySO.Float += MoneyEarnedSO.Float;
        MoneyEarnedSO.ResetValue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
