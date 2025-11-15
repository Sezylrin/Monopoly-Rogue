using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum ButtonAction
{
    boundaryAdjustment,
    duplicate,
    transport,
    transfer,
    teleport,
}
public class Tile : MonoBehaviour
{
    [CollapsibleGroup("Tiles")]
    [SerializeField]
    private Building currentBuilding;
    private TileGrid grid;
    private Dictionary<string,TileEffect> effects = new Dictionary<string, TileEffect>();
    private int currentPos;

    [SerializeField]
    private Button button;

    private float multiplier = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    #region Getter and Setter
    public void SetTilePos(int currentPos)
    {
        this.currentPos = currentPos;
    }
    public void SetGrid(TileGrid grid)
    {
        this.grid = grid;
    }
    public float GetTileScore()
    {
        if (currentBuilding)
            return currentBuilding.GetCurrentValue() * multiplier;
        else
            return 0;
    }
    public Building GetCurrentBuilding()
    {
        return currentBuilding;
    }
    #endregion

    #region Button Management
    [CollapsibleGroup("Button Management")]
    [SerializeField]
    private GameObject buttonUIOBJ;
    [SerializeField]
    private BoolSO IsDisableButtonSO;
    public void ShowUI(ButtonAction action)
    {
        buttonUIOBJ.SetActive(true);
        AddListeners(action);
    }
    public void HideUI(ButtonAction action)
    {
        RemoveListeners(action);
        buttonUIOBJ.SetActive(false);
    }
    public void AddListeners(ButtonAction action)
    {
        switch (action)
        {
            case ButtonAction.boundaryAdjustment:
                button.onClick.AddListener(SetNewTilePos);
                break;
            case ButtonAction.duplicate:
                button.onClick.AddListener(CopyBuilding);
                break;
            case ButtonAction.transport:
                button.onClick.AddListener(SetNewTilePos);
                break;
            case ButtonAction.transfer:
                button.onClick.AddListener(Transfer);
                break;
            case ButtonAction.teleport:
                button.onClick.AddListener(Teleport);
                break;
        }
    }
    public void RemoveListeners(ButtonAction action)
    {
        switch (action)
        {
            case ButtonAction.boundaryAdjustment:
                button.onClick.RemoveListener(SetNewTilePos);
                break;
            case ButtonAction.duplicate:
                button.onClick.RemoveListener(CopyBuilding);
                break;
            case ButtonAction.transport:
                button.onClick.RemoveListener(SetNewTilePos);
                break;
            case ButtonAction.transfer:
                button.onClick.RemoveListener(Transfer);
                break;
            case ButtonAction.teleport:
                button.onClick.RemoveListener(Teleport);
                break;
        }
    }
    #endregion

    #region Teleport
    [CollapsibleGroup("Teleport")]
    [SerializeField]
    private IntSO teleportPosSO;
    private void Teleport()
    {
        IsDisableButtonSO.Bool = false;
        teleportPosSO.Int = currentPos;
    }
    #endregion

    #region Transfer
    private void Transfer()
    {
        foreach (BuildingEffect effect in currentBuilding.GetAllEffects())
        {
            grid.AddBuildingEffect(effect.gameObject);
        }
        IsDisableButtonSO.Bool = false;
    }
    #endregion

    #region Duplicate
    private void CopyBuilding()
    {
        Building temp = Instantiate(currentBuilding.gameObject).GetComponent<Building>();
        temp.ClearEffects();
        temp.ResetAllValues();
        grid.Duplicate(temp);
        temp.UpdateDict();
        temp.ReapplyEffect();
        IsDisableButtonSO.Bool = false;
    }
    #endregion

    #region Movement
    [CollapsibleGroup("Movement")]
    [SerializeField]
    private IntSO newTilePosSO;
    private void SetNewTilePos()
    {
        newTilePosSO.Int = currentPos;
        IsDisableButtonSO.Bool = false;
    }
    #endregion

    #region Change Building
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
            {
                if (currentBuilding.GetSO().maxLimit > 0)
                    grid.RemoveLimit(currentBuilding.GetSO());
                Destroy(currentBuilding.gameObject);
            }
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
    #endregion

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

    #region Historical
    public void ModifyMultiplier(float value)
    {
        multiplier += value;
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
            effect.gameObject.name = buildingEffectPF.name;
            currentBuilding.AddBuildingEffect(effect);
            return true;
        }
    }

    public bool BuildingContainsAnyEffects()
    {
        return currentBuilding? currentBuilding.ContainsAnyEffect() : false;
    }
    #endregion

    #region debug
    [ContextMenu("Debug destroy building")]
    private void DestroyBuilding()
    {
        ChangeBuilding(null, currentPos);
    }
    #endregion
}
