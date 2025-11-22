using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct probability
{
    public int Common;
    public int Rare;
    public int Epic;
    public int Legendary;

    public int[] ToArray()
    {
        return new int[] { Common, Rare, Epic, Legendary };
    }
}
public class ProbabilityManager : MonoBehaviour
{
    [SerializeField]
    private IntListSO calculatedProbabilityListSO;
    [SerializeField]
    private IntListSO initialProbabilityListSO;
    [SerializeField]
    private IntSO ProbabilityLevelSO;
    [SerializeField]
    private List<probability> rarityProbabilty = new List<probability>();
    // Start is called before the first frame update
    void Awake()
    {
        initialProbabilityListSO.Copy(rarityProbabilty[0].ToArray());
        calculatedProbabilityListSO.Copy(new int[4]);
        ProbabilityLevelSO.onValueChanged += UpdateProbability;
        GenerateProbability();
    }
    private void GenerateProbability()
    {
        calculatedProbabilityListSO[0] = initialProbabilityListSO[0];
        for (int i = 1; i < initialProbabilityListSO.Count; i++)
        {
            calculatedProbabilityListSO[i] = calculatedProbabilityListSO[i - 1] + initialProbabilityListSO[i];
            if (calculatedProbabilityListSO[i] > 100)
                Debug.LogWarning("rarity probabilty exceedes 100");
        }
        calculatedProbabilityListSO.ValueChanged();
    }

    private void UpdateProbability(object sender, EventArgs e)
    {
        initialProbabilityListSO.CopyInvoke(rarityProbabilty[ProbabilityLevelSO.Int].ToArray());
        GenerateProbability();
    }
}
