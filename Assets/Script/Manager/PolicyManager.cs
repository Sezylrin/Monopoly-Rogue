using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolicyManager : MonoBehaviour
{
    // Start is called before the first frame update
    [CollapsibleGroup("Policy")]
    [SerializeField]
    private BasePolicyListSO policies;
    [SerializeField]
    private PolicySOSO PolicyToBuySO;
    private void Start()
    {
        PolicyToBuySO.onValueChanged += BuyPolicy;
        PolicySlotToSellSO.onValueChanged += SellPolicy;
    }
    public void BuyPolicy(object sender, EventArgs e)
    {
        SpawnPolicy(PolicyToBuySO.PolicySO);
        PolicyToBuySO.ResetValue();
    }

    public void SpawnPolicy(PolicySO policy)
    {
        BasePolicy newPolicy = Instantiate(policy.prefab, transform).GetComponent<BasePolicy>();
        newPolicy.Initialise(policy);
        newPolicy.gameObject.name = policy.name;
        policies.Add(newPolicy);
        newPolicy.ApplyEffect();
    }
    #region Sell
    [SerializeField]
    private IntSO PolicySlotToSellSO;
    private void SellPolicy(object sender, EventArgs e)
    {
        policies[PolicySlotToSellSO.Int].RemoveEffect();
        Destroy(policies[PolicySlotToSellSO.Int].gameObject);
        policies.RemoveAt(PolicySlotToSellSO.Int);
        PolicySlotToSellSO.ResetValue();
    }
    #endregion

    #region Debug
    [CollapsibleGroup("Debug")]
    [SerializeField]
    private List<PolicySO> debugSpawn = new List<PolicySO>();
    [ContextMenu("Generate Policy")]
    private void DebugPolicy()
    {
        foreach (PolicySO policy in debugSpawn)
        {
            SpawnPolicy(policy);
        } 
    }
    #endregion
}
