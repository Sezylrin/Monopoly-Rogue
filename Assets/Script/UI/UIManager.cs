using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        currentSubTurnSO.onValueChanged += TurnsLeftTillTax;
        taxAmountSO.onValueChanged += TurnsLeftTillTax;
        isOpenCustomDiceUiSO.onValueChanged += OpenCustomDiceUI;
        IsOpenBuildMenuUI.onValueChanged += OpenBuildUiItem;
        IsDisableButtonSO.onValueChanged += DisableButton;
        IsDisableButtonSO.onValueChanged += EnableButtons;
        IsOpenProbabilityUiSO.onValueChanged += OpenProbabilityUI;
    }
    void Start()
    {
        allButtons = GetComponentsInChildren<Button>(true);
    }

    // Update is called once per frame
    void Update()
    {

    }
    #region Disable Buttons
    [CollapsibleGroup("Disable Buttons")]
    public BoolSO IsDisableButtonSO;
    [SerializeField]
    private Button[] allButtons;
    private List<Button> alreadyDisabled = new List<Button>();
    private void DisableButton(object sender, EventArgs e)
    {
        if (!IsDisableButtonSO.Bool)
            return;
        for (int i = 0; i < allButtons.Length; i++)
        {
            if (!allButtons[i].enabled)
            {
                alreadyDisabled.Add(allButtons[i]);
            }
            allButtons[i].enabled = false;
        }
    }

    private void EnableButtons(object sender, EventArgs e)
    {
        if (IsDisableButtonSO.Bool)
            return;
        for (int i = 0; i < allButtons.Length; i++)
        {
            allButtons[i].enabled = true;
        }
        for (int i = 0; i < alreadyDisabled.Count; i++)
        {
            alreadyDisabled[i].enabled = false;
        }
        alreadyDisabled.Clear();

    }
    #endregion

    #region Roll Button
    [CollapsibleGroup("Roll Button")]
    [SerializeField]
    private BoolSO isToRollSO;
    [SerializeField]
    private GameObject rollUIObj, openBuildMenuObj, collectCashObj;
    [SerializeField]
    private Button buildMenuButton;
    //called by unity button event
    public void RollButtonUI()
    {
        isToRollSO.Bool = true;
        rollUIObj.SetActive(false);
        openBuildMenuObj.SetActive(true);
        collectCashObj.SetActive(true);
    }
    #endregion

    #region Build Menu
    [CollapsibleGroup("Build Menu")]
    [SerializeField]
    private GameObject buildMenuCanvas;
    [Header("Development Item")]
    [SerializeField]
    private BoolSO IsOpenBuildMenuUI;
    [SerializeField]
    private RaritySO developmentRarity;
    [SerializeField]
    private GameObject rerollButtonOBJ;
    //called by unity button event
    public void OpenBuildUI()
    {
        if (TileGrid.Instance.IsTileProtected())
            return;
        buildMenuCanvas.SetActive(true);
        openBuildMenuObj.SetActive(false);
        collectCashObj.SetActive(false);
    }

    //called by unity button event
    public void CloseBuildUI()
    {
        buildMenuCanvas.SetActive(false);
        if (IsOpenBuildMenuUI.Bool)
        {
            rerollButtonOBJ.SetActive(true);
            IsOpenBuildMenuUI.Bool = false;
            roller.ResetCardToStoredSO();
            return;
        }
        openBuildMenuObj.SetActive(true);
        collectCashObj.SetActive(true);
    }
    //called by Unity button event
    public void OnBuildingSelected()
    {
        if (!IsOpenBuildMenuUI.Bool)
            buildMenuButton.enabled = false;
        CloseBuildUI();
    }

    private void OpenBuildUiItem(object sender, EventArgs e)
    {
        if (!IsOpenBuildMenuUI.Bool)
            return;
        roller.GenerateBuildingTemp(developmentRarity.Rarity);
        buildMenuCanvas.SetActive(true);
        rerollButtonOBJ.SetActive(false);
    }
    #endregion

    #region Collect Money
    [CollapsibleGroup("Collect Money")]
    [SerializeField]
    private BoolSO IsCollectCashSO;
    [SerializeField]
    private BoolSO IsTurnChangeSO;
    [SerializeField]
    private BoolSO CanRollSO;
    [SerializeField]
    private BuildingRoller roller;
    //being called by unity button event
    public void OnCollectCash()
    {
        IsCollectCashSO.Bool = true;
        buildMenuButton.enabled = true;
        openBuildMenuObj.SetActive(false);
        collectCashObj.SetActive(false);
        rollUIObj.SetActive(true);

        IsTurnChangeSO.Bool = true;
        CanRollSO.Bool = true;
        roller.GenerateBuildingStored();
    }
    #endregion

    #region Turns till tax
    [CollapsibleGroup("Turns till tax")]
    [SerializeField]
    private TMP_Text taxText;
    [SerializeField]
    private IntSO currentSubTurnSO;
    [SerializeField]
    private IntSO taxAmountSO;

    public void TurnsLeftTillTax(object sender, EventArgs e)
    {
        if (currentSubTurnSO.Int != 0)
            taxText.text = taxAmountSO.Int.ToString() + " required in " + currentSubTurnSO.Int.ToString() + " turns";
        else
            taxText.text = taxAmountSO.Int.ToString() + " required this turn";

    }
    #endregion

    #region Custom Dice
    [CollapsibleGroup("Custom Dice UI")]
    [SerializeField]
    private GameObject customDiceUIOBJ;
    [SerializeField]
    private BoolSO isOpenCustomDiceUiSO;

    private void OpenCustomDiceUI(object sender, EventArgs e)
    {
        customDiceUIOBJ.SetActive(isOpenCustomDiceUiSO.Bool);
    }
    #endregion

    #region Probability
    [CollapsibleGroup("Probability")]
    [SerializeField]
    private BoolSO IsOpenProbabilityUiSO;
    [SerializeField]
    private GameObject probabilityUIOBJ;
    private void OpenProbabilityUI(object sedner, EventArgs e)
    {
        probabilityUIOBJ.SetActive(IsOpenProbabilityUiSO.Bool);
    }
    #endregion
}
