using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolicyManager : MonoBehaviour
{
    // Start is called before the first frame update
    [CollapsibleGroup("Policy")]
    [SerializeField]
    private List<BasePolicy> policies = new List<BasePolicy>(); 
    public void AddPolicy(BasePolicy policy)
    {
        policies.Add(policy);
        policy.ApplyEffect();
    }

    public void RemovePolicy(BasePolicy policy)
    {
        policies.Remove(policy);
        policy.RemoveEffect();
    }

    public void SpawnPolicy(PolicySO policy)
    {
        BasePolicy newPolicy = Instantiate(policy.PolicyPF, transform).GetComponent<BasePolicy>();
        newPolicy.Initialise();
        AddPolicy(newPolicy);
    }
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
}
