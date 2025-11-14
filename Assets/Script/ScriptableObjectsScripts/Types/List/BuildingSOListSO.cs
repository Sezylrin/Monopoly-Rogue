using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "BuildingSOListSO", menuName = "ScriptableObjects/Building/BuildingSOListSO")]
public class BuildingSOListSO : ScriptableObject
{
    public List<BuildingSO> List = new List<BuildingSO>();
    public Dictionary<string,BuildingSO> Dict = new Dictionary<string,BuildingSO>();

    [ContextMenu("Generate Dict")]
    public void CreateDict()
    {
        Dict.Clear();
        foreach (BuildingSO item in List)
        {
            Dict.Add(item.name, item);
        }
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssetIfDirty(this);
    }
    public GameObject buildingObj;
    [ContextMenu("Apply Object")]
    public void ApplyObject()
    {
        foreach (BuildingSO item in List)
        {
            item.building = buildingObj;
            EditorUtility.SetDirty(item);
        }
        AssetDatabase.SaveAssets();
    }
}
