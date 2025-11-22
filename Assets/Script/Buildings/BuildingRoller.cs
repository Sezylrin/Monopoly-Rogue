using KevinCastejon.MissingFeatures.MissingAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;



public class BuildingRoller : MonoBehaviour
{
    [SerializeField]
    private BuildingSOListSO allBuildings;

    private List<BuildingSO> discardedBuilding;

    private BuildingSO[] selectedBuilding = new BuildingSO[3];
    //list one rarity, list two which category, list 3 actual building
    [SerializeField]
    private List<ListWrapper<ListWrapper<BuildingSO>>> sortedBuildings = new List<ListWrapper<ListWrapper<BuildingSO>>>();

    [SerializeField]
    private CalculateRarityListSO CalculateAllRarity;

    [SerializeField, NamedArray(new string[] { "residential", "industrial", "commerce", "energy", "educational", "environmental"})]
    private int[] categoryProbabilty = new int[Enum.GetValues(typeof(BuildingCategory)).Length];
    [SerializeField, ReadOnlyProp]
    private int[] _categoryProbabilty = new int[Enum.GetValues(typeof(BuildingCategory)).Length];

    [SerializeField]
    private BuildingCardButton[] cards  = new BuildingCardButton[3];
    [SerializeField]
    private List<BuildingSO> selectedCards;
    // Start is called before the first frame update
    public static BuildingRoller Instance;
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
        UpdateRerollText();
    }
    void Start()
    {
        
        GenerateProbability(); 
        GenerateBuildingStored();
        allCategory = (BuildingCategory[])Enum.GetValues(typeof(BuildingCategory));
        IsTurnChangeSO.onValueChanged += ProbabilityTurnCounter;
    }
    #region Sort Building
    [ContextMenu("Sort Buildings")]
    public void SortBuildings()
    {
        sortedBuildings.Clear();
        for (int i = 0; i < Enum.GetValues(typeof(Rarity)).Length; i++)
        {
            ListWrapper<ListWrapper<BuildingSO>> category = new ListWrapper<ListWrapper<BuildingSO>>();
            for (int j = 0; j < Enum.GetValues(typeof(BuildingCategory)).Length; j++)
            {
                category.Add(new ListWrapper<BuildingSO>(Enum.GetName(typeof(BuildingCategory),(int)MathF.Pow(2, j))));
            }
            category.Name = Enum.GetName(typeof(Rarity), i);
            sortedBuildings.Add(category);
        }
        foreach (BuildingSO buildingSO in allBuildings.List)
        {
            foreach (int ints in Utility.EnumPoses(buildingSO.category))
            {
                sortedBuildings[(int)buildingSO.rarity][ints].Add(buildingSO);
            }
        }
    }

    public void RemoveFromSorted(BuildingSO toRemove)
    {
        foreach (int listPos in Utility.EnumPoses(toRemove.category))
        {
            sortedBuildings[(int)toRemove.rarity][listPos].Remove(toRemove);
        }
    }

    public void AddToSorted(BuildingSO toAdd)
    {
        foreach (int listPos in Utility.EnumPoses(toAdd.category))
        {
            sortedBuildings[(int)toAdd.rarity][listPos].Add(toAdd);
        }
    }
    #endregion

    #region Generate Building
    private List<BuildingSO> GenerateBuilding(Rarity RarityOverride = (Rarity)(-1))
    {
        List<BuildingSO> finalBuildings = new List<BuildingSO>();
#if UNITY_EDITOR
        allCategory = (BuildingCategory[])Enum.GetValues(typeof(BuildingCategory));
#endif
        BuildingCategory selectedCategory = 0;
        List<BuildingSO> selectedSO = new List<BuildingSO>();
        for (int i = 0; i < 3; i++)
        {
            Rarity selectedRarity;
            int rarity = Random.Range(0, 100);
            if (RarityOverride == (Rarity)(-1))
                selectedRarity = CalculateAllRarity.CalculateRarity();
            else
                selectedRarity = RarityOverride;
            //Debug.Log(rarity + " " + selectedRarity.ToString());
            //Debug.Log("Rarity: " + rarity + " " + selectedRarity.ToString());
            bool success = false;
#if UNITY_EDITOR
            int debugFailSafe = 0;
#endif
            while (!success)
            {
                int category = Random.Range(0, _categoryProbabilty[_categoryProbabilty.Length - 1]);
                for (int j = 0; j < _categoryProbabilty.Length; j++)
                {
                    if (category < _categoryProbabilty[j])
                    {
                        selectedCategory = allCategory[j];
                        break;
                    }
                }
                if (sortedBuildings[(int)selectedRarity][Utility.EnumPos(selectedCategory)].Count > 0)
                    success = true;
#if UNITY_EDITOR
                else
                {
                    debugFailSafe++;
                    if (debugFailSafe > 100)
                    {
                        Debug.LogWarning("SortedList does not have enough cards in rarity :" + selectedRarity.ToString());
                        Debug.Break();
                        return null;
                    }
                }
#endif
            }
            
            //Debug.Log("Category: " + category + " " + selectedCategory.ToString());
            int random = Random.Range(0, sortedBuildings[(int)selectedRarity][Utility.EnumPos(selectedCategory)].Count);
            //Debug.Log("Random: " + random.ToString());
            debugValuesSO.String = "Selected rarity: " + ((int)selectedRarity).ToString() + " " + selectedRarity.ToString() + " Random: " + random + " selected category: " + selectedCategory.ToString();
            BuildingSO chosen = sortedBuildings[(int)selectedRarity][Utility.EnumPos(selectedCategory)][random];
            finalBuildings.Add(chosen);
            selectedSO.Add(chosen);
            List<int> chosenCategories = Utility.EnumPoses(chosen.category);
            foreach (int c in chosenCategories)
                sortedBuildings[(int)chosen.rarity][c].Remove(chosen);
        }
        foreach (BuildingSO so in selectedSO)
        {
            List<int> chosenCategories = Utility.EnumPoses(so.category);
            foreach (int c in chosenCategories)
                sortedBuildings[(int)so.rarity][c].Add(so);
        }
        return finalBuildings;
    }

    public void GenerateBuildingTemp(Rarity rarity = 0)
    {
        SetCardWithBuildingSO(GenerateBuilding(rarity));
    }

    public void GenerateBuildingStored()
    {
        selectedCards = GenerateBuilding();
        SetCardWithBuildingSO(selectedCards);
    }
    #endregion

    #region CardSO Setting
    private void SetCardWithBuildingSO(List<BuildingSO> buildings)
    {
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].ShowGenerateCard(buildings[i]);
        }
    }
    public void ResetCardToStoredSO()
    {
        SetCardWithBuildingSO(selectedCards);
    }
    #endregion

    #region Probability Function
    [ContextMenu("GenerateProbabilty")]
    private void GenerateProbability()
    {
        _categoryProbabilty[0] = categoryProbabilty[0];
        for (int i = 1; i < _categoryProbabilty.Length; i++)
            _categoryProbabilty[i] = _categoryProbabilty[i - 1] + categoryProbabilty[i];
    }
    #region Probability
    [CollapsibleGroup("Probability")]
    [SerializeField]
    private IntSO probabilityIncreaseSO;
    [SerializeField]
    private int maxTurns;
    [SerializeField]
    private int currentTurn;
    [SerializeField]
    private BoolSO IsTurnChangeSO;
    private int modifiedAmount;
    private int modifiedPos;
    [SerializeField, ReadOnlyProp]
    private bool isProbModActive = false;

    public void ModifyCategoryProbability(int category)
    {
        if (isProbModActive)
            categoryProbabilty[modifiedPos] -= modifiedAmount;
        modifiedPos = category;
        modifiedAmount = probabilityIncreaseSO.Int;
        categoryProbabilty[modifiedPos] += modifiedAmount;
        isProbModActive = true;
        GenerateProbability();
        GenerateBuildingStored();
    }

    private void ProbabilityTurnCounter(object sender, EventArgs e)
    {
        if (!isProbModActive)
            return;
        currentTurn++;
        if (currentTurn == maxTurns)
        {
            isProbModActive = false;
            currentTurn = 0;
            RemoveProbabilityModification();
        }
    }

    private void RemoveProbabilityModification()
    {
        categoryProbabilty[modifiedPos] -= modifiedAmount;
        GenerateProbability();
    }
    #endregion
    #endregion

    #region Reroll
    [CollapsibleGroup("Reroll")]
    [SerializeField]
    private int rerollAmount;
    [SerializeField]
    private int currentReroll;
    [SerializeField]
    private TMP_Text rerollText;
    public void Reroll()
    {
        if (currentReroll == 0)
            return;
        currentReroll--;
        GenerateBuildingStored();
        UpdateRerollText();
    }
    public void ResetReroll()
    {
        currentReroll = rerollAmount;
        UpdateRerollText();
    }
    public void UpdateRerollText()
    {
        rerollText.text = "Reroll (" + currentReroll.ToString() + ")";
    }
    #endregion

    #region Debug
    [CollapsibleGroup("Debug")]
    [SerializeField]
    private BuildingCategory[] allCategory = new BuildingCategory[Enum.GetValues(typeof(BuildingCategory)).Length];
    
    [SerializeField]
    private StringSO debugValuesSO;
    #endregion
}
