using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingCard : MonoBehaviour
{
    [SerializeField]
    private BuildingSO building;
    [SerializeField]
    private TMP_Text Name;
    [SerializeField]
    private Image image;
    [SerializeField]
    private TMP_Text effect;
    [SerializeField]
    private TMP_Text baseValue;

    private Building emulated;
    void Start()
    {

        if(building != null)
            LoadData();
    }
    private void OnDisable()
    {
        emulated = null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBuildingSO(BuildingSO building)
    {
        this.building = building;
        LoadData();
    }

    public BuildingSO GetSO()
    {
        return building;
    }

    private void LoadData()
    {
        Name.text = building.name;
        effect.text = building.effect;
        image.sprite = building.texture;
        string baseValue = building.baseValue.ToString();
        if(emulated)
        {
            baseValue += " (";
            baseValue += emulated.GetCurrentValue().ToString() + ")";
        }
        this.baseValue.text = baseValue;
    }

    public void SetEmulatedBuilding(Building emulated)
    {
        this.emulated = emulated;
        SetBuildingSO(this.emulated.GetSO());
    }
}
