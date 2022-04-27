using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    private static bool created = false;
    public static int fixTheLight;
    public static int construction;
    public static int secondChance;
    public static int doubleMoney;
    private static string securityKey;

    private void Awake() {
        if (created) {
            DestroyImmediate(this.gameObject);
        } else {
            DontDestroyOnLoad(this.gameObject);
            created = true;
            LoadItems();
        }
    }

    private void LoadItems() {
        if (!PlayerPrefs.HasKey("Items")) { //No previous saving
            fixTheLight = 0;
            construction = 0;
            secondChance = 0;
            doubleMoney = 0;
            SetSecurityKey();
            return;
        }

        int light = PlayerPrefs.GetInt("FixTheLight");
        int constr = PlayerPrefs.GetInt("Construction");
        int chance = PlayerPrefs.GetInt("SecondChance");
        int money = PlayerPrefs.GetInt("DoubleMoney");
        string loadedKey = PlayerPrefs.GetString("InventorySecurity");

        //Verify security key
        string key = "" + Hash(light) + Hash(constr) + Hash(chance) + Hash(money);

        if(loadedKey != key) {
            fixTheLight = 0;
            construction = 0;
            secondChance = 0;
            doubleMoney = 0;

        } else {
            fixTheLight = light;
            construction = constr;
            secondChance = chance;
            doubleMoney = money;
        }
        SetSecurityKey();
    }

    private void SaveItems() {
        SetSecurityKey();
        PlayerPrefs.SetInt("Items", 1);
        PlayerPrefs.SetInt("FixTheLight", fixTheLight);
        PlayerPrefs.SetInt("Construction", construction);
        PlayerPrefs.SetInt("SecondChance", secondChance);
        PlayerPrefs.SetInt("DoubleMoney", doubleMoney);
        PlayerPrefs.SetString("InventorySecurity", securityKey);
    }

    private void SetSecurityKey() {
        securityKey = "" + Hash(fixTheLight) + Hash(construction) + Hash(secondChance) + Hash(doubleMoney);
    }

    private int Hash(int val) {
        return ("" + val).GetHashCode();
    }

    private void OnApplicationQuit() {
        SaveItems();
    }

}
