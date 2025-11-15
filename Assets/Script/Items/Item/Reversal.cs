using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reversal : BaseItem
{
    [SerializeField]
    private BoolSO IsReversalActive;

    public override void AttemptItemUse()
    {
        IsReversalActive.Bool = true;
        ItemUseSuccessful();
    }
}
