using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePolicy : MonoBehaviour
{
    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    public virtual void Initialise()
    {

    }
    public virtual void ApplyEffect()
    {

    }
    public virtual void RemoveEffect()
    {

    }
}

public class BuildingPolicy : BasePolicy
{
    protected TileGrid grid;
    protected List<Building> affected = new List<Building>();

    public override void Initialise()
    {
        grid = TileGrid.Instance;
    }
}
