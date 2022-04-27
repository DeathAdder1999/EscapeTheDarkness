using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

public class AdsManager : MonoBehaviour {
    private static bool created = false;

    private void Awake() {
        if (created) {
            Destroy(this.gameObject);
        } else {
            created = true;
            DontDestroyOnLoad(this.gameObject);
        }

    }

#if UNITY_ADS
    public static void ShowRewardedAdd(int mode) {// 0 - shop, 1 - game
        DateManager.VerifyDate();
        if (mode == 0 && !DateManager.AdAvailable) {
            Debug.Log("Ad is not available");
            return;
        }
        Debug.Log("Want to show a video");

        if (Advertisement.IsReady("rewardedVideo")) {
            ShowOptions options;

            if (mode == 0) {
                options = new ShowOptions { resultCallback = HandleShowResultShop };
                DateManager.UpdateTime();
            } else
                options = new ShowOptions { resultCallback = HandleShowResultGame };

            Advertisement.Show("rewardedVideo", options);
        } 
  
    }

    private static void HandleShowResultShop(ShowResult result) {
        switch (result) {
            case ShowResult.Finished:
                GameFlowManager.currency += 1000;
                break;
        }
    }

    private static void HandleShowResultGame(ShowResult result) {
        switch (result) {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                DeathManager.videoStatus = 1;
                break;
            default:
                DeathManager.videoStatus = -2;
                break;
        }
    }
#endif

}

