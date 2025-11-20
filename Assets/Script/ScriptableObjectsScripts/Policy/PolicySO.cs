using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PolicySO", menuName = "ScriptableObjects/Policy/PolicySO")]
public class PolicySO : ScriptableObject
{
    public GameObject PolicyPF;
    public Rarity rarity;
    [TextArea(3,10)]
    public string effect;
}
