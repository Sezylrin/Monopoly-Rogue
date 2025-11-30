using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour, IPoolable<MoneyUI>
{
    public Pool<MoneyUI> Pool { get; set; }
    public bool IsPooled { get; set; }
    public bool IsNewSpawn { get; set; }

    [SerializeField]
    private Vector3 endPos;
    [SerializeField]
    private Timer timer;
    [SerializeField]
    private FloatSO MoneyEarnedSO;

    private float value;
    public void PoolSelf()
    {
        MoneyEarnedSO.Float = value;
        timer.RestartTimer(false);
        Pool.PoolObj(this);
    }
    private float duration;
    public void InitialiseNewSpawn(float delay, float duration)
    {
        timer.GenerateTimer();
        timer.SetTime(delay, false);
        this.duration = duration;
        timer.SubscribeToTimerIsZero(AnimationPartTwo);
        IsNewSpawn = false;
    }

    [SerializeField]
    private TMP_Text moneyText;

    public void SetText(float Value)
    {
        value = Value;
        moneyText.text = Value.ToString();
    }
    public void StartAnimation()
    {        
        transform.DOMove(transform.position + new Vector3(0, 0.5f, 0), 0.5f).SetEase(Ease.InSine).OnComplete(() => timer.ResumeTimer());
    }

    private void AnimationPartTwo(object sender, EventArgs e)
    {        
        transform.DOMove(endPos, duration).SetEase(Ease.Linear).OnComplete(() => PoolSelf());
    }
}
