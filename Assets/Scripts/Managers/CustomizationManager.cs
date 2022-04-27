using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationManager : MonoBehaviour {
    [SerializeField] private SkinnedMeshRenderer[] hairStyles;
    [SerializeField] private SkinnedMeshRenderer[] topStyles;
    [SerializeField] private SkinnedMeshRenderer[] pantsStyles;
    [SerializeField] private SkinnedMeshRenderer[] shoeStyles;

    [SerializeField] private Text hairText;
    [SerializeField] private Text topText;
    [SerializeField] private Text pantsText;
    [SerializeField] private Text shoeText;

    [SerializeField] private GameObject currentPlayerModel;

    private GameFlowManager gameFlowManager;

    private string[] hairNames = {"Hair", "Afro", "Middle Fringe", "Left Fringe", "Up Fringe" , "Right Fringe", "Bald" };
    private string[] topNames = {"T-Shirt", "Tank" , "No Shirt" } ;
    private string[] pantsNames = {"With pants", "No pants"};
    private string[] shoeNames = {"With shoes", "No shoes" };

    private int hairID;
    private int topID;
    private int pantsID;
    private int shoeID;

    private void Awake() {
        gameFlowManager = GameObject.FindWithTag("GameFlowManager").GetComponent<GameFlowManager>();
        SetUpAppeareance();
    }

    public void NextHairStyle() {

        if (hairID == hairStyles.Length - 1) hairID = 0;
        else hairID++;

        hairText.text = hairNames[hairID];

        Choose(hairStyles, hairID);
    }

    public void PreviousHairStyle() {
        if (hairID == 0) hairID = hairStyles.Length - 1;
        else hairID--;

        hairText.text = hairNames[hairID];

        Choose(hairStyles, hairID);
    }

    public void NextTopStyle() {
        if (topID == topStyles.Length - 1) topID = 0;
        else topID++;

        topText.text = topNames[topID];

        Choose(topStyles, topID);
    }

    public void PreviousTopStyle() {
        if (topID == 0) topID = topStyles.Length - 1;
        else topID--;

        topText.text = topNames[topID];

        Choose(topStyles, topID);
    }

    public void NextShoeStyle() {
        if (shoeID == shoeStyles.Length - 1) shoeID = 0;
        else shoeID++;

        shoeText.text = shoeNames[shoeID];

        Choose(shoeStyles, shoeID);
    }
    
    public void PreviousShoeStyle() {
        if (shoeID == 0) topID = shoeStyles.Length - 1;
        else shoeID--;

        shoeText.text = shoeNames[shoeID];

        Choose(shoeStyles, shoeID);
    }

    public void NextPantsStyle() {
        if (pantsID == pantsStyles.Length - 1) pantsID = 0;
        else pantsID++;

        pantsText.text = pantsNames[pantsID];

        Choose(pantsStyles, pantsID);
    }

    public void PreviousPantsStyle() {
        if (pantsID == 0) pantsID = pantsStyles.Length - 1;
        else pantsID--;

        pantsText.text = pantsNames[pantsID];

        Choose(pantsStyles, pantsID);
    }

    public void SavePlayer() {
        PlayerAppearance pa = new PlayerAppearance(hairID, topID, pantsID, shoeID);
        gameFlowManager.SetAppearance(pa);
    }

    private void SetUpAppeareance() {
        if (GameFlowManager.GetAppearance() == null) {
            Debug.Log("It is null");
            return;
        }
        PlayerAppearance appearance = GameFlowManager.GetAppearance();
        //SetUp Hair
        hairID = appearance.HairID;
        Choose(hairStyles, hairID);
        hairText.text = hairNames[hairID];

        //SetUp Top
        topID = appearance.TopID;
        Choose(topStyles, topID);
        topText.text = topNames[topID];


        //SetUp Pants
        pantsID = appearance.PantsID;
        Choose(pantsStyles, pantsID);
        pantsText.text = pantsNames[pantsID];

        //SetUp Shoes
        shoeID = appearance.ShoesID;
        Choose(shoeStyles, shoeID);
        shoeText.text = shoeNames[shoeID];
    
    }

    private void Choose(SkinnedMeshRenderer[] array, int index) {
      

        array[index].enabled = true;

        for (int i = 0; i < array.Length; i++) {
            if (i != index) array[i].enabled = false;
        }
    }
}
