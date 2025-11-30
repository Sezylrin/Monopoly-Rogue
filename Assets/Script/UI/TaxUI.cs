using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TaxUI : MonoBehaviour
{
    [SerializeField]
    private FloatSO CurrentMoneySO;
    [SerializeField]
    private TMP_Text moneyText;
    [SerializeField]
    private IntSO taxAmount;
    [SerializeField]
    private TMP_Text taxText;
    [SerializeField]
    private int animDuration;
    [SerializeField]
    private Timer timer;
    [SerializeField]
    private float animStartDelay;
    [SerializeField]
    private EventSO OnTaxTurnSO;
    [SerializeField]
    private GameObject taxUiObj;
    [SerializeField]
    private BoolSO IsGameOver;
    [SerializeField]
    private UIManager uiManager;
    [SerializeField]
    private Button wholeScreenButton;
    [SerializeField]
    private TMP_Text continueText;
    void Start()
    {
        timer.GenerateTimer();
        timer.SetTime(animStartDelay, false);
        timer.SubscribeToTimerIsZero(PlayAnimation);
        OnTaxTurnSO.onEventTrigger += StartAnimSequence;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private int current;
    private int tax;
    private void StartAnimSequence(object sender, EventArgs e)
    {
        taxUiObj.SetActive(true);
        current = Mathf.FloorToInt(CurrentMoneySO.Float);
        tax = taxAmount.Int;
        ModifyCurrentValue(current); 
        ModifyTaxValue(tax);
        timer.RestartTimer();
    }
    private void PlayAnimation(object sender, EventArgs e)
    {
        DOVirtual.Int(current, current - tax, animDuration, ModifyCurrentValue).SetEase(Ease.InOutSine);
        DOVirtual.Int(tax, 0, animDuration, ModifyTaxValue).SetEase(Ease.InOutSine).OnComplete(AnimationEnd);
    }

    private void ModifyCurrentValue(int newValue)
    {
        moneyText.text = "$" + newValue.ToString();
    }

    private void ModifyTaxValue(int newValue)
    {
        taxText.text ="-" + newValue.ToString();
    }

    private void AnimationEnd()
    {
        wholeScreenButton.enabled = true;
        continueText.gameObject.SetActive(true);
    }

    public void DetermineNext()
    {
        if (IsGameOver.Bool)
        {
            Debug.Log("Game over");
        }
        else
        {
            taxUiObj.SetActive(false);
            uiManager.OpenShopUI();
        }
        wholeScreenButton.enabled = false;
        continueText.gameObject.SetActive(false);
    }
}
