using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePolicy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    protected PolicySO policySO;

    protected int sellPrice;
    protected virtual void Start()
    {
        
    }
    public virtual void Initialise(PolicySO policySO)
    {
        this.policySO = policySO;
        sellPrice = Mathf.FloorToInt(policySO.cost * 0.5f);
        Initialise();
    }

    protected virtual void Initialise()
    {

    }
    public virtual void ApplyEffect()
    {

    }
    public virtual void RemoveEffect()
    {

    }

    public PolicySO GetSO()
    {
        return policySO;
    }
    public int GetSellPrice()
    {
        return sellPrice;
    }
}

public class BuildingPolicy : BasePolicy
{
    protected TileGrid grid;
    protected List<Building> affected = new List<Building>();

    protected override void Initialise()
    {
        grid = TileGrid.Instance;
    }
}
