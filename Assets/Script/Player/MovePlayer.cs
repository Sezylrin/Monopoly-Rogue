using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class MovePlayer : MonoBehaviour
{
    [CollapsibleGroup("Player")]
    [SerializeField]
    private Transform playerPos;
    [SerializeField]
    private Vector2 diceMinMax;
    [SerializeField]
    private int diceAmount;
    [SerializeField]
    private IntSO customDiceRollSO;
    [SerializeField]
    private float moveDuration;

    private int currentPos = 0;
    private TileGrid grid;
    // Start is called before the first frame update
    private void Awake()
    {
        grid = TileGrid.Instance;
    }
    void Start()
    {
        isToRollSO.onValueChanged += MovePlayerAround;
        teleportPosSO.onValueChanged += Teleport;
    }
    [SerializeField]
    private BoolSO isToRollSO;
    [SerializeField]
    private BoolSO canRollSO;
    public void MovePlayerAround(object sender, EventArgs e)
    {
        if (!isToRollSO.Bool || !canRollSO.Bool)
            return;
        isToRollSO.ResetValue();
        canRollSO.Bool = false;
        int moveAmount = 0;
        if(customDiceRollSO.Int == 0)
        {
            for (int i = 0; i < diceAmount; i++)
            {
                moveAmount += Random.Range((int)diceMinMax.x, (int)diceMinMax.y);
            }
        }
        else
        {
            moveAmount = customDiceRollSO.Int;
            customDiceRollSO.ResetValue();
        }
        transform.position = grid.GetTile()[currentPos].transform.position;
        Sequence diceMoving = DOTween.Sequence();
        float duration = moveDuration / moveAmount;
        for (int i = 0;i < moveAmount; i++)
        {
            diceMoving.Append(transform.DOMove(grid.GetTile()[(currentPos + i + 1) % grid.GetTile().Length].transform.position, duration).SetEase(Ease.Linear));
        }
        diceMoving.OnComplete(() => SetPos(moveAmount));
    }

    private void MovePlayerFunc(int amount)
    {

    }

    private void SetPos(int moveAmount)
    {
        currentPos = (currentPos + moveAmount) % grid.GetTile().Length;
        grid.SetCurrentPos(currentPos);
    }
    [CollapsibleGroup("Teleport")]
    [SerializeField]
    private IntSO teleportPosSO;

    private void Teleport(object sender, EventArgs e)
    {
        currentPos = teleportPosSO.Int;
        teleportPosSO.ResetValue();
        grid.SetCurrentPos(currentPos); 
        transform.position = grid.GetTile()[currentPos].transform.position;
    }
}
