using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TempUI : MonoBehaviour
{
    public TMP_Text buildingName;
    public TMP_Text buildingValue;
    public Building building;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        buildingName.text = building.GetSO().name;
        buildingValue.text = building.GetCurrentValue().ToString();
    }

}
