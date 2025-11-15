using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingCardButton : MonoBehaviour
{
    [SerializeField]
    private BuildingCard card;
    [SerializeField]
    private GameObject Outline;
    [SerializeField]
    private BuildingTypeSO newBuildingSO;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowGenerateCard(BuildingSO buildingSO)
    {
        card.SetBuildingSO(buildingSO);
    }

    public void OnHoverEnter()
    {
        Outline.SetActive(true);
    }

    public void OnHoverExit()
    {
        Outline.SetActive(false);
    }

    public void OnBuildingSelected()
    {
        Building temp = Instantiate(card.GetSO().building, Vector3.zero, Quaternion.identity).GetComponent<Building>();
        temp.Initiate(card.GetSO());
        newBuildingSO.Building = temp;
        Outline.SetActive(false);
    }
}
