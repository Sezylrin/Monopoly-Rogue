using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbabilityUI : MonoBehaviour
{
    [SerializeField]
    private BoolSO IsOpenProbabilityUiSO;

    public void ModifyProbability(int category)
    {
        BuildingRoller.Instance.ModifyCategoryProbability(category);
        IsOpenProbabilityUiSO.Bool = false;

    }
}
