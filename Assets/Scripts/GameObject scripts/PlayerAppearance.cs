using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAppearance {

    private int hairID;
    private int topID;
    private int pantsID;
    private int shoesID;

    public int HairID { get { return hairID; } }
    public int TopID { get { return topID; } }
    public int PantsID { get { return pantsID; } }
    public int ShoesID { get { return pantsID; } }

    public PlayerAppearance(int hairID, int topID, int pantsID, int shoesID) {
        this.hairID = hairID;
        this.topID = topID;
        this.pantsID = pantsID;
        this.shoesID = shoesID;
    }

    public override string ToString() {
        return "hairID: " + hairID + "\n topID: " + topID + "\n pantsID: " + pantsID + "\n shoesID: " + shoesID;
    }
}
