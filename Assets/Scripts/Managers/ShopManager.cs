using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour {
    #region VARIABLES
    [SerializeField] private Text moneyText;
    [SerializeField] private Text fixTheLight;
    [SerializeField] private Text secondChance;
    [SerializeField] private Text construction;
    [SerializeField] private Text doubleMoney;
    [SerializeField] private Text countMinute;
    [SerializeField] private Text countSecond;
    [SerializeField] private GameObject addCurrencyPanel;
    [SerializeField] private GameObject timeCountDownPanel;
    [SerializeField] private GameObject addCurrencyButton;

    //Items
    [SerializeField] private GameObject fixTheLightPanel;
    [SerializeField] private GameObject constructionPanel;
    Text[] textChildrenLight;
    Image[] imgChildrenLight;
    Text[] textChildrenConstruction;
    Image[] imgChildrenConstruction;
    private int currency;
    #endregion

    private void Awake() {
        //  addCurrencyPanel.SetActive(false);

        textChildrenLight = fixTheLightPanel.GetComponentsInChildren<Text>();
        imgChildrenLight = fixTheLightPanel.GetComponentsInChildren<Image>();
        textChildrenConstruction = constructionPanel.GetComponentsInChildren<Text>();
        imgChildrenConstruction = constructionPanel.GetComponentsInChildren<Image>();

        UpdateIcons();

        moneyText.text = GameFlowManager.currency + "";
        currency = GameFlowManager.currency;
        DisableAddCurrency();
    }

    private void Update() {
        if (currency != GameFlowManager.currency) {
            currency = GameFlowManager.currency;
            moneyText.text = currency + "";
        }

        UpdateIcons();

        fixTheLight.text = "BOUGHT: " + Inventory.fixTheLight + "";
        secondChance.text = "BOUGHT: " + Inventory.secondChance + "";
        construction.text = "BOUGHT: " + Inventory.construction + "";
        doubleMoney.text = "BOUGHT: " + Inventory.doubleMoney + "";
    }

    #region SETTING UP ICONS
    private void UpdateIcons() {
        if (Inventory.fixTheLight > 0) MuteConstruction();
        else if (Inventory.construction > 0) MuteFixTheLight();
    }

    private void MuteFixTheLight() {
        foreach (Text child in textChildrenLight) {
            var color = child.color;
            color.a = 0.5f;
            child.color = color;
        }

        foreach (Image child in imgChildrenLight) {
            var color = child.color;
            color.a = 0.2f;
            child.color = color;
        }
    }

    private void MuteConstruction() {
        foreach (Text child in textChildrenConstruction) {
            var color = child.color;
            color.a = 0.5f;
            child.color = color;
        }

        foreach (Image child in imgChildrenConstruction) {
            var color = child.color;
            color.a = 0.2f;
            child.color = color;
        }


    }

    #endregion
    #region PURCHASING
    private bool ValidateTransaction(int price) {
        if (price > GameFlowManager.currency) return false;

        GameFlowManager.currency -= price;

        return true;
    }

    public void BuyFixTheLight() {
        if (Inventory.fixTheLight > 0 || Inventory.construction > 0) return;
        if (ValidateTransaction(3000)) Inventory.fixTheLight++;
    }

    public void BuyConstruction() {
        if (Inventory.construction > 0 || Inventory.fixTheLight > 0) return;
        if (ValidateTransaction(3000)) Inventory.construction++;
    }

    public void BuySecondChance() {
        if (ValidateTransaction(1000)) Inventory.secondChance++;
    }

    public void BuyDoubleMoney() {
        if (ValidateTransaction(1000)) Inventory.doubleMoney++;
    }
    #endregion

    #region ADDING_CURRENCY

    public void EnableAddCurrency() {
        DateManager.VerifyDate();
        if (DateManager.AdAvailable)
            addCurrencyPanel.SetActive(true);
        else
            EnableCountdownPanel();
    }

    public void DisableAddCurrency() {
        addCurrencyPanel.SetActive(false);
        DisableCountDownPanel();
    }

    public void WatchVideo() {
        DisableAddCurrency();
        AdsManager.ShowRewardedAdd(0);
    }

    private void EnableCountdownPanel() {
        timeCountDownPanel.SetActive(true);
        StartCoroutine(StartCountdown());
    }

    private void DisableCountDownPanel() {
        timeCountDownPanel.SetActive(false);
    }

    private IEnumerator StartCountdown() {
        int secondsLeft = DateManager.GetTimeLeft();
        Debug.Log("Seconds Left: " + secondsLeft);
        while(secondsLeft != 0) {
            secondsLeft--;
            countMinute.text = "" +  (secondsLeft / 60);
            countSecond.text = secondsLeft % 60 + "";

            yield return new WaitForSeconds(1.0f);
        }

        DisableCountDownPanel();
        addCurrencyPanel.SetActive(true);
        
    }


    #endregion

}
