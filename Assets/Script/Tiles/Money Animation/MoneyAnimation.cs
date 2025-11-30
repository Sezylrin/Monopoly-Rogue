using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyAnimation : MonoBehaviour
{
    private Tile[] tiles;

    private Pool<MoneyUI> moneyUIPool;
    [SerializeField]
    private GameObject moneyUIOBJ;
    [SerializeField]
    private EventSO OnCollectCashSO;
    [SerializeField]
    private Timer timer;
    [SerializeField]
    private float moneyUiDelay;
    [SerializeField]
    private float moneyUiDuration;
    [SerializeField]
    private float delay;
    private int currentAnimStep = 0;
    [SerializeField]
    private EventSO OnTurnChangeSO;
    private void Start()
    {
        tiles = TileGrid.Instance.GetTile(); 
        PoolingManager.FindPool(moneyUIOBJ, out moneyUIPool);
        OnCollectCashSO.onEventTrigger += SortValue;
        timer.GenerateTimer(1);
        timer.SubscribeToTimerIsZero(StartAnimation);
        timer.SetTime(delay, false);
    }
    private List<float> unique;
    private Dictionary<float, List<Tile>> locations;
    private void SortValue(object sender, EventArgs e)
    {
        unique = new List<float>();
        locations = new Dictionary<float,List<Tile>>();
        foreach (Tile tile in tiles)
        {
            if (tile.GetTileScore() > 0)
            {
                if (!unique.Contains(tile.GetTileScore()))
                {
                    unique.Add(tile.GetTileScore());
                    locations.Add(tile.GetTileScore(), new List<Tile>() { tile });
                }
                else
                    locations[tile.GetTileScore()].Add(tile);
            }
        }
        unique.Sort();
        currentAnimStep = 0;
        PlayAnimation();
    }
    private void PlayAnimation()
    {
        if (currentAnimStep < unique.Count)
        {
            foreach (Tile tile in locations[unique[currentAnimStep]])
            {
                MoneyUI newObj = moneyUIPool.GetPooledObj();
                if (newObj.IsNewSpawn)
                    newObj.InitialiseNewSpawn(moneyUiDelay, moneyUiDuration);
                newObj.transform.position = tile.transform.position;
                newObj.SetText(tile.GetTileScore());
                newObj.StartAnimation();
            }
            timer.RestartTimer();
            currentAnimStep++;
        }
        else
            OnTurnChangeSO.Invoke();
    }

    private void StartAnimation(object sender, EventArgs e)
    {
        PlayAnimation();
    }
}
