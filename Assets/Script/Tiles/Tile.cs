using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
public class Tile : MonoBehaviour
{
    [CollapsibleGroup("Tiles")]
    [SerializeField]
    private Building currentBuilding;
    private TileGrid grid;
    private Dictionary<string,TileEffect> effects = new Dictionary<string, TileEffect>();
    private int tilePos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    [CollapsibleGroup("Boundary Adjustment")]
    [SerializeField]
    private IntSO newTilePos;
    [SerializeField]
    private GameObject UiForBoundaryAdjustment;
    [SerializeField]
    private BoolSO IsDisableButtonSO;
    public void ShowUI()
    {
        UiForBoundaryAdjustment.SetActive(true);
    }
    public void HideUI()
    {
        UiForBoundaryAdjustment.SetActive(false);
    }
    public void SetNewTilePos()
    {
        newTilePos.Int = tilePos; 
        UiForBoundaryAdjustment.SetActive(false);
        IsDisableButtonSO.Bool = false;
    }
    public void SetTilePos(int tilePos)
    {
        this.tilePos = tilePos;
    }
    public void SetGrid(TileGrid grid)
    {
        this.grid = grid;
    }

    public float GetTileScore()
    {
        if(currentBuilding)
        return currentBuilding.GetCurrentValue();
        else
            return 0;
        
    }

    public Building GetCurrentBuilding()
    {
        return currentBuilding;
    }
    public void ChangeBuilding(Building newBuilding, int position, bool destroy = true)
    {
        if (currentBuilding)
        {
            foreach (KeyValuePair<string,TileEffect> effect in effects)
            {
                effect.Value.RemoveEffect();
            }
            currentBuilding.RemoveAllAffected();
            if (destroy)
                Destroy(currentBuilding.gameObject);
        }
        if(newBuilding == null)
        {
            currentBuilding = null;
            return;
        }
        newBuilding.AssignPosition(position);
        newBuilding.gameObject.transform.SetParent(transform, false);
        currentBuilding = newBuilding;
        newBuilding.AssignCurrentTile(this);
        foreach (KeyValuePair<string, TileEffect> effect in effects)
        {
            effect.Value.SetCurrentBuilding();
            effect.Value.ApplyEffect();
        }
    }
    #region TileEffects
    public void RemoveAllTileEffect()
    {
        foreach (KeyValuePair<string, TileEffect> effect in effects)
        {
            effect.Value.RemoveEffect();
            Destroy(effect.Value.gameObject);
        }
        effects.Clear();

    }
    /// <summary>
    /// must not have duplicate
    /// </summary>
    /// <param name="effect"></param>
    public void AddTileEffect(TileEffect effect)
    {
        effects.Add(effect.gameObject.name,effect);
        effect.SetTile(this);
        effect.ApplyEffect();
    }

    public bool ContainsTileEffect(string effect)
    {
        return effects.ContainsKey(effect);
    }

    #region Protected Site

    [CollapsibleGroup("ProtectedSite")]
    [SerializeField]
    private bool isTileProtected;

    public void SetProtectionEffect(bool value)
    {
        isTileProtected = value;
    }
    public bool IsTileProtected()
    {
        return isTileProtected;
    }
    #endregion

    #endregion

    #region BuildingEffect
    public bool AddBuildingEffect(GameObject buildingEffectPF)
    {
        if (!currentBuilding || currentBuilding.ContainsBuildingEffect(buildingEffectPF.name))
        {
            return false;
        }
        else
        {
            BuildingEffect effect = Instantiate(buildingEffectPF, currentBuilding.transform).GetComponent<BuildingEffect>();
            currentBuilding.AddBuildingEffect(effect);
            return true;
        }
    }
    #endregion
}
