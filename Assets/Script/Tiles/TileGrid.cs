using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.InputSystem.Composites;

public class TileGrid : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Tile[] tiles = new Tile[24];

    [SerializeField]
    private IntSO currentPosSO;
    private int currentPos;

    [SerializeField]
    private IntSO moneyEarned;

    [SerializeField]
    private BuildingSO startingBuilding;

    public static TileGrid Instance;

    [SerializeField]
    private BuildingTypeSO buildingToChange;

    [SerializeField]
    private IntSO GridSize;

    [SerializeField]
    private BoolSO IsCollectCash;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            DestroyImmediate(Instance);
            Instance = this;
            Debug.Log("smth has gone wrong");
        }
    }
    void Start()
    {
        //spawn starting building
        /*for (int i = 0; i < tiles.Length; i ++)
        {
            tiles[i].SetGrid(this);
            Building temp = Instantiate(startingBuilding.building, Vector3.zero, Quaternion.identity).GetComponent<Building>();
            temp.Initiate(startingBuilding);
            temp.gameObject.name = temp.GetSO().name + " " + i.ToString();
            tiles[i].ChangeBuilding(temp,i);
        }*/
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].SetGrid(this);
            tiles[i].SetTilePos(i);
        }
        buildingToChange.onValueChanged += ToChangeBuilding;
        GridSize.Int = tiles.Length;
        IsCollectCash.onValueChanged += CalculateMoney;
        isRemoveTileEffect.onValueChanged += RemoveAllTileEffect;
        newTilePosSO.onValueChanged += MoveBuilding;
        
        SetCurrentPos(0);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void CalculateMoney(object sender, EventArgs e)
    {
        if (!IsCollectCash.Bool)
            return;
        float money = 0;
        foreach(Tile tile in tiles)
            money += tile.GetTileScore();
        moneyEarned.Int += Mathf.FloorToInt(money);
        IsCollectCash.ResetValue();
    }

    private void ChangeBuilding(Building building,int pos, bool destroy = true)
    {
        tiles[pos].ChangeBuilding(building, pos, destroy);
        if (building == null)
            return;
        building.gameObject.name = building.GetSO().name + " " + pos.ToString();
        if (building.GetSO().maxLimit > 0)
            ModifyLimits(building.GetSO());
        List<Building> affectedBuildings = new List<Building>{building};
        for (int i = 0; i < tiles.Length;i ++)
        {
            if (i == pos)
                continue;
            if (tiles[i].GetCurrentBuilding() == null)
                continue;
            Building temp = building.CompareComparisons(tiles[i].GetCurrentBuilding(), i);
            
            if (temp != null && !affectedBuildings.Contains(temp))
            {
                affectedBuildings.Add(temp);
            }
            temp = tiles[i].GetCurrentBuilding().CompareComparisons(building, pos);
            if (temp != null && !affectedBuildings.Contains(temp))
            {
                affectedBuildings.Add(temp);
            }

        }
        foreach (Building needUpdating in affectedBuildings)
        {
            needUpdating.ApplyEffects();
        }
    }
    private void ToChangeBuilding(object sender, EventArgs e)
    {
        if (buildingToChange.Building != null)
        {
            ChangeBuilding(buildingToChange.Building, currentPos);
            buildingToChange.ResetValue();
        }
    }

    public Tile[] GetTile()
    {
        return tiles;
    }

    public void SetCurrentPos(int pos)
    {
        currentPos = pos;
        currentPosSO.Int = currentPos;
    }

    public Building GetBuildingOnTile(int pos = -1)
    {
        return tiles[CheckPos(pos)].GetCurrentBuilding();
    }

    public int GetSize()
    {
        return GridSize.Int;
    }
    #region Limit
    private Dictionary<BuildingSO, int> limits = new Dictionary<BuildingSO, int>();
    public void ModifyLimits(BuildingSO building)
    {
        if(!limits.TryAdd(building, 1))
        {
            limits[building]++;
        }
        if (limits[building] == building.maxLimit)
        {
            BuildingRoller.Instance.RemoveFromSorted(building);
        }
    }

    public void RemoveLimit(BuildingSO building)
    {
        if (limits.ContainsKey(building))
        {
            limits[building]--;
            BuildingRoller.Instance.AddToSorted(building);
        }
        else
        {
            Debug.Log("Limits messed up somewhere");
        }
    }
    public bool IsNotAtLimit(BuildingSO building)
    {
        return limits.ContainsKey(building) ? limits[building] < building.maxLimit : false;
    }
    #endregion

    #region BuildingEffects
    public bool AddBuildingEffect(GameObject buildingEffectPF, int pos = -1)
    {
        return tiles[CheckPos(pos)].AddBuildingEffect(buildingEffectPF);
    }

    public bool BuildingContainsAnyEffects(int pos = -1)
    {
        return tiles[CheckPos(pos)].BuildingContainsAnyEffects();
    }
    #endregion

    #region Duplicate
    public void Duplicate(Building building)
    {
        ChangeBuilding(building, currentPos);
    }
    #endregion

    #region Movement
    [CollapsibleGroup("Movement")]
    [SerializeField]
    private IntSO newTilePosSO;
    public void EnableTileButtonUI(int pos, ButtonAction action)
    {
        if (!tiles[pos].IsTileProtected())
            tiles[pos].ShowUI(action);
    }
    
    public void EnableTileButtonUIIgnoreProtection(int pos, ButtonAction action)
    {
        tiles[pos].ShowUI(action);
    }
    public  void DisableTileButtonUI(int pos, ButtonAction action)
    {
        tiles[pos].HideUI(action);
    }

    public void MoveBuilding(object sender, EventArgs e)
    {
        Building buildingOne = GetBuildingOnTile(currentPos);
        buildingOne.RemoveAllAffected();
        buildingOne.ResetAllValues();
        buildingOne.ReapplyEffect();
        Building buildingTwo = GetBuildingOnTile(newTilePosSO.Int);
        if (buildingTwo != null)
        {
            buildingTwo.RemoveAllAffected();
            buildingTwo.ResetAllValues();
            buildingTwo.ReapplyEffect();
        }
        ChangeBuilding(null, currentPos, false);
        ChangeBuilding(buildingOne, newTilePosSO.Int, false);
        ChangeBuilding(buildingTwo, currentPos, false);
    }
    #endregion

    #region tileEffects
    [CollapsibleGroup("TileEffects")]
    [SerializeField]
    private BoolSO isRemoveTileEffect;

    private void RemoveAllTileEffect(object sender, EventArgs e)
    {
        tiles[currentPos].RemoveAllTileEffect();
        isRemoveTileEffect.ResetValue();
    }

    public bool AddTileEffect(GameObject effectToAdd, int pos = -1)
    {
        pos = CheckPos(pos);
        if (tiles[pos].IsTileProtected())
            return false;
        if (!tiles[pos].ContainsTileEffect(effectToAdd.name))
        {
            TileEffect effect = Instantiate(effectToAdd, tiles[pos].transform).GetComponent<TileEffect>();
            tiles[pos].AddTileEffect(effect);
            return true;
        }
        return false;
    }

    public bool IsTileProtected(int pos = -1)
    {
        pos = CheckPos(pos);
        return tiles[pos].IsTileProtected();
    }
    #endregion

    #region Utility
    private int CheckPos(int pos)
    {
        return pos < 0? currentPos : pos;
    }
    #endregion

    #region Debug
    [CollapsibleGroup("debug")]
    [SerializeField]
    private BuildingSO debugBuildingSO;
    [SerializeField]
    private int DebugPosition;
    [ContextMenu("Debug change building")]
    private void DebugTestChange()
    {
        Building temp = Instantiate(debugBuildingSO.building, Vector3.zero, Quaternion.identity).GetComponent<Building>();
        temp.Initiate(debugBuildingSO);
        ChangeBuilding(temp, DebugPosition);
    }
    #endregion
}
