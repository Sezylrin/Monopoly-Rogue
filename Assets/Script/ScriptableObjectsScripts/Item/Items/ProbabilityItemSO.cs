using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ProbabilitySO", menuName = "ScriptableObjects/Items/Probability")]

public class ProbabilityItemSO : ItemSO
{
    [CollapsibleGroup("Probability")]
    public int amount;
}
