using System.Collections;
using System.Collections.Generic;

public class PlayerData { 

    //IMPORTANT Game data
    private int currency;
    private int highScoreDarkness;
    private int highScoreBlinkingLights;
    private PlayerAppearance appearance;
    private string securityKey;

    public int Currency { get { return currency; } }
    public int HighScoreDarkness { get { return highScoreDarkness; } }
    public int HighScoreBlinkinghLights { get { return highScoreBlinkingLights; } }
    public PlayerAppearance Appearance { get { return appearance; } }
    public string SecurityKey { get { return securityKey; } }

    public PlayerData(int currency,int highScoreBlinkingLights, int highScoreDarkness, PlayerAppearance appearance) { //Used to save data
        this.currency = currency;
        this.highScoreDarkness = highScoreDarkness;
        this.highScoreBlinkingLights = highScoreBlinkingLights;
        this.appearance = appearance;

        

        SetKey();
    }

    public string GetKey() {
        return securityKey;
    }

    public bool ValidateKey(string keyToValidate) {
        if (keyToValidate == securityKey) return true;

        return false;
    }

    public void SetKey() {
        securityKey ="" + ("" + currency).GetHashCode() + ("" + highScoreBlinkingLights).GetHashCode() + ("" + highScoreDarkness).GetHashCode();
    }
}
