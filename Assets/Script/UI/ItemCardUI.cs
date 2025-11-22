using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemCardUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text itemName;
    [SerializeField]
    private TMP_Text description;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValueFromSO(BasePrefabSO itemSO)
    {
        itemName.text = itemSO.name;
        description.text = itemSO.effect;
    }
}
