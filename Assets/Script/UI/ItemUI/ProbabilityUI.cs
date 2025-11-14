using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbabilityUI : MonoBehaviour
{
    [SerializeField]
    private BoolSO IsOpenProbabilityUiSO;
    [SerializeField]
    private BuildingRoller roller;

    public void ModifyProbability(int category)
    {
        roller.ModifyCategoryProbability(category);
        IsOpenProbabilityUiSO.Bool = false;

    }
}
